using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Proyecto_TiendaElectronica.ModelBinder
{
    public class UsuarioModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            var request = bindingContext.HttpContext.Request;
            
            var usuarioId = request.Form["UsuarioId"];
            var nombre = request.Form["Nombre"].ToString();
            var correo = request.Form["Correo"].ToString();
            var telefono = request.Form["Telefono"];
            var estadostr = request.Form["Estado"];
            bool.TryParse(estadostr, out bool estado);

            var rol = request.Form["Rol"].ToString();


            var contrasena = "Abcd1234*";
            var confirmarContrasena = "Abcd1234*";

            //usuario.Add(Contrasena);
            var result = new ViewModels.UserViewModel
            {
                UsuarioId = usuarioId,
                Nombre = nombre,
                Correo = correo,
                Telefono = telefono,
                Contrasena = contrasena,
                ConfirmarContrasena= confirmarContrasena,
                Rol = rol,
                Estado = estado

            };
            bindingContext.Result = ModelBindingResult.Success(result);
            return Task.CompletedTask;
        }

    }
}
