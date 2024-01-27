using System;
using System.Collections.Generic;

namespace SWF_API.Models;

public partial class Jugador
{
    public int Id { get; set; }

    public string? Descripcion { get; set; }

    public string? Nombre { get; set; }

    public int? Camiseta { get; set; }

    public string? ImagenUrl { get; set; }

    public int? IdCampeonato { get; set; }
    public virtual Campeonato? IdCampeonatoNavigation { get; set; }

    public virtual ICollection<Tweet> Tweets { get; set; } = new List<Tweet>();
}
