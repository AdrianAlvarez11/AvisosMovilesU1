using AvisosApp.Views;
using Microsoft.Extensions.DependencyInjection;

namespace AvisosApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            Routing.RegisterRoute("registrarAlumno", typeof(RegisterAlumnoView));
            Routing.RegisterRoute("crearavisogeneral", typeof(ProfesorCrearAvisoGeneralView));
            Routing.RegisterRoute("crearavisopersonal", typeof(ProfesorCrearAvisoPersonalView));
            Routing.RegisterRoute("detallesalumnos", typeof(ProfesorDetallesAlumnosView));
            Routing.RegisterRoute("detalleavisopersonal", typeof(ProfesorDetalleAvisoPersonalView));
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
    }
}