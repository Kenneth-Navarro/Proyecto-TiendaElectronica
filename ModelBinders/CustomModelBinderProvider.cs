using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Proyecto_TiendaElectronica.ModelBinders;
using Proyecto_TiendaElectronica.Models;
using Proyecto_TiendaElectronica.ViewModels;
using System;

namespace Proyecto_TiendaElectronica.ModelBinder
{
    public class CustomModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context) {
            if (context == null) throw new ArgumentNullException(nameof(context));

            if (context.Metadata.ModelType == typeof(Usuario)) return new BinderTypeModelBinder(typeof(UsuarioModelBinder));

            if (context.BindingInfo?.BinderModelName == "CustomBinderForCreate" &&
          context.Metadata.ModelType == typeof(Articulo))
            {
                return new BinderTypeModelBinder(typeof(ArticuloModelBinder));
            }

            return null;
        }
    }
}
