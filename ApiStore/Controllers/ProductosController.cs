using ApiStore.Data;
using ApiStore.Models;
using ApiStore.Models.Dto;
using ApiStore.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace ApiStore.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductosController : ControllerBase
{
    private readonly StoreDbContext _context;

    public ProductosController(StoreDbContext context)
    {
        _context = context;
    }

    [HttpGet(Name = "ObtenerTodos")]
    public async Task<IActionResult> ObtenerTodos()
    {
        try
        {
            var lista = await _context.Productos.ToListAsync();
            return Ok(lista);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("ObtenerPorId/{productoId:int}")]
    public async Task<IActionResult> ObtenerPorId([FromRoute(Name = "productoId")] int id)
    {
        try
        {
            var item = await _context.Productos.FirstOrDefaultAsync(x => x.Id == id);
            return Ok(item);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost()]
    public async Task<IActionResult> Crear([FromBody] Producto producto)
    {
        try
        {
            producto.RutaImagen = "Imagen/";
            await _context.Productos.AddAsync(producto);
            var result = await _context.SaveChangesAsync();

            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("CrearConImagen")]
    public async Task<IActionResult> CrearConImagen([FromForm] CrearProductoDto crearProducto)
    {
        try
        {
            
            var producto = new Producto()
            {
                Nombre = crearProducto.Nombre,
                Precio = crearProducto.Precio,
                Descripcion = crearProducto.Descripcion,
                Stock = crearProducto.Stock,
            };

            // Subida Archivo
            if (crearProducto.Imagen != null)
            {
                string nombreArchivo = producto.Id + Guid.NewGuid().ToString() + Path.GetExtension(crearProducto.Imagen.FileName);
                string rutaArchivo = @"wwwroot\ImagenesProductos\" + nombreArchivo;

                var ubicacionDirectorio = Path.Combine(Directory.GetCurrentDirectory(), rutaArchivo);
            
                FileInfo file = new FileInfo(ubicacionDirectorio);

                if (file.Exists) {
                    file.Delete();
                }

                using (var fileStream = new FileStream(ubicacionDirectorio, FileMode.Create))
                {
                    crearProducto.Imagen.CopyTo(fileStream);
                }

                var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Value}{HttpContext.Request.PathBase.Value}";
                // ubicacion acceso exterior desde navegador
                producto.RutaImagen = baseUrl + "/ImagenesProductos/" + nombreArchivo;
                // ubicacion local en servidor
                producto.RutaImagenLocal = rutaArchivo;
            }
            else
            {
                producto.RutaImagen = "https://placehold.com/600x400";
            }
            await _context.Productos.AddAsync(producto);
            await _context.SaveChangesAsync();

            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{productoId:int}")]
    public async Task<IActionResult> Borrar([FromRoute] int productoId)
    {
        try
        {
            var productoExistente = await _context.Productos.FindAsync(productoId);

            if(productoExistente != null)
            {
                _context.Productos.Remove(productoExistente);
                await _context.SaveChangesAsync();
            }
                

            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{productoId:int}")]
    public async Task<IActionResult> Modificar([FromBody] Producto producto, [FromRoute] int productoId)
    {
        try
        {
            var productoExistente = await _context.Productos.FindAsync(productoId);

            if (productoExistente != null)
            {
                if(!producto.Descripcion.IsNullOrEmpty()) productoExistente.Descripcion = producto.Descripcion;
                if(!producto.Nombre.IsNullOrEmpty()) productoExistente.Nombre = producto.Nombre;
                if(!producto.RutaImagen.IsNullOrEmpty()) productoExistente.RutaImagen = producto.RutaImagen;
                if(producto.Precio!=null) productoExistente.Precio = producto.Precio;
                if(producto.Stock != null) productoExistente.Stock = producto.Stock;

                _context.Productos.Update(productoExistente);
                await _context.SaveChangesAsync();
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("GuardarImagen")]
    public async Task<IActionResult> GuardarImagen([FromForm] UploadFileApi archivo)
    {
        var ruta = string.Empty;

        if (archivo.Archivo.Length>0)
        {
            var nombreArchivo = Guid.NewGuid().ToString() + Path.GetExtension(archivo.Archivo.FileName);
            ruta = $"Images/{nombreArchivo}";
            using (var stream = new FileStream(ruta, FileMode.Create))
            {
                try
                {
                    await archivo.Archivo.CopyToAsync(stream);
                    // TODO: grabar ruta archivo en BD                    
                }
                catch (Exception ex)
                {
                    return BadRequest("Error al grabar archivo: " + ex.Message);
                }
            }
        }        
        return Ok();
    }
}
