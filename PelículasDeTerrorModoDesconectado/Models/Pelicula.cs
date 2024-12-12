using System;
using System.Collections.Generic;

namespace PelículasDeTerrorModoDesconectado.Models;

public partial class Pelicula
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public int Duracion { get; set; }

    public DateTime Estreno { get; set; }

    public int Rating { get; set; }

    public string Director { get; set; } = null!;

    public string Sinopsis { get; set; } = null!;

    public string Portada { get; set; } = null!;
}
