using ApiStore.Data;
using ApiStore.Models;
using ApiStore.Models.Dto;
using ApiStore.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace ApiStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly StoreDbContext _context;

        public UsuariosController(StoreDbContext context)
        {
            _context = context;
        }

        [HttpPost("ValidarCredencial")]
        public async Task<IActionResult> ValidarCredencial([FromBody] UsuarioLoginDto usuario)
        {
            var existeLogin = await _context.Usuarios
                .AnyAsync(x => x.Email.Equals(usuario.Email) && x.Password.Equals(usuario.Password));

            Usuario usuarioLogin = await _context.Usuarios.FirstOrDefaultAsync(x => x.Email.Equals(usuario.Email) && x.Password.Equals(usuario.Password));

            
            if (!existeLogin)
            {
                return NotFound("Usuario No Existe");
            }

            LoginResponseDto loginReponse = new LoginResponseDto()
            {
                Autenticado = existeLogin,
                Email = existeLogin ? usuarioLogin.Email : "",
                Nombre = existeLogin ? usuarioLogin.Nombre : "",
                IdRol = existeLogin ? usuarioLogin.IdRol : 0,
                Id = existeLogin ? usuarioLogin.Id : 0
            };  

            return Ok(loginReponse);
        }
    }
}
