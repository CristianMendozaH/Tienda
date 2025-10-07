using System.ComponentModel.DataAnnotations;

namespace Tienda.Models
{
    public class DetallePedido
    {
        [Key]
        public int IDDetallePedido { get; set; }
        public int Cantidad { get; set; }
    }
}
