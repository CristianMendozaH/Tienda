using System.ComponentModel.DataAnnotations;

namespace Tienda.Models
{
    public class Permiso
    {
        [Key]
        public int IDPermiso { get; set; }
        public bool Caja { get; set; }
        public bool Dashboard { get; set; }
        public bool Catalogo { get; set; }
        public bool Inventario { get; set; }
        public bool Ventas { get; set; }
        public bool Administracion { get; set; }
        public bool Reportes { get; set; }

        public int IDUsuario { get; set; }
        public int IDRol { get; set; }
    }
}
