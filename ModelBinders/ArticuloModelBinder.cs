using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Proyecto_TiendaElectronica.Models;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Proyecto_TiendaElectronica.ModelBinders
{
    public class ArticuloModelBinder : IModelBinder
    {
        private readonly AppDBContext _context;

        public ArticuloModelBinder(AppDBContext context)
        {
            _context = context;
        }

        public async Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            var request = bindingContext.HttpContext.Request;

            var nombre = request.Form["Nombre"].ToString();
            var precio = decimal.Parse(request.Form["Precio"]);
            var descripcion = request.Form["Descripcion"].ToString();
            var marca = request.Form["Marca"].ToString();
            var cantidad = int.Parse(request.Form["Cantidad"].ToString());
            var idCategoria = int.Parse(request.Form["idCategoria"].ToString());

            var imagen1 = request.Form.Files["Imagen1"];
            var imagen2 = request.Form.Files["Imagen2"];
            var imagen3 = request.Form.Files["Imagen3"];

            Categoria categoria = await _context.Categoria.FindAsync(idCategoria);
            var articulosCategoria = await _context.Articulo
                .Where(a => a.idCategoria == idCategoria)
                .ToListAsync();

            categoria.Articulos = articulosCategoria;

            Imagen imagen = new Imagen
            {
                Imagen1 = await FormFileToByteArrayAsync(imagen1),
                Imagen2 = await FormFileToByteArrayAsync(imagen2),
                Imagen3 = await FormFileToByteArrayAsync(imagen3)
            };

            _context.Imagen.Add(imagen);
            await _context.SaveChangesAsync();

            var articulo = new Articulo
            {
                Nombre = nombre,
                Precio = precio,
                Descripcion = descripcion,
                Marca = marca,
                Cantidad = cantidad,
                codigoImagen = imagen.ImagenId,
                idCategoria = idCategoria,
                Imagen = imagen,
                Categoria = categoria,
                Imagen1 = imagen1,
                Imagen2 = imagen2,
                Imagen3 = imagen3
            };

            bindingContext.Result = ModelBindingResult.Success(articulo);
        }

        private async Task<byte[]> FormFileToByteArrayAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return null;

            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                return memoryStream.ToArray();
            }
        }
    }
}
