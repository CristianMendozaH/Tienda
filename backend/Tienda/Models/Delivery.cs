using System.ComponentModel.DataAnnotations;

namespace Tienda.Models
{
    public class Delivery
    {
        [Key]
        public int IDDelivery { get; set; }
        public string NombreEmpresa { get; set; }
    }
}
