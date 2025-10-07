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
    [Authorize]
    public class VentasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public VentasController(AppDbContext context)
        {
            _context = context;
        }

        // ✅ GET: api/Ventas
        [HttpGet]
        [AuthorizeRoles("Administrador", "Vendedora")]
        public async Task<IActionResult> GetVentas()
        {
            var ventas = await _context.Ventas
                .Include(v => v.Cliente)
                .Include(v => v.Usuario)
                .Include(v => v.Caja)
                .Include(v => v.DetallesVenta)
                    .ThenInclude(dv => dv.Producto)
                .ToListAsync();

            return Ok(ventas);
        }

        // ✅ POST: api/Ventas
        [HttpPost]
        [AuthorizeRoles("Administrador", "Vendedora")]
        public async Task<IActionResult> CrearVenta([FromBody] Venta venta)
        {
            if (venta == null || venta.DetallesVenta == null || !venta.DetallesVenta.Any())
                return BadRequest(new { mensaje = "Debe incluir al menos un producto en la venta." });

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                venta.Fecha = DateTime.Now;
                venta.Anulado = false;

                // Guardar la venta
                _context.Ventas.Add(venta);
                await _context.SaveChangesAsync();

                decimal totalVenta = 0;

                // Procesar cada detalle
                foreach (var detalle in venta.DetallesVenta)
                {
                    var producto = await _context.Productos
                        .Include(p => p.Stock)
                        .FirstOrDefaultAsync(p => p.IDProducto == detalle.IDProducto);

                    if (producto == null)
                        throw new Exception($"El producto con ID {detalle.IDProducto} no existe.");

                    if (producto.Stock == null || producto.Stock.Cantidad < detalle.Cantidad)
                        throw new Exception($"Stock insuficiente para el producto '{producto.NombreProducto}'.");

                    // Restar del stock
                    producto.Stock.Cantidad -= detalle.Cantidad;
                    producto.Stock.FechaActualizacion = DateTime.Now;

                    // Calcular subtotal
                    detalle.IDVenta = venta.IDVentas;
                    detalle.PrecioUnitario = producto.PrecioVenta;
                    detalle.Subtotal = detalle.Cantidad * detalle.PrecioUnitario;

                    _context.DetallesVenta.Add(detalle);
                    totalVenta += detalle.Subtotal;
                }

                // Guardar totales
                venta.Total = totalVenta;
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return Ok(new
                {
                    mensaje = "✅ Venta registrada correctamente.",
                    venta.IDVentas,
                    Total = venta.Total
                });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
