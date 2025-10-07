using System.ComponentModel.DataAnnotations;

namespace Tienda.Models
{
    public class Cliente
    {
        [Key]
        public int IDCliente { get; set; }
        public string Nombre { get; set; }
        public string Telefono { get; set; }

        public ICollection<Venta> Ventas { get; set; }
    }
}
