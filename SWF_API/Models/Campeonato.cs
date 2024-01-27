using System;
using System.Collections.Generic;

namespace SWF_API.Models;

public partial class Campeonato
{
    public int Id { get; set; }

    public string? Descripcion { get; set; }

    public virtual ICollection<Jugador> Jugadores { get; set; } = new List<Jugador>();
}
