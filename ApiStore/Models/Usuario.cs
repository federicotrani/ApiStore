using System.ComponentModel.DataAnnotations.Schema;

namespace ApiStore.Models;

public class Usuario
{
    public int Id { get; set; }
    public string Nombre { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    public string Domicilio { get; set; }    
    public int IdRol { get; set; }
    public DateTime FechaAlta { get; set; }
    public bool Activo { get; set; }
}
