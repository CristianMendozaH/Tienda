using System.ComponentModel.DataAnnotations;

namespace Tienda.Models
{
    public class Recibo
    {
        [Key]
        public int IDRecibo { get; set; }
        public string Fecha { get; set; }
    }
}
