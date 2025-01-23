using Microsoft.AspNetCore.Identity;

namespace RegistroPersonas.Modelos
{
    public class AppUsuario : IdentityUser
    {
        public string Nombre { get; set; }
    }
}
