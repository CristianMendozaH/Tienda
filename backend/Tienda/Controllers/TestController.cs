using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Tienda.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        // 🔓 Ruta pública: no requiere autenticación
        [HttpGet("publico")]
        [AllowAnonymous]
        public IActionResult Publico()
        {
            return Ok(new
            {
                mensaje = "🌍 Acceso público: no requiere autenticación."
            });
        }

        // 🔒 Solo para usuarios con rol Vendedora o Administrador
        [HttpGet("vendedora")]
        [Authorize(Roles = "Vendedora,Administrador")]
        public IActionResult Vendedora()
        {
            var usuario = User.FindFirstValue(ClaimTypes.Email);
            var rol = User.FindFirstValue(ClaimTypes.Role);

            return Ok(new
            {
                mensaje = "✅ Acceso permitido a Administrador o Vendedora.",
                usuario,
                rol
            });
        }

        // 🔒 Solo para Administradores
        [HttpGet("admin")]
        [Authorize(Roles = "Administrador")]
        public IActionResult Admin()
        {
            var usuario = User.FindFirstValue(ClaimTypes.Email);
            var rol = User.FindFirstValue(ClaimTypes.Role);

            return Ok(new
            {
                mensaje = "✅ Acceso permitido solo al Administrador.",
                usuario,
                rol
            });
        }
    }
}
