using System;
using System.Collections.Generic;

namespace SWF_API.Models;

public partial class Campeonato
{
    public int Id { get; set; }

    public string? Descripcion { get; set; }

    public virtual ICollection<Tweet> Tweets { get; set; } = new List<Tweet>();
}
