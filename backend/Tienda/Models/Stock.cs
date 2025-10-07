using System.ComponentModel.DataAnnotations;

namespace Tienda.Models
{
    public class Stock
    {
        [Key]
        public int IDStock { get; set; }
        public int Minimo { get; set; }
        public int Cantidad { get; set; }
        public DateTime FechaActualizacion { get; set; }
    }
}
