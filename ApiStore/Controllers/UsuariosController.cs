using System.IdentityModel.Tokens.Jwt;
using ApiStore.Data;
using ApiStore.Models;
using ApiStore.Models.Dto;
using ApiStore.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;

namespace ApiStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly StoreDbContext _context;
        private readonly IConfiguration _configuration;

        public UsuariosController(StoreDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [AllowAnonymous]
        [HttpPost("ValidarCredencial")]
        public async Task<IActionResult> ValidarCredencial([FromBody] UsuarioLoginDto usuario)
        {
            try
            {
                var existeLogin = await _context.Usuarios
                .AnyAsync(x => x.Email.Equals(usuario.Email) && x.Password.Equals(usuario.Password));

                Usuario usuarioLogin = await _context.Usuarios.FirstOrDefaultAsync(x => x.Email.Equals(usuario.Email) && x.Password.Equals(usuario.Password));


                if (!existeLogin)
                {
                    return NotFound("Usuario No Existe");
                }
                
                var token = GenerateJwtToken(usuario.Email);

                LoginResponseDto loginReponse = new LoginResponseDto()
                {
                    Autenticado = existeLogin,
                    Email = existeLogin ? usuarioLogin.Email : "",
                    Nombre = existeLogin ? usuarioLogin.Nombre : "",
                    IdRol = existeLogin ? usuarioLogin.IdRol : 0,
                    Id = existeLogin ? usuarioLogin.Id : 0,
                    Token = token
                };

                return Ok(loginReponse);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        private string GenerateJwtToken(string email)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings").GetSection("Secret").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "localhost",
                audience: "localhost",
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
