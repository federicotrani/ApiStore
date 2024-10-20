﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiStore.Models;

public class Producto
{
    [Key]
    public int Id { get; set; }
    public string Nombre { get; set; }
    public string? Descripcion { get; set; }
    public decimal? Precio { get; set; }
    public int? Stock { get; set; }
    public string? RutaImagen { get; set; }
    public string? RutaImagenLocal { get; set; }
    public int? IdCategoria { get; set; }
}
