using System.ComponentModel.DataAnnotations;

namespace Proyecto_TiendaElectronica.ViewModels
{
    public class UserLogin
    {
        [Required(ErrorMessage = "Ingrese el nombre de usuario")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Ingrese la contraseña")]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
