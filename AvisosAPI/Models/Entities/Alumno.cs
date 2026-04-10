using System;
using System.Collections.Generic;

namespace AvisosAPI.Models.Entities;

public partial class Alumno
{
    public int Id { get; set; }

    public int IdGrupo { get; set; }

    public string NumControl { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public string Contrasena { get; set; } = null!;

    public virtual ICollection<Avisopersonal> Avisopersonal { get; set; } = new List<Avisopersonal>();

    public virtual Grupo IdGrupoNavigation { get; set; } = null!;
}
