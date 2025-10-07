using System.ComponentModel.DataAnnotations;

namespace Tienda.Models
{
    public class InformacionEmpresa
    {
        [Key]
        public int IDEmpresa { get; set; }
        public string Nombre { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public string Direccion { get; set; }
    }
}
