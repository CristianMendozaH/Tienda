using Microsoft.AspNetCore.Mvc;
using Tienda.Services;
using Tienda.Models.Auth;

namespace Tienda.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        // 🔹 Registro de usuarios
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            try
            {
                var usuario = await _authService.RegisterAsync(request);
                return Ok(new
                {
                    message = "Usuario registrado con éxito",
                    correoElectronico = usuario.CorreoElectronico
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // 🔹 Login que devuelve el token JWT
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                var token = await _authService.LoginAsync(request);

                if (token == null)
                    return Unauthorized(new { message = "Credenciales incorrectas" });

                return Ok(new { token });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
