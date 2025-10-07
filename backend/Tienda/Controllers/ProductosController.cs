using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tienda.Data;
using Tienda.Models;
using Tienda.Attributes; // 👈 Importante para [AuthorizeRoles]

namespace Tienda.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // 🔒 Requiere token JWT válido
    public class ProductosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductosController(AppDbContext context)
        {
            _context = context;
        }

        // ✅ GET: api/Productos
        [HttpGet]
        [AuthorizeRoles("Administrador", "Vendedora")]
        public async Task<IActionResult> GetProductos()
        {
            var productos = await _context.Productos
                .Include(p => p.Categoria)
                .Include(p => p.Proveedor)
                .Include(p => p.Stock)
                .ToListAsync();

            return Ok(productos);
        }

        // ✅ GET: api/Productos/{id}
        [HttpGet("{id}")]
        [AuthorizeRoles("Administrador", "Vendedora")]
        public async Task<IActionResult> GetProducto(int id)
        {
            var producto = await _context.Productos
                .Include(p => p.Categoria)
                .Include(p => p.Proveedor)
                .Include(p => p.Stock)
                .FirstOrDefaultAsync(p => p.IDProducto == id);

            if (producto == null)
                return NotFound(new { mensaje = "Producto no encontrado." });

            return Ok(producto);
        }

        // ✅ POST: api/Productos
        [HttpPost]
        [AuthorizeRoles("Administrador")]
        public async Task<IActionResult> CrearProducto([FromBody] Producto producto)
        {
            if (producto == null)
                return BadRequest(new { mensaje = "Datos inválidos." });

            try
            {
                _context.Productos.Add(producto);
                await _context.SaveChangesAsync();
                return Ok(new { mensaje = "Producto creado correctamente.", producto });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // ✅ PUT: api/Productos/{id}
        [HttpPut("{id}")]
        [AuthorizeRoles("Administrador")]
        public async Task<IActionResult> ActualizarProducto(int id, [FromBody] Producto producto)
        {
            if (id != producto.IDProducto)
                return BadRequest(new { mensaje = "El ID no coincide." });

            _context.Entry(producto).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return Ok(new { mensaje = "Producto actualizado correctamente." });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Productos.Any(p => p.IDProducto == id))
                    return NotFound(new { mensaje = "Producto no encontrado." });
                throw;
            }
        }

        // ✅ DELETE: api/Productos/{id}
        [HttpDelete("{id}")]
        [AuthorizeRoles("Administrador")]
        public async Task<IActionResult> EliminarProducto(int id)
        {
            var producto = await _context.Productos.FindAsync(id);

            if (producto == null)
                return NotFound(new { mensaje = "Producto no encontrado." });

            _context.Productos.Remove(producto);
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Producto eliminado correctamente." });
        }
    }
}
