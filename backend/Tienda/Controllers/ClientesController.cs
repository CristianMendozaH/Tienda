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
    [Authorize] // 🔒 Requiere JWT
    public class ClientesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ClientesController(AppDbContext context)
        {
            _context = context;
        }

        // ✅ GET: api/Clientes
        [HttpGet]
        [AuthorizeRoles("Administrador", "Vendedora")]
        public async Task<IActionResult> GetClientes()
        {
            var clientes = await _context.Clientes
                .Include(c => c.Ventas)
                .ToListAsync();

            return Ok(clientes);
        }

        // ✅ GET: api/Clientes/{id}
        [HttpGet("{id}")]
        [AuthorizeRoles("Administrador", "Vendedora")]
        public async Task<IActionResult> GetCliente(int id)
        {
            var cliente = await _context.Clientes
                .Include(c => c.Ventas)
                .FirstOrDefaultAsync(c => c.IDCliente == id);

            if (cliente == null)
                return NotFound(new { mensaje = "Cliente no encontrado." });

            return Ok(cliente);
        }

        // ✅ POST: api/Clientes
        [HttpPost]
        [AuthorizeRoles("Administrador", "Vendedora")]
        public async Task<IActionResult> CrearCliente([FromBody] Cliente cliente)
        {
            if (cliente == null)
                return BadRequest(new { mensaje = "Datos inválidos." });

            try
            {
                _context.Clientes.Add(cliente);
                await _context.SaveChangesAsync();
                return Ok(new { mensaje = "Cliente registrado correctamente.", cliente });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // ✅ PUT: api/Clientes/{id}
        [HttpPut("{id}")]
        [AuthorizeRoles("Administrador", "Vendedora")]
        public async Task<IActionResult> ActualizarCliente(int id, [FromBody] Cliente cliente)
        {
            if (id != cliente.IDCliente)
                return BadRequest(new { mensaje = "El ID no coincide." });

            _context.Entry(cliente).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return Ok(new { mensaje = "Cliente actualizado correctamente." });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Clientes.Any(c => c.IDCliente == id))
                    return NotFound(new { mensaje = "Cliente no encontrado." });
                throw;
            }
        }

        // ✅ DELETE: api/Clientes/{id}
        [HttpDelete("{id}")]
        [AuthorizeRoles("Administrador")]
        public async Task<IActionResult> EliminarCliente(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);

            if (cliente == null)
                return NotFound(new { mensaje = "Cliente no encontrado." });

            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Cliente eliminado correctamente." });
        }
    }
}
