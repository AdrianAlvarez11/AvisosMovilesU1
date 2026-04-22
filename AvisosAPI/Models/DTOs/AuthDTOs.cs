namespace AvisosAPI.Models.DTOs
{
    public class AuthDTOs
    {
        public class LoginDTO
        {
            public string NumControl { get; set; } = null!;
            public string Contrasena { get; set; } = null!;
        }

        // Lo que regresa la API si el login fue exitoso
        // El Token es el JWT que la app guarda y manda en cada request
        public class LoginResponseDTO
        {
            public int Id { get; set; }
            public string NumControl { get; set; } = null!;
            public string Nombre { get; set; } = null!;
            public int IdGrupo { get; set; }
            public string NombreGrupo { get; set; } = null!;
            public string Token { get; set; } = null!;
            public string Rol { get; set; } = null!;
        }
    }
}
