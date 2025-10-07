using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tienda.Attributes;
using Tienda.Data;
using Tienda.Models;

namespace Tienda.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // 🔒 Requiere token JWT válido
    public class ProveedoresController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProveedoresController(AppDbContext context)
        {
            _context = context;
        }

        // ✅ GET: api/Proveedores
        [HttpGet]
        [AuthorizeRoles("Administrador", "Vendedora")]
        public async Task<IActionResult> GetProveedores()
        {
            var proveedores = await _context.Proveedores.ToListAsync();
            return Ok(proveedores);
        }

        // ✅ GET: api/Proveedores/{id}
        [HttpGet("{id}")]
        [AuthorizeRoles("Administrador", "Vendedora")]
        public async Task<IActionResult> GetProveedor(int id)
        {
            var proveedor = await _context.Proveedores.FindAsync(id);

            if (proveedor == null)
                return NotFound(new { mensaje = "Proveedor no encontrado." });

            return Ok(proveedor);
        }

        // ✅ POST: api/Proveedores
        [HttpPost]
        [AuthorizeRoles("Administrador")]
        public async Task<IActionResult> CrearProveedor([FromBody] Proveedor proveedor)
        {
            if (proveedor == null)
                return BadRequest(new { mensaje = "Datos inválidos." });

            try
            {
                _context.Proveedores.Add(proveedor);
                await _context.SaveChangesAsync();
                return Ok(new { mensaje = "Proveedor creado correctamente.", proveedor });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // ✅ PUT: api/Proveedores/{id}
        [HttpPut("{id}")]
        [AuthorizeRoles("Administrador")]
        public async Task<IActionResult> ActualizarProveedor(int id, [FromBody] Proveedor proveedor)
        {
            if (id != proveedor.IDProveedor)
                return BadRequest(new { mensaje = "El ID no coincide." });

            _context.Entry(proveedor).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return Ok(new { mensaje = "Proveedor actualizado correctamente." });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Proveedores.Any(p => p.IDProveedor == id))
                    return NotFound(new { mensaje = "Proveedor no encontrado." });
                throw;
            }
        }

        // ✅ DELETE: api/Proveedores/{id}
        [HttpDelete("{id}")]
        [AuthorizeRoles("Administrador")]
        public async Task<IActionResult> EliminarProveedor(int id)
        {
            var proveedor = await _context.Proveedores.FindAsync(id);

            if (proveedor == null)
                return NotFound(new { mensaje = "Proveedor no encontrado." });

            _context.Proveedores.Remove(proveedor);
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Proveedor eliminado correctamente." });
        }
    }
}
