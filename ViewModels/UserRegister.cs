using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Proyecto_TiendaElectronica.ViewModels
{
    public class UserRegister
    {
        [Required(ErrorMessage = "El número de cédula es obligatorio")]
        [StringLength(9, MinimumLength = 9, ErrorMessage = "La cédula debe de contener 9 dígitos")]
        public string UsuarioId { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio ")]
        [StringLength(10, MinimumLength = 2, ErrorMessage = "El nombre ingresado no es válido")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El correo electrónico es obligatorio")]
        [EmailAddress(ErrorMessage = "El correo electrónico no es válido")]
        public string Correo { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        //Cambiar por que la expresion esta mal
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_])[A-Za-z\d\W_]{9,255}$",
        ErrorMessage = "La contraseña debe ser mayor a 8 dígitos y contener al menos una letra mayúscula, una letra minúscula, un número y un carácter especial.")]
        public string Contrasena { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "Debe de confirmar la contraseña")]
        [Compare(nameof(Contrasena), ErrorMessage = "Las contraseñas no coinciden")]
        public string ConfirmarContrasena { get; set; }

        [Required(ErrorMessage = "El número telefónico es obligatorio")]
        [Phone(ErrorMessage = "El número telefónico no es válido")]
        [StringLength(8, MinimumLength = 8, ErrorMessage = "El número ingresado no es válido, debe contener al menos 8 dígitos")]
        public string Telefono { get; set; }
    }
}
