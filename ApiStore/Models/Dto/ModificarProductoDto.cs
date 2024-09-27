namespace ApiStore.Models.Dto;

public class ModificarProductoDto
{
    public int Id { get; set; }
    public string Nombre { get; set; }
    public string? Descripcion { get; set; }
    public decimal? Precio { get; set; }
    public int? Stock { get; set; }
    public string? RutaImagen { get; set; }
    public string? RutaImagenLocal { get; set; }
    public IFormFile Imagen { get; set; }
}
