using System.ComponentModel.DataAnnotations;

namespace ApiStore.Models;

public class Localidad
{
    [Key]
    public int Id { get; set; }
    public string Nombre { get; set; }
    public string Provincia { get; set; }
}
