using System;
using System.Collections.Generic;

namespace AvisosAPI.Models.Entities;

public partial class Alumnoavisogeneral
{
    public int Id { get; set; }

    public int IdAlumno { get; set; }

    public int IdAvisoGeneral { get; set; }

    public DateTime? FechaLeido { get; set; }

    public int IdEstado { get; set; }

    public virtual Alumno IdAlumnoNavigation { get; set; } = null!;

    public virtual Avisogeneral IdAvisoGeneralNavigation { get; set; } = null!;

    public virtual Estadoaviso IdEstadoNavigation { get; set; } = null!;
}
