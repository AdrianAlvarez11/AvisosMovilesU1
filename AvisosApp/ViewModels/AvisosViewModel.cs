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

        public string NumControl { get; set; }
        public string Contrasena { get; set; }

        public ICommand LoginCommand { get; set; }
        public ICommand VistaRegistrarCommand { get; set; }
        public ICommand VistaLoginCommand { get; set; }

        public AvisosViewModel()
        {
            LoginCommand = new Command(Login);
            VistaRegistrarCommand = new Command(() => Shell.Current.GoToAsync("//registrar"));
            VistaLoginCommand = new Command(() => Shell.Current.GoToAsync("//login"));

            RegistrarMaestroCommand = new Command(RegistrarMaestro);
            RegistrarAlumnoCommand = new Command(RegistrarAlumno);

            CargarGrupoCommand = new Command(CargarGrupo);
            VerAlumnoCommand = new Command<int>(GetAlumno);
            EliminarAlumnoCommand = new Command<int>(EliminarAlumno);

            CargarAvisosPersonalesCommand = new Command(CargarAvisosPersonales);
            VerAvisosPersonalesCommand = new Command<int>(VerDetalleAvisosPersonales);
            EliminarAvisosPersonalesCommand = new Command<int>(EliminarAvisosPersonales);

            CargarAvisosGeneralesCommand = new Command(CargarAvisosGenerales);
            VerAvisosGeneralesCommand = new Command<int>(VerDetalleAvisosGenerales);
            EliminarAvisosGeneralesCommand = new Command<int>(EliminarAvisosGenerales);

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
                if(response.Rol == "Maestro")
                    await Shell.Current.GoToAsync("//homemaestro");
                else if(response.Rol == "Alumno")
                    await Shell.Current.GoToAsync("//homealumno");

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
                await Shell.Current.GoToAsync("//login");
        }



        public GrupoDetalleDTO? Grupo { get; set; }
        public ObservableCollection<AlumnoResumenDTO> Alumnos { get; set; } = new();
        public AlumnoDetalleDTO? AlumnoSeleccionado { get; set; }

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

                await Shell.Current.GoToAsync("//detallealumno");
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
        public AvisoPersonalDetalleDTO? AvisoSeleccionado { get; set; }

        public ICommand CargarAvisosPersonalesCommand { get; set; }
        public ICommand VerAvisosPersonalesCommand { get; set; }
        public ICommand EliminarAvisosPersonalesCommand { get; set; }
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

        public AvisoGeneralDetalleMaestroDTO? SeleccionadoGeneral { get; set; }

        public ICommand CargarAvisosGeneralesCommand { get; set; }
        public ICommand VerAvisosGeneralesCommand { get; set; }
        public ICommand EliminarAvisosGeneralesCommand { get; set; }
        private async void CargarAvisosGenerales()
        {

            var avisosGenerales = await service.GetAvisos();
            listaGeneral = avisosGenerales;

            Avisos.Clear();
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
