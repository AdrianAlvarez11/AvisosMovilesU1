using System;
using System.Collections.Generic;

namespace AvisosAPI.Models.Entities;

public partial class Avisogeneral
{
    public int Id { get; set; }

    public string Titulo { get; set; } = null!;

    public string Contenido { get; set; } = null!;

    public int IdMaestro { get; set; }

    public DateTime FechaEnviado { get; set; }

    public DateTime FechaExpira { get; set; }

    public bool Eliminado { get; set; }

    public virtual ICollection<Alumnoavisogeneral> Alumnoavisogeneral { get; set; } = new List<Alumnoavisogeneral>();

    public virtual Maestro IdMaestroNavigation { get; set; } = null!;
}
