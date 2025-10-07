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
    public class StockController : ControllerBase
    {
        private readonly AppDbContext _context;

        public StockController(AppDbContext context)
        {
            _context = context;
        }

        // ✅ GET: api/Stock
        [HttpGet]
        [AuthorizeRoles("Administrador", "Vendedora")]
        public async Task<IActionResult> GetStock()
        {
            var stock = await _context.Stock.ToListAsync();
            return Ok(stock);
        }

        // ✅ GET: api/Stock/{id}
        [HttpGet("{id}")]
        [AuthorizeRoles("Administrador", "Vendedora")]
        public async Task<IActionResult> GetStockItem(int id)
        {
            var stock = await _context.Stock.FindAsync(id);

            if (stock == null)
                return NotFound(new { mensaje = "Registro de stock no encontrado." });

            return Ok(stock);
        }

        // ✅ POST: api/Stock
        [HttpPost]
        [AuthorizeRoles("Administrador")]
        public async Task<IActionResult> CrearStock([FromBody] Stock stock)
        {
            if (stock == null)
                return BadRequest(new { mensaje = "Datos inválidos." });

            stock.FechaActualizacion = DateTime.Now;

            try
            {
                _context.Stock.Add(stock);
                await _context.SaveChangesAsync();
                return Ok(new { mensaje = "Registro de stock creado correctamente.", stock });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // ✅ PUT: api/Stock/{id}
        [HttpPut("{id}")]
        [AuthorizeRoles("Administrador", "Vendedora")]
        public async Task<IActionResult> ActualizarStock(int id, [FromBody] Stock stock)
        {
            if (id != stock.IDStock)
                return BadRequest(new { mensaje = "El ID no coincide." });

            stock.FechaActualizacion = DateTime.Now;
            _context.Entry(stock).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return Ok(new { mensaje = "Stock actualizado correctamente." });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Stock.Any(s => s.IDStock == id))
                    return NotFound(new { mensaje = "Registro de stock no encontrado." });
                throw;
            }
        }

        // ✅ DELETE: api/Stock/{id}
        [HttpDelete("{id}")]
        [AuthorizeRoles("Administrador")]
        public async Task<IActionResult> EliminarStock(int id)
        {
            var stock = await _context.Stock.FindAsync(id);

            if (stock == null)
                return NotFound(new { mensaje = "Registro de stock no encontrado." });

            _context.Stock.Remove(stock);
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Registro de stock eliminado correctamente." });
        }
    }
}
