using Microsoft.AspNetCore.Identity;

namespace Proyecto_TiendaElectronica.ViewModels
{
    public class Usuario : IdentityUser
    {
        public bool State { get; set; }
    }
}
