using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Tienda.Data;
using Tienda.Models;
using Tienda.Models.Auth;

namespace Tienda.Services
{
    public class AuthService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;

        public AuthService(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        // 🔹 Registro de usuario con verificación de rol
        public async Task<Usuario> RegisterAsync(RegisterRequest request)
        {
            if (await _context.Usuarios.AnyAsync(u => u.CorreoElectronico == request.CorreoElectronico))
                throw new Exception("El correo ya está registrado.");

            var rol = await _context.Roles.FirstOrDefaultAsync(r => r.NombreRol == request.Rol);
            if (rol == null)
                throw new Exception("El rol especificado no existe.");

            var usuario = new Usuario
            {
                Nombre = request.Nombre,
                Apellido = request.Apellido,
                CorreoElectronico = request.CorreoElectronico,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                Estado = "Activo",
                IDRol = rol.IDRol
            };

            try
            {
                _context.Usuarios.Add(usuario);
                await _context.SaveChangesAsync();
                return usuario;
            }
            catch (Exception ex)
            {
                var inner = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                throw new Exception($"⚠️ Error al guardar el usuario: {inner}");
            }
        }

        // 🔹 Login de usuario + generación del token JWT
        public async Task<string?> LoginAsync(LoginRequest request)
        {
            var usuario = await _context.Usuarios
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(u => u.CorreoElectronico == request.CorreoElectronico);

            if (usuario == null || !BCrypt.Net.BCrypt.Verify(request.Password, usuario.PasswordHash))
                return null;

            // 🧩 Crear los claims (identidad del usuario dentro del token)
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, usuario.CorreoElectronico),
                new Claim(ClaimTypes.Email, usuario.CorreoElectronico),
                new Claim(ClaimTypes.Name, $"{usuario.Nombre} {usuario.Apellido}"),
                new Claim(ClaimTypes.Role, usuario.Rol.NombreRol) // 🔥 Aquí se agrega el rol
            };

            // 🧩 Generar la clave secreta desde appsettings.json
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // 🧩 Crear el token
            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(6),
                signingCredentials: creds
            );

            // 🔹 Devolver el token JWT listo para usar en Swagger
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
