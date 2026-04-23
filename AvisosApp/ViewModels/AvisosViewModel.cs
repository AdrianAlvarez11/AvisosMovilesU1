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

        public ICommand CambiarAGeneralesAlumnoCommand { get; set; }
        public ICommand CambiarAPersonalesAlumnoCommand { get; set; }

        public ICommand CambiarAGrupoCommand { get; set; }
        public ICommand CambiarAGeneralesCommand { get; set; }

        public AvisosViewModel()
        {
            LoginCommand = new Command(Login);
            LogoutCommand = new Command(Logout);
            VistaRegistrarCommand = new Command(() => Shell.Current.GoToAsync("//registrar"));
            VistaLoginCommand = new Command(() => Shell.Current.GoToAsync("//login"));

            RegistrarMaestroCommand = new Command(RegistrarMaestro);
            RegistrarAlumnoCommand = new Command(RegistrarAlumno);

            CargarGrupoCommand = new Command(CargarGrupo);
            IrRegistrarAlumnoCommand = new Command(() => Shell.Current.GoToAsync("//registrarAlumno"));
            VerAlumnoCommand = new Command<int>(GetAlumno);
            EliminarAlumnoCommand = new Command<int>(EliminarAlumno);

            CargarAvisosPersonalesCommand = new Command(CargarAvisosPersonales);
            VerAvisosPersonalesCommand = new Command<int>(VerDetalleAvisosPersonales);
            EliminarAvisosPersonalesCommand = new Command<int>(EliminarAvisosPersonales);

            IrCrearAvisoGeneralCommand = new Command(() => Shell.Current.GoToAsync("//crearavisogeneral"));
            CrearAvisoGeneralCommand = new Command(CrearAvisoGeneral);

            IrCrearAvisoPersonalCommand = new Command(() => Shell.Current.GoToAsync("//crearavisopersonal"));
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
        }

        private void Logout()
        {
            service.Logout();
            NumControl = "";
            Contrasena = "";
            Shell.Current.GoToAsync("//login");
        }

        private async void Login()
        {
            var response = await service.Login(new LoginDTO
            {
                NumControl = NumControl,
                Contrasena = Contrasena
            });

            if (response != null)
            {
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


        public MaestroRegistroDTO Maestro { get; set; } = new();
        public AlumnoRegistroDTO Alumno { get; set; } = new();

        public ICommand RegistrarMaestroCommand { get; set; }
        public ICommand RegistrarAlumnoCommand { get; set; }

        private async void RegistrarMaestro()
        {
            var resonse = await service.RegistrarMaestro(Maestro);

            if (resonse)
                await Shell.Current.GoToAsync("//login");
        }

        private async void RegistrarAlumno()
        {
            var response = await service.RegistrarAlumno(Alumno);

            if (response)
            {
                CargarGrupo();
                await Shell.Current.GoToAsync("//homemaestro");

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
                await Shell.Current.GoToAsync("//detallesalumnos");

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
            }
        }



        public ObservableCollection<AvisoPersonalResumenDTO> Avisos { get; set; } = new();
        private List<AvisoPersonalResumenDTO> lista = new();
        public AvisoPersonalCreateDTO AvisoPersonal { get; set; } = new();

        public AvisoPersonalDetalleDTO? AvisoSeleccionado { get; set; }

        public ICommand CrearAvisoPersonalCommand { get; set; }
        public ICommand CargarAvisosPersonalesCommand { get; set; }
        public ICommand VerAvisosPersonalesCommand { get; set; }
        public ICommand EliminarAvisosPersonalesCommand { get; set; }

        private async void CrearAvisoPersonal()
        {
            AvisoPersonal.IdAlumno = AlumnoSeleccionado.Id;
            var response = await service.Crear(AvisoPersonal);
            if (response)
            {
                CargarAvisosPersonales();
                await Shell.Current.GoToAsync("//homemaestro");
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

                await Shell.Current.GoToAsync("//detalleavisopersonal");
            }
        }

        private async void EliminarAvisosPersonales(int id)
        {
            var response = await service.Eliminar(id);
            if (response)
            {
                var aviso = Avisos.FirstOrDefault(x => x.Id == id);
                if (aviso != null) Avisos.Remove(aviso);
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
            var response = await service.Crear(AvisoGeneral);
            if (response)
            {
                CargarAvisosGenerales();
                await Shell.Current.GoToAsync("//homemaestro");
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
