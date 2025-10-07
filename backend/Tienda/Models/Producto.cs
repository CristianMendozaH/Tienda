using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tienda.Models
{
    [Table("Productos")]
    public class Producto
    {
        [Key]
        public int IDProducto { get; set; }

        [Required]
        [MaxLength(100)]
        public string NombreProducto { get; set; } = string.Empty;

        [Column(TypeName = "decimal(10,2)")]
        public decimal PrecioAdquisicion { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal PrecioVenta { get; set; }

        [MaxLength(255)]
        public string? Descripcion { get; set; }

        public string? ImagenProducto { get; set; }

        // Relaciones con otras tablas
        public int IDCategoria { get; set; }
        [ForeignKey("IDCategoria")]
        public Categoria? Categoria { get; set; }

        public int IDStock { get; set; }
        [ForeignKey("IDStock")]
        public Stock? Stock { get; set; }

        public int IDProveedor { get; set; }
        [ForeignKey("IDProveedor")]
        public Proveedor? Proveedor { get; set; }
    }
}
