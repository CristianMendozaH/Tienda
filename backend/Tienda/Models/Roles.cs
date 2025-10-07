using System.ComponentModel.DataAnnotations;

namespace Tienda.Models
{
    public class Rol
    {
        [Key]
        public int IDRol { get; set; }
        [Required]
        public string NombreRol { get; set; }

        public ICollection<Usuario> Usuarios { get; set; }
    }
}
