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
    [Authorize] // 🔒 Requiere autenticación JWT
    public class CategoriasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CategoriasController(AppDbContext context)
        {
            _context = context;
        }

        // ✅ GET: api/Categorias
        [HttpGet]
        [AuthorizeRoles("Administrador", "Vendedora")]
        public async Task<IActionResult> GetCategorias()
        {
            var categorias = await _context.Categorias
                .Include(c => c.Productos)
                .ToListAsync();

            return Ok(categorias);
        }

        // ✅ GET: api/Categorias/{id}
        [HttpGet("{id}")]
        [AuthorizeRoles("Administrador", "Vendedora")]
        public async Task<IActionResult> GetCategoria(int id)
        {
            var categoria = await _context.Categorias
                .Include(c => c.Productos)
                .FirstOrDefaultAsync(c => c.IDCategoria == id);

            if (categoria == null)
                return NotFound(new { mensaje = "Categoría no encontrada." });

            return Ok(categoria);
        }

        // ✅ POST: api/Categorias
        [HttpPost]
        [AuthorizeRoles("Administrador")]
        public async Task<IActionResult> CrearCategoria([FromBody] Categoria categoria)
        {
            if (categoria == null)
                return BadRequest(new { mensaje = "Datos inválidos." });

            try
            {
                _context.Categorias.Add(categoria);
                await _context.SaveChangesAsync();
                return Ok(new { mensaje = "Categoría creada correctamente.", categoria });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // ✅ PUT: api/Categorias/{id}
        [HttpPut("{id}")]
        [AuthorizeRoles("Administrador")]
        public async Task<IActionResult> ActualizarCategoria(int id, [FromBody] Categoria categoria)
        {
            if (id != categoria.IDCategoria)
                return BadRequest(new { mensaje = "El ID no coincide." });

            _context.Entry(categoria).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return Ok(new { mensaje = "Categoría actualizada correctamente." });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Categorias.Any(c => c.IDCategoria == id))
                    return NotFound(new { mensaje = "Categoría no encontrada." });
                throw;
            }
        }

        // ✅ DELETE: api/Categorias/{id}
        [HttpDelete("{id}")]
        [AuthorizeRoles("Administrador")]
        public async Task<IActionResult> EliminarCategoria(int id)
        {
            var categoria = await _context.Categorias.FindAsync(id);

            if (categoria == null)
                return NotFound(new { mensaje = "Categoría no encontrada." });

            _context.Categorias.Remove(categoria);
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Categoría eliminada correctamente." });
        }
    }
}
