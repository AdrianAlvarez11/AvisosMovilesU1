namespace AvisosAPI.Models.DTOs
{
    public class AvisoGeneralResumenDTO
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = null!;
        public string NombreMaestro { get; set; } = null!;
        public DateTime FechaEnviado { get; set; }
        public DateTime FechaExpira { get; set; }
    }

    public class AvisoGeneralDetalleDTO
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = null!;
        public string Contenido { get; set; } = null!;
        public string NombreMaestro { get; set; } = null!;
        public DateTime FechaEnviado { get; set; }
        public DateTime FechaExpira { get; set; }
    }

    public class AvisoGeneralCreateDTO
    {
        public string Titulo { get; set; } = null!;
        public string Contenido { get; set; } = null!;
        public DateTime FechaExpira { get; set; }
    }
}
