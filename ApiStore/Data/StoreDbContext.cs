using ApiStore.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiStore.Data;

public class StoreDbContext : DbContext
{
    public StoreDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Producto> Productos { get; set; }
    public DbSet<Usuario> Usuarios { get; set; }    


}
