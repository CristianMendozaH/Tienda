using System.ComponentModel.DataAnnotations;

namespace Tienda.Models
{
    public class Estado
    {
        [Key]
        public int IDEstado { get; set; }
        public string EstadoNombre { get; set; }
    }
}
