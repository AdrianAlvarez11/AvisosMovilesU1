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
        }
    }
}
