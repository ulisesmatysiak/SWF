﻿using System;
using System.Collections.Generic;

namespace SWF_API.Models;

public partial class Tweet
{
    public int Id { get; set; }

    public int? IdJugador { get; set; }

    public int? IdCampeonato { get; set; }

    public int? IdFecha { get; set; }

    public virtual Campeonato? IdCampeonatoNavigation { get; set; }

    public virtual Fecha? IdFechaNavigation { get; set; }

    public virtual Jugadores? oJugadores { get; set; }
}