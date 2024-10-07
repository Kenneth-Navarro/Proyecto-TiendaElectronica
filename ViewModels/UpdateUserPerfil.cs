using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Proyecto_TiendaElectronica.ViewModels
{
    public class UpdateUserPerfil
    {
        [Required(ErrorMessage = "El número de cédula es obligatorio")]
        [StringLength(9, MinimumLength = 9, ErrorMessage = "La cédula debe de contener 9 dígitos")]
        public string UsuarioId { get; set; }

        [Required(ErrorMessage = "El correo electrónico es obligatorio")]
        [EmailAddress(ErrorMessage = "El correo electrónico no es válido")]
        public string Correo { get; set; }

        [Required(ErrorMessage = "El número telefónico es obligatorio")]
        [Phone(ErrorMessage = "El número telefónico no es válido")]
        [StringLength(8, MinimumLength = 8, ErrorMessage = "El número ingresado no es válido, debe contener al menos 8 dígitos")]
        public string Telefono { get; set; }
    }
}
