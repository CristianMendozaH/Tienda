using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tienda.Models
{
    [Table("Ventas")]
    public class Venta
    {
        [Key]
        public int IDVentas { get; set; }

        public DateTime Fecha { get; set; } = DateTime.Now;

        [Column(TypeName = "decimal(10,2)")]
        public decimal Total { get; set; }

        public bool Anulado { get; set; } = false;

        // Relaciones principales
        [ForeignKey("IDUsuario")]
        public int IDUsuario { get; set; }
        public Usuario? Usuario { get; set; }

        [ForeignKey("IDCliente")]
        public int IDCliente { get; set; }
        public Cliente? Cliente { get; set; }

        [ForeignKey("IDCaja")]
        public int? IDCaja { get; set; }
        public Caja? Caja { get; set; }

        // 🔹 Relación uno a muchos con DetalleVenta
        public ICollection<DetalleVenta>? DetallesVenta { get; set; }
    }
}
