namespace AvisosApp.Models.DTOs
{
    // la lista del alumno
    public class AvisoGeneralResumenDTO
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = null!;
        public string NombreMaestro { get; set; } = null!;
        public DateTime FechaEnviado { get; set; }
        public DateTime FechaExpira { get; set; }
        public int IdEstado { get; set; }
        public string NombreEstado { get; set; } = null!;

    }

    // Detalle para el alumno 
    public class AvisoGeneralDetalleAlumnoDTO
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = null!;
        public string Contenido { get; set; } = null!;
        public string NombreMaestro { get; set; } = null!;
        public DateTime FechaEnviado { get; set; }
        public DateTime FechaExpira { get; set; }
    }

    // Detalle para el maestro — incluye los datos del aviso
    // y dos listas separadas según el estado de lectura de cada alumno.
    public class AvisoGeneralDetalleMaestroDTO
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = null!;
        public string Contenido { get; set; } = null!;
        public string NombreMaestro { get; set; } = null!;
        public int IdMaestro { get; set; } //para que el maestro pueda eliminar
        public DateTime FechaEnviado { get; set; }
        public DateTime FechaExpira { get; set; }
        public bool EsProfesor { get; set; }


        // Alumnos con estado Nuevo o Recibido aún no lo leen
        public List<AlumnoLecturaDTO> PendientesLectura { get; set; } = new();

        // Alumnos con estado Leído
        public List<AlumnoLecturaDTO> Leidos { get; set; } = new();
    }

    //alumno dentro de las listas de lectura
    public class AlumnoLecturaDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string NumControl { get; set; } = null!;
        public DateTime? FechaLeido { get; set; }   // null si aún no leyó, no se si lo incluiremos en el diseño
    }

    public class AvisoGeneralCreateDTO
    {
        public string Titulo { get; set; } = null!;
        public string Contenido { get; set; } = null!;
        public DateTime FechaExpira { get; set; }
    }

}