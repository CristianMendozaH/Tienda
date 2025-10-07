using System.ComponentModel.DataAnnotations;

namespace Tienda.Models
{
    public class Pedido
    {
        [Key]
        public int IDPedido { get; set; }
        public string Fecha { get; set; }
    }
}
