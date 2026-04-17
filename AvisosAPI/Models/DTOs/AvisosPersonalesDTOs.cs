namespace AvisosAPI.Models.DTOs
{
    // Para la lista — sin Contenido para que cargue rápido
    public class AvisoPersonalResumenDTO
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = null!;
        public string NombreMaestro { get; set; } = null!;
        public DateTime FechaEnviado { get; set; }
        public int IdEstado { get; set; }
        public string NombreEstado { get; set; } = null!;
    }

    // Para cuando el alumno abre un aviso específico
    public class AvisoPersonalDetalleDTO
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = null!;
        public string Contenido { get; set; } = null!;
        public string NombreMaestro { get; set; } = null!;
        public DateTime FechaEnviado { get; set; }
        public DateTime? FechaLeido { get; set; }   
        public int IdEstado { get; set; }
        public string NombreEstado { get; set; } = null!;
    }

    // Lo que manda el maestro al crear un aviso
    // IdMaestro NO va aquí — lo saca el servicio del token JWT
    public class AvisoPersonalCreateDTO
    {
        public int IdAlumno { get; set; }
        public string Titulo { get; set; } = null!;
        public string Contenido { get; set; } = null!;
    }
}
