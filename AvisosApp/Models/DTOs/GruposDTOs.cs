namespace AvisosApp.Models.DTOs
{
    public class AlumnoResumenDTO
    {
        public int Id { get; set; }
        public string NumControl { get; set; } = null!;
        public string Nombre { get; set; } = null!;
    }

    // Lo que ve el maestro al entrar: info del grupo + lista de alumnos
    public class GrupoDetalleDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public List<AlumnoResumenDTO> Alumnos { get; set; } = new();
    }

    // Lo que ve el maestro al seleccionar un alumno específico
    public class AlumnoDetalleDTO
    {
        public int Id { get; set; }
        public string NumControl { get; set; } = null!;
        public string Nombre { get; set; } = null!;
        public string NombreGrupo { get; set; } = null!;
        public List<AvisoPersonalResumenDTO> Avisos { get; set; } = new();
    }
}
