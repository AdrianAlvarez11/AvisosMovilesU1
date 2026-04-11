namespace AvisosAPI.Models.DTOs
{
    public class RegistroDTOs
    {
        public class MaestroRegistroDTO
        {
            public string NumControl { get; set; } = null!;
            public string Nombre { get; set; } = null!;
            public string Contrasena { get; set; } = null!;
            public string NombreGrupo { get; set; } = null!;
        }

        public class AlumnoRegistroDTO
        {
            public string NumControl { get; set; } = null!;
            public string Nombre { get; set; } = null!;
            public string Contrasena { get; set; } = null!;
        }
    }
}
