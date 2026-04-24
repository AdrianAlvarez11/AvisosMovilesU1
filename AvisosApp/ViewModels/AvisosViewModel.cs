using AvisosApp.Models.DTOs;
using AvisosApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using static AvisosApp.Models.DTOs.AuthDTOs;
using static AvisosApp.Models.DTOs.RegistroDTOs;

namespace AvisosApp.ViewModels
{
    public class AvisosViewModel : INotifyPropertyChanged
    {
        AvisosService service = new();

        public event PropertyChangedEventHandler? PropertyChanged;

        private bool isBusy;
        public bool IsBusy
        {
            get => isBusy;
            set
            {
                isBusy = value;
                PropertyChanged?.Invoke(this, new(nameof(IsBusy)));
            }
        }

        private string? error;
        public string? Error
        {
            get => error;
            set
            {
                error = value;
                PropertyChanged?.Invoke(this, new(nameof(Error)));
                PropertyChanged?.Invoke(this, new(nameof(HayError)));
            }
        }
        
        public bool HayError => !string.IsNullOrEmpty(Error);

        private string nombreUsuario = "";
        public string NombreUsuario
        {
            get => nombreUsuario;
            set
            {
                nombreUsuario = value;
                PropertyChanged?.Invoke(this, new(nameof(NombreUsuario)));
            }
        }

        private string grupoUsuario = "";
        public string GrupoUsuario
        {
            get => grupoUsuario;
            set
            {
                grupoUsuario = value;
                PropertyChanged?.Invoke(this, new(nameof(GrupoUsuario)));
            }
        }

        private void MostrarError(Exception ex)
        {
            if (ex is System.Net.Http.HttpRequestException)
            {
                Error = "No se pudo conectar con el servidor. Verifica tu conexión a internet o intenta más tarde.";
            }
            else
            {
                Error = ex.Message;
            }
        }

        private string numControl = "";
        public string NumControl
        {
            get => numControl;
            set
            {
                numControl = value;
                PropertyChanged?.Invoke(this, new(nameof(NumControl)));
            }
        }

        private string contrasena = "";
        public string Contrasena
        {
            get => contrasena;
            set
            {
                contrasena = value;
                PropertyChanged?.Invoke(this, new(nameof(Contrasena)));
            }
        }

        public ICommand LoginCommand { get; set; }
        public ICommand LogoutCommand { get; set; }
        public ICommand VistaRegistrarCommand { get; set; }
        public ICommand VistaLoginCommand { get; set; }

        private bool mostrarGrupo = true;
        public bool MostrarGrupo
        {
            get => mostrarGrupo;
            set
            {
                mostrarGrupo = value;
                PropertyChanged?.Invoke(this, new(nameof(MostrarGrupo)));
                PropertyChanged?.Invoke(this, new(nameof(MostrarGenerales)));
            }
        }


        public bool MostrarGenerales => !MostrarGrupo;

        // Propiedades para Alumno
        private bool mostrarAvisosGeneralesAlumno = true;
        public bool MostrarAvisosGeneralesAlumno
        {
            get => mostrarAvisosGeneralesAlumno;
            set
            {
                mostrarAvisosGeneralesAlumno = value;
                PropertyChanged?.Invoke(this, new(nameof(MostrarAvisosGeneralesAlumno)));
                PropertyChanged?.Invoke(this, new(nameof(MostrarAvisosPersonalesAlumno)));
            }
        }
        public bool MostrarAvisosPersonalesAlumno => !MostrarAvisosGeneralesAlumno;

        private bool hayLeidosGenerales = false;
        public bool HayLeidosGenerales
        {
            get => hayLeidosGenerales;
            set
            {
                hayLeidosGenerales = value;
                PropertyChanged?.Invoke(this, new(nameof(HayLeidosGenerales)));
            }
        }
        private bool hayPendientesGenerales = false;
        public bool HayPendientesGenerales
        {
            get => hayPendientesGenerales;
            set
            {
                hayPendientesGenerales = value;
                PropertyChanged?.Invoke(this, new(nameof(HayPendientesGenerales)));
            }
        }
        private bool propiedadDelProfesor = false;
        public bool PropiedadDelProfesor
        {
            get => propiedadDelProfesor;
            set
            {
                propiedadDelProfesor = value;
                PropertyChanged?.Invoke(this, new(nameof(PropiedadDelProfesor)));
            }
        }

        
        public ICommand CambiarAGeneralesAlumnoCommand { get; set; }
        public ICommand CambiarAPersonalesAlumnoCommand { get; set; }
        public ICommand RefrescarAvisosAlumnoCommand { get; set; }

