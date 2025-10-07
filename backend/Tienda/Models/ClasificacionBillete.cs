using System.ComponentModel.DataAnnotations;

namespace Tienda.Models
{
    public class ClasificacionBillete
    {
        [Key]
        public int IDClasificacion { get; set; }
        public string Tipo { get; set; }
    }
}
