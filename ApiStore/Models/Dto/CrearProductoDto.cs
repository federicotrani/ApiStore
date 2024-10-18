﻿namespace ApiStore.Models.Dto;

public class CrearProductoDto
{   
    public string Nombre { get; set; }
    public string? Descripcion { get; set; }
    public decimal? Precio { get; set; }
    public int? Stock { get; set; }    
    public IFormFile Imagen{ get; set; }
    public int Categoria { get; set; }
}
