using System.ComponentModel.DataAnnotations;

namespace Tienda.Models
{
    public class Proveedor
    {
        [Key]
        public int IDProveedor { get; set; }
        public string NombreProveedor { get; set; }
        public string NombreContacto { get; set; }
        public string DireccionEmpresa { get; set; }
        public string EmailEmpresa { get; set; }
        public string TelefonoEmpresa { get; set; }
        public string Descripcion { get; set; }
    }
}
