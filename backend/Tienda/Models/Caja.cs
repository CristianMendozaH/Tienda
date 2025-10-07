using System.ComponentModel.DataAnnotations;

namespace Tienda.Models
{
    public class Caja
    {
        [Key]
        public int IDCaja { get; set; }
        public DateTime FechaApertura { get; set; }
        public decimal MontoApertura { get; set; }
        public string TipoEfectivo { get; set; }
    }
}
