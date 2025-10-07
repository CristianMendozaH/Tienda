namespace Tienda.Models.Auth
{
    public class RegisterRequest
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string CorreoElectronico { get; set; }
        public string Password { get; set; }
        public string Rol { get; set; } // "Administrador" o "Vendedora"
    }
}
