using AvisosApp.Views;

namespace AvisosApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute("crearavisogeneral", typeof(ProfesorCrearAvisoGeneralView));
            Routing.RegisterRoute("crearavisopersonal", typeof(ProfesorCrearAvisoPersonalView));
            Routing.RegisterRoute("detallesalumnos", typeof(ProfesorDetallesAlumnosView));
            Routing.RegisterRoute("detalleavisopersonalmaestro", typeof(ProfesorDetalleAvisoPersonalView));

            // Rutas para detalles (navegación relativa)
            Routing.RegisterRoute("detalleavisoalumno", typeof(AlumnoAvisoDetallesView));
            Routing.RegisterRoute("detalleavisopersonal", typeof(AlumnoAvisoPersonalDetallesView));
            Routing.RegisterRoute("detalleavisogeneral", typeof(ProfesorDetalleAvisoGeneralView));
        }
    }
}