        public ICommand CambiarAGrupoCommand { get; set; }
        public ICommand CambiarAGeneralesCommand { get; set; }

        public AvisosViewModel()
        {
            LoginCommand = new Command(Login);
            LogoutCommand = new Command(Logout);
            VistaRegistrarCommand = new Command(() => 
            {
                Error = "";
                Maestro = new();
                PropertyChanged?.Invoke(this, new(nameof(Maestro)));
                Shell.Current.GoToAsync("//registrar");
            });
            VistaLoginCommand = new Command(() => 
            {
                Error = "";
                NumControl = "";
                Contrasena = "";
                Shell.Current.GoToAsync("//login");
            });

            RegistrarMaestroCommand = new Command(RegistrarMaestro);
            RegistrarAlumnoCommand = new Command(RegistrarAlumno);

            CargarGrupoCommand = new Command(CargarGrupo);
            IrRegistrarAlumnoCommand = new Command(() => 
            {
                Error = "";
                Alumno = new();
                PropertyChanged?.Invoke(this, new(nameof(Alumno)));
                Shell.Current.GoToAsync("//registrarAlumno");
            });
            VerAlumnoCommand = new Command<int>(GetAlumno);
            EliminarAlumnoCommand = new Command<int>(EliminarAlumno);

            CargarAvisosPersonalesCommand = new Command(CargarAvisosPersonales);
            VerAvisosPersonalesCommand = new Command<int>(VerDetalleAvisosPersonales);
            VerAvisosPersonalesMaestroCommand = new Command<int>(VerDetalleAvisosPersonalesMaestro);
            EliminarAvisosPersonalesCommand = new Command<int>(EliminarAvisosPersonales);

            IrCrearAvisoGeneralCommand = new Command(() => 
            {
                Error = "";
                AvisoGeneral = new();
                PropertyChanged?.Invoke(this, new(nameof(AvisoGeneral)));
                Shell.Current.GoToAsync("crearavisogeneral");
            });
            CrearAvisoGeneralCommand = new Command(CrearAvisoGeneral);

            IrCrearAvisoPersonalCommand = new Command(CambiarACrearAvisoPersonal);
            CrearAvisoPersonalCommand = new Command(CrearAvisoPersonal);

            CargarAvisosGeneralesCommand = new Command(CargarAvisosGenerales);
            VerAvisosGeneralesCommand = new Command<int>(VerDetalleAvisosGenerales);
            EliminarAvisosGeneralesCommand = new Command<int>(EliminarAvisosGenerales);

            VerAvisosGeneralesAlumnoCommand = new Command<int>(VerDetalleAvisosGeneralesAlumno);

            RegresarMaestroCommand = new Command(() => Shell.Current.GoToAsync("//homemaestro"));
            RegresarAlumnoCommand = new Command(() => Shell.Current.GoToAsync("//homealumno"));

            CambiarAGrupoCommand = new Command(() =>
            {
                MostrarGrupo = true;
                CargarGrupo();
            });

            CambiarAGeneralesCommand = new Command(() =>
            {
                MostrarGrupo = false;
                CargarAvisosGenerales();
            });

            CambiarAGeneralesAlumnoCommand = new Command(() =>
            {
                MostrarAvisosGeneralesAlumno = true;
                CargarAvisosGeneralesAlumno();
            });

            CambiarAPersonalesAlumnoCommand = new Command(() =>
            {
                MostrarAvisosGeneralesAlumno = false;
                CargarAvisosPersonales();
            });

            RefrescarAvisosAlumnoCommand = new Command(() =>
            {
                if (MostrarAvisosGeneralesAlumno)
                {
                    CargarAvisosGeneralesAlumno();
                }
                else
                {
                    CargarAvisosPersonales();
                }
            });
        }

        public void CambiarACrearAvisoPersonal()
        {
            Error = "";
            AvisoPersonal = new();
            PropertyChanged?.Invoke(this, new(nameof(AvisoPersonal)));
            Shell.Current.GoToAsync("crearavisopersonal");
        }
        private void Logout()
        {
            service.Logout();
            NombreUsuario = "";
            GrupoUsuario = "";
            NumControl = "";
            Contrasena = "";
            Shell.Current.GoToAsync("//login");
        }

