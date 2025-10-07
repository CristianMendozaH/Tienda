using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tienda.Models
{
    [Table("DetallesVenta")]
    public class DetalleVenta
    {
        [Key]
        public int IDDetalle { get; set; }

        public int IDVenta { get; set; }
        [ForeignKey("IDVenta")]
        public Venta? Venta { get; set; }

        public int IDProducto { get; set; }
        [ForeignKey("IDProducto")]
        public Producto? Producto { get; set; }

        public int Cantidad { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal PrecioUnitario { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal Subtotal { get; set; }
    }
}
