using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tienda.Models
{
    public class Usuario
    {
        [Key]
        public int IDUsuario { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Required]
        public string Apellido { get; set; }
        public string? Telefono { get; set; }
        [Required]
        public string CorreoElectronico { get; set; }
        [Required]
        public string PasswordHash { get; set; }
        public string Estado { get; set; }

        // Relaciones
        public int IDRol { get; set; }
        [ForeignKey("IDRol")]
        public Rol Rol { get; set; }

        public int? IDCaja { get; set; }
        [ForeignKey("IDCaja")]
        public Caja Caja { get; set; }
    }
}