        private async void Login()
        {
            try
            {
                IsBusy = true;
                Error = "";
                var response = await service.Login(new LoginDTO
                {
                    NumControl = NumControl,
                    Contrasena = Contrasena
                });

                if (response != null)
                {
                    NombreUsuario = response.Nombre;
                    GrupoUsuario = response.NombreGrupo;

                    if (response.Rol == "Maestro")
                    {
                        await Shell.Current.GoToAsync("//homemaestro");
                        CargarGrupo();
                    }
                    else if (response.Rol == "Alumno")
                    {
                        await Shell.Current.GoToAsync("//homealumno");
                        CargarAvisosGeneralesAlumno();
                        MostrarAvisosGeneralesAlumno = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }


        public MaestroRegistroDTO Maestro { get; set; } = new();
        public AlumnoRegistroDTO Alumno { get; set; } = new();

        public ICommand RegistrarMaestroCommand { get; set; }
        public ICommand RegistrarAlumnoCommand { get; set; }

        private async void RegistrarMaestro()
        {
            try
            {
                Error = "";
                var resonse = await service.RegistrarMaestro(Maestro);

                if (resonse)
                    await Shell.Current.GoToAsync("//login");
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        private async void RegistrarAlumno()
        {
            try
            {
                Error = "";
                var response = await service.RegistrarAlumno(Alumno);

                if (response)
                {
                    CargarGrupo();
                    await Shell.Current.GoToAsync("//homemaestro");
                }
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }



        public GrupoDetalleDTO? Grupo { get; set; }
        public ObservableCollection<AlumnoResumenDTO> Alumnos { get; set; } = new();
        public AlumnoDetalleDTO? AlumnoSeleccionado { get; set; }

        public ICommand IrRegistrarAlumnoCommand { get; set; }
        public ICommand IrCrearAvisoGeneralCommand { get; set; }
        public ICommand IrCrearAvisoPersonalCommand { get; set; }
        public ICommand CargarGrupoCommand { get; set; }
        public ICommand VerAlumnoCommand { get; set; }
        public ICommand EliminarAlumnoCommand { get; set; }

        //propiedad para controlar el eliminar de avisos generales

        private async void CargarGrupo()
        {

            var grupo = await service.GetGrupo();
            if (grupo != null)
            {
                Grupo = grupo;
                PropertyChanged?.Invoke(this, new(nameof(Grupo)));

                Alumnos.Clear();
                grupo.Alumnos.ForEach(Alumnos.Add);
            }

        }

        private async void GetAlumno(int id)
        {
            var alumno = await service.GetAlumno(id);
            if (alumno != null)
            {  
                AlumnoSeleccionado = alumno;
                PropertyChanged?.Invoke(this, new(nameof(AlumnoSeleccionado)));
                await Shell.Current.GoToAsync("detallesalumnos");

            }
        }

        private async void EliminarAlumno(int id)
        {
            var response = await service.EliminarAlumno(id);
            if (response)
            {
                var alumno = Alumnos.FirstOrDefault(x => x.Id == id);

                if (alumno != null)
                    Alumnos.Remove(alumno);

                CargarGrupo();
                await Shell.Current.GoToAsync("//homemaestro");

            }
        }



        public ObservableCollection<AvisoPersonalResumenDTO> Avisos { get; set; } = new();
        private List<AvisoPersonalResumenDTO> lista = new();
        public AvisoPersonalCreateDTO AvisoPersonal { get; set; } = new();

        public AvisoPersonalDetalleDTO? AvisoSeleccionado { get; set; }

        public ICommand CrearAvisoPersonalCommand { get; set; }
        public ICommand CargarAvisosPersonalesCommand { get; set; }
        public ICommand VerAvisosPersonalesCommand { get; set; }
        public ICommand VerAvisosPersonalesMaestroCommand { get; set; }
        public ICommand EliminarAvisosPersonalesCommand { get; set; }

        private async void CrearAvisoPersonal()
        {
            try
            {
                Error = "";
                AvisoPersonal.IdAlumno = AlumnoSeleccionado.Id;
                var response = await service.Crear(AvisoPersonal);
                if (response)
                {
                    CargarAvisosPersonales();
                    await Shell.Current.GoToAsync("//homemaestro");
                }
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }
        private async void CargarAvisosPersonales()
        {

            var avisosPersonales = await service.GetMisAvisos();
            lista = avisosPersonales;

            Avisos.Clear();
            avisosPersonales.ForEach(Avisos.Add);

        }

        private async void VerDetalleAvisosPersonales(int id)
        {
            var aviso = await service.GetDetalle(id);
            if (aviso != null)
            {
                AvisoSeleccionado = aviso;
                PropertyChanged?.Invoke(this, new(nameof(AvisoSeleccionado)));

                await Shell.Current.GoToAsync("///detalleavisopersonal");
            }
        }

        private async void VerDetalleAvisosPersonalesMaestro(int id)
        {
            var aviso = await service.GetDetalleAvisoPersonalMaestro(id);
            if (aviso != null)
            {
                AvisoSeleccionado = aviso;
                PropertyChanged?.Invoke(this, new(nameof(AvisoSeleccionado)));

                await Shell.Current.GoToAsync("///detalleavisopersonalmaestro");
            }
        }

        private async void EliminarAvisosPersonales(int id)
        {
            var response = await service.Eliminar(id);
            if (response)
            {
                var aviso = Avisos.FirstOrDefault(x => x.Id == id);

                if (aviso != null) 
                    Avisos.Remove(aviso);

                GetAlumno(AlumnoSeleccionado.Id);

            }
        }

        public ObservableCollection<AvisoGeneralResumenDTO> AvisosGenerales { get; set; } = new();
        private List<AvisoGeneralResumenDTO> listaGeneral = new();
        public AvisoGeneralCreateDTO AvisoGeneral { get; set; } = new();
        public AvisoGeneralDetalleMaestroDTO? SeleccionadoGeneral { get; set; }
        public AvisoGeneralDetalleAlumnoDTO? SeleccionadoGeneralAlumno { get; set; }

        public ICommand CrearAvisoGeneralCommand { get; set; }
        public ICommand CargarAvisosGeneralesCommand { get; set; }
        public ICommand VerAvisosGeneralesCommand { get; set; }
        public ICommand EliminarAvisosGeneralesCommand { get; set; }
        
        public ICommand VerAvisosGeneralesAlumnoCommand { get; set; }

        public ICommand RegresarMaestroCommand { get; set; }
        public ICommand RegresarAlumnoCommand { get; set; }

        private async void CrearAvisoGeneral()
        {
            try
            {
                Error = "";
                var response = await service.Crear(AvisoGeneral);
                if (response)
                {
                    CargarAvisosGenerales();
                    await Shell.Current.GoToAsync("//homemaestro");
                }
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }
        
        private async void CargarAvisosGeneralesAlumno()
        {
            var avisosGenerales = await service.GetAvisosGeneralesAlumno();
            listaGeneral = avisosGenerales;

            AvisosGenerales.Clear();
            avisosGenerales.ForEach(AvisosGenerales.Add);
        }

        private async void CargarAvisosGenerales()
        {

            var avisosGenerales = await service.GetAvisos();
            listaGeneral = avisosGenerales;

            AvisosGenerales.Clear();
            avisosGenerales.ForEach(AvisosGenerales.Add);

        }

        private async void VerDetalleAvisosGenerales(int id)
        {
            var aviso = await service.GetDetalleMaestro(id);
            
            if (aviso != null)
            {
                SeleccionadoGeneral = aviso;
                PropertyChanged?.Invoke(this, new(nameof(SeleccionadoGeneral)));

                PropiedadDelProfesor = aviso.EsProfesor;

                HayLeidosGenerales = aviso.Leidos != null && aviso.Leidos.Count > 0;
                HayPendientesGenerales = aviso.PendientesLectura != null && aviso.PendientesLectura.Count > 0;

                await Shell.Current.GoToAsync("//detalleavisogeneral");
            }
        }
        
        private async void VerDetalleAvisosGeneralesAlumno(int id)
        {
            var aviso = await service.GetDetalleAlumno(id);
            if (aviso != null)
            {
                SeleccionadoGeneralAlumno = aviso;
                PropertyChanged?.Invoke(this, new(nameof(SeleccionadoGeneralAlumno)));

                await Shell.Current.GoToAsync("//detalleavisoalumno");
            }
        }

        private async void EliminarAvisosGenerales(int id)
        {
            var response = await service.EliminarAvisoGeneral(id);
            if (response)
            {
                var item = AvisosGenerales.FirstOrDefault(x => x.Id == id);
                if (item != null)
                    AvisosGenerales.Remove(item);
            }
        }
    }
}
