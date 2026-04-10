using System;
using System.Collections.Generic;

namespace AvisosAPI.Models.Entities;

public partial class Maestro
{
    public int Id { get; set; }

    public string NumControl { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public string Contrasena { get; set; } = null!;

    public virtual ICollection<Avisogeneral> Avisogeneral { get; set; } = new List<Avisogeneral>();

    public virtual ICollection<Avisopersonal> Avisopersonal { get; set; } = new List<Avisopersonal>();

    public virtual ICollection<Grupo> Grupo { get; set; } = new List<Grupo>();
}
