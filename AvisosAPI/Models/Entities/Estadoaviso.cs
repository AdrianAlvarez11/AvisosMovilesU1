using System;
using System.Collections.Generic;

namespace AvisosAPI.Models.Entities;

public partial class Estadoaviso
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Alumnoavisogeneral> Alumnoavisogeneral { get; set; } = new List<Alumnoavisogeneral>();

    public virtual ICollection<Avisopersonal> Avisopersonal { get; set; } = new List<Avisopersonal>();
}
