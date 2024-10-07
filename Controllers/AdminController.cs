using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Proyecto_TiendaElectronica.Models;
using Proyecto_TiendaElectronica.ModelBinder;
using Microsoft.AspNetCore.Identity;
using Proyecto_TiendaElectronica.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Proyecto_TiendaElectronica.ModelBinders;
using QuestPDF.Infrastructure;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace Proyecto_TiendaElectronica.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDBContext _context;
        private readonly UserManager<Usuario> _userManager;
        private readonly SignInManager<Usuario> _signInManager;

        public AdminController(AppDBContext context, UserManager<Usuario> userManager, SignInManager<Usuario> signInManager) { 
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            QuestPDF.Settings.License = LicenseType.Community;
        }

		[Authorize(Roles = "Administrador")]
		public ActionResult Index()
		{
			ViewBag.Pagina = "Index";
			return View();
		}

        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Articulos()
		{

			var articulos = await _context.Articulo.Include("Categoria").Include("Imagen").Take(10).ToListAsync();
			ViewBag.Pagina = "Articulos";

			return View(articulos);

		}

        [Authorize(Roles = "Administrador")]


        public ActionResult CrearArticulo()
		{

			var categorias = _context.Categoria.ToList();

			ViewBag.Categorias = new SelectList(categorias, "CategoriaId", "Nombre");

			ViewBag.Pagina = "CrearArticulo";

			return View();
		}


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CrearArticulo([ModelBinder(BinderType = typeof(ArticuloModelBinder), Name = "CustomBinderForCreate")] Articulo articulo)
        {

          
            try
            {
                _context.Articulo.Add(articulo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Articulos));
            }
            catch (DbUpdateException)
            {
                TempData["SweetAlertScript"] = "<script>Swal.fire({ title: \"Error\", text: \"Error al guardar el artículo. Por favor, inténtelo más tarde.\", icon: \"error\", confirmButtonColor: \"#E14848\" });</script>";
            }
            catch (Exception)
            {
                TempData["SweetAlertScript"] = "<script>Swal.fire({ title: \"Error\", text: \"Ha ocurrido un error. Por favor, inténtelo más tarde.\", icon: \"error\", confirmButtonColor: \"#E14848\" });</script>";
            }

            return View(articulo);
        }



        [HttpGet]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> VerArticulo(int id)
		{
			if (id == null)
			{
				return NotFound();
			}
			try
			{
				var articulo = await _context.Articulo.FindAsync(id);
				if (articulo == null)
				{
					return NotFound();
				}

				var imagen = await _context.Imagen.FindAsync(articulo.codigoImagen);
				var categoria = await _context.Categoria.FindAsync(articulo.idCategoria);

				ViewBag.Imagen1 = imagen.Imagen1;
				ViewBag.Imagen2 = imagen.Imagen2;
				ViewBag.Imagen3 = imagen.Imagen3;

				ViewBag.Categoria = categoria.Nombre.ToString();


				return View(articulo);
			}
			catch (Exception)
			{
				return RedirectToAction(nameof(Articulos));
			}
		}

		[HttpGet]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> EditarArticulo(int id)
		{
			var articulo = await _context.Articulo.FindAsync(id);
			if (articulo == null)
			{
				return NotFound();
			}



			var categorias = _context.Categoria.ToList();
			ViewBag.Categorias = new SelectList(categorias, "CategoriaId", "Nombre");

            return View(articulo);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> EditarArticulo(Articulo articulo)
		{
			Articulo articuloDB = new Articulo
			{
				ArticuloId = articulo.ArticuloId,
				Nombre = articulo.Nombre,
				Descripcion = articulo.Descripcion,
				Marca = articulo.Marca,
				Precio = articulo.Precio,
				Cantidad = articulo.Cantidad,
				idCategoria = articulo.idCategoria,
				codigoImagen = articulo.codigoImagen,
			};



			try
			{
				_context.Articulo.Update(articuloDB);
				await _context.SaveChangesAsync();


				return RedirectToAction(nameof(Articulos));
			}

			catch (Exception)
			{
				return View(articuloDB);
			}

		}
		[HttpGet]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> ModificaImagen(int codigoImagen, int ArticuloID)
		{
			var Imagen = await _context.Imagen.FindAsync(codigoImagen);

			if (Imagen == null)
			{
				return NotFound();
			}
			ViewBag.ArticuloId = ArticuloID;
            ViewBag.Pagina = "ModificaImagen";
            return View(Imagen);
		}

		[HttpPost]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> ModificaImagen(Imagen imagen, IFormFile Imagen1, IFormFile Imagen2, IFormFile Imagen3)
		{

			var Imagen = await _context.Imagen.FindAsync(imagen.ImagenId);
			if (Imagen1 != null)
			{
				using (var memoryStream = new MemoryStream())
				{
					await Imagen1.CopyToAsync(memoryStream);
					Imagen.Imagen1 = memoryStream.ToArray();
				}
			}

			if (Imagen2 != null)
			{
				using (var memoryStream = new MemoryStream())
				{
					await Imagen2.CopyToAsync(memoryStream);
					Imagen.Imagen2 = memoryStream.ToArray();
				}
			}

			if (Imagen3 != null)
			{
				using (var memoryStream = new MemoryStream())
				{
					await Imagen3.CopyToAsync(memoryStream);
					Imagen.Imagen3 = memoryStream.ToArray();
				}
			}

			_context.Update(Imagen);
			await _context.SaveChangesAsync();
			return RedirectToAction("Articulos");

		}

		[HttpGet]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> EliminarArticulo(int id)
		{
			if (id == null)
			{
				return Json(new { success = false, message = "ID de Articulo no proporcionado." });
			}

			try
			{
				var articulo = await _context.Articulo.FindAsync(id);

				if (articulo == null)
				{
					return Json(new { success = false, message = "articulo no encontrado." });
				}
				_context.Articulo.Remove(articulo);

				_context.SaveChanges();

				return Json(new { success = true, message = "Articulo eliminado con éxito." });
			}
			catch (Exception)
			{
				return Json(new { success = false, message = "Ocurrió un error al eliminar el articulo." });
			}



		}


		[HttpGet]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Usuarios()
		{

			var usuarios = await _context.Usuario.ToListAsync();
			var datos = new List<UserViewModel>();

			foreach (var usuario in usuarios) {
				var roles = await _userManager.GetRolesAsync(usuario);

				var rol = roles.FirstOrDefault();

				var user = Conversion(usuario, rol);

				datos.Add(user);
			}

			ViewBag.Pagina = "Usuarios";

			return View(datos);

		}


		[HttpGet]
        [Authorize(Roles = "Administrador")]
        public IActionResult CrearUsuario()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> CrearUsuario(UserViewModel model)
		{

			if (ModelState.IsValid)
			{
				var usuario = new Usuario { Id = model.UsuarioId, UserName = model.Nombre, Email = model.Correo, PhoneNumber = model.Telefono, State = model.Estado };

				var result = await _userManager.CreateAsync(usuario, model.Contrasena);

				if (result.Succeeded)
				{

					await _userManager.AddToRoleAsync(usuario, model.Rol);

					return RedirectToAction("Usuarios");
				}

				foreach (var error in result.Errors)
				{
					ModelState.AddModelError("", error.Description);
				}

			}

			return View(model);


		}


		[HttpGet]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> VerUsuario(string id)
		{
			if (id == null)
			{
				return NotFound();
			}

			try
			{
				var usuario = await _context.Usuario.FindAsync(id);

				if (usuario == null)
				{
					return NotFound();
				}

				var roles = await _userManager.GetRolesAsync(usuario);

				var rol = roles.FirstOrDefault();

				var user = Conversion(usuario, rol);

				return View(user);
			}
			catch (Exception)
			{
				return RedirectToAction(nameof(Usuarios));
			}


		}

		[HttpGet]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> EditarUsuario(string id)
		{
			if (id == null)
			{
				return NotFound();
			}


			try
			{
				var usuario = await _context.Usuario.FindAsync(id);

				if (usuario == null)
				{
					return NotFound();
				}

				var roles = await _userManager.GetRolesAsync(usuario);

				var rol = roles.FirstOrDefault();

				var user = Conversion(usuario, rol);

				return View(user);
			}
			catch (Exception)
			{
				return RedirectToAction(nameof(Usuarios));
			}


		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> EditarUsuario([ModelBinder(BinderType = typeof(ModelBinder.UsuarioModelBinder))] UserViewModel model)
		{

			if (ModelState.IsValid)
			{
				var usuario = await _userManager.FindByIdAsync(model.UsuarioId);

				if (usuario == null) {
					return NotFound();
				}

				usuario.State = model.Estado;
				usuario.UserName = model.Nombre;
				usuario.Email = model.Correo;
				usuario.PhoneNumber = model.Telefono;

				var result = await _userManager.UpdateAsync(usuario);

				if (result.Succeeded)
				{
					var rolActual = await _userManager.GetRolesAsync(usuario);

					await _userManager.RemoveFromRolesAsync(usuario, rolActual);

					await _userManager.AddToRoleAsync(usuario, model.Rol);

					return RedirectToAction("Usuarios");
				}

				foreach (var error in result.Errors)
				{
					ModelState.AddModelError("", error.Description);
				}

			}

			return View(model);

		}

		[HttpGet]
        [Authorize(Roles = "Administrador")]
        public async  Task<IActionResult> EditarContrasena(string id) {
			if (id == null) {
				return NotFound();
			}

			var usuario = await _userManager.FindByIdAsync(id);

			if (usuario == null) {
				return NotFound();
			}

			var user = Conversion(usuario);

			return View(user);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async  Task<IActionResult> EditarContrasena(UserViewModel model)
		{
			if (model.UsuarioId != null && model.Contrasena != null && model.ConfirmarContrasena != null) {
				var usuario = await _userManager.FindByIdAsync(model.UsuarioId);

				if (usuario == null) {
					return NotFound();
				}

                var token = await _userManager.GeneratePasswordResetTokenAsync(usuario);
                var result = await _userManager.ResetPasswordAsync(usuario, token, model.Contrasena);

                if (result.Succeeded)
                {
					return RedirectToAction("EditarUsuario", new { id = model.UsuarioId });
					
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

			return View(model);


			
		}


		public async Task<IActionResult> EliminarUsuario(string id)
		{
			if (id == null)
			{
				return Json(new { success = false, message = "ID de usuario no proporcionado." });
			}

			var usuario = await _userManager.FindByIdAsync(id);

			if (usuario == null)
			{
				return Json(new { success = false, message = "Usuario no encontrado." });
			}
			var result = await _userManager.DeleteAsync(usuario);

			if (result.Succeeded)
			{
				return Json(new { success = true, message = "Usuario eliminado con éxito." });
			}

			foreach (var error in result.Errors)
			{
				ModelState.AddModelError("", error.Description);
			}
			
			return Json(new { success = false, message = "Ocurrió un error al eliminar el usuario." });
			

		}

		[HttpGet]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Factura()
        {
            var facturas = await _context.Factura.ToListAsync();
            ViewBag.Pagina = "Facturas";

            return View(facturas);
        }

		[HttpGet]
		[Authorize(Roles = "Administrador")]
		public async Task<IActionResult> VerFactura(int facturaId) {
			if (facturaId == null) {
				return NotFound();
			}

            var factura = await _context.Factura
			.Include(f => f.articulosFactura)  
				.ThenInclude(af => af.Articulo) 
			.Where(f => f.FacturaId == facturaId)
			.FirstOrDefaultAsync();

            if (factura == null) {
				return NotFound();
			}

			var usuario = await _userManager.FindByIdAsync(factura.UsuarioId);

			ViewBag.Usuario = usuario;

			return View(factura);
		}


        [HttpGet]
        public async Task<IActionResult> DescargarPDF(int facturaId)
        {
            // Recupera la factura incluyendo los detalles de los artículos comprados
            var factura = await _context.Factura
                .Include(f => f.articulosFactura)
                .ThenInclude(af => af.Articulo)
                .FirstOrDefaultAsync(f => f.FacturaId == facturaId);

            if (factura == null)
            {
                return NotFound("Factura no encontrada.");
            }

            var usuario = await _context.Usuario.FirstOrDefaultAsync(u => u.Id == factura.UsuarioId);

            // Verifica si la factura fue encontrada


            // Crea el documento PDF usando QuestPDF
            var pdf = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(30);

                    // Cabecera de la página
                    page.Header().Row(row =>
                    {

                        row.RelativeItem().Column(col =>
                        {
                            col.Item().Text("Factura").Bold().FontSize(22).FontColor("#333333");
                            col.Item().Text($"Número de Factura: {factura.FacturaId}").FontSize(14).FontColor("#555555");
                            col.Item().Text($"Fecha: {factura.FechaCreacion.ToShortDateString()}").FontSize(12).FontColor("#777777");
                        });

                        row.RelativeItem().AlignRight().Column(col =>
                        {
                            col.Item().Text($"Cliente: {usuario.Id}").FontSize(14).FontColor("#000000");
                            col.Item().Text($"Nombre: {usuario.UserName}").FontSize(14).FontColor("#000000");
                            col.Item().Text($"Número: {usuario.PhoneNumber}").FontSize(14).FontColor("#000000");
                            col.Item().Text($"Email: {usuario.Email}").FontSize(14).FontColor("#000000");

                        });
                    });

                    // Contenido de la factura
                    page.Content().PaddingVertical(10).Column(col =>
                    {
                        col.Item().Text("Detalles de la Factura").Bold().FontSize(16);

                        col.Item().Table(table =>
                        {
                            // Definición de columnas
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(3);  // Nombre del producto
                                columns.RelativeColumn();   // Precio unitario
                                columns.RelativeColumn();   // Cantidad
                                columns.RelativeColumn();   // Total por articulo
                            });

                            // Encabezado de la tabla
                            table.Header(header =>
                            {
                                header.Cell().Background("#E14848").Padding(5).Text("Producto").Bold().FontColor("#ffffff");
                                header.Cell().Background("#E14848").Padding(5).Text("Precio Unitario").Bold().FontColor("#ffffff");
                                header.Cell().Background("#E14848").Padding(5).Text("Cantidad").Bold().FontColor("#ffffff");
                                header.Cell().Background("#E14848").Padding(5).Text("Precio").Bold().FontColor("#ffffff");
                            });

                            // Agrega las filas de productos
                            foreach (var articuloFactura in factura.articulosFactura)
                            {
                                var articulo = articuloFactura.Articulo;
                                var cantidad = articuloFactura.CantidadArticulo;
                                var precio = articulo.Precio;
                                var total = cantidad * precio;

                                table.Cell().Text(articulo.Nombre);
                                table.Cell().Text($"₡ {precio}");
                                table.Cell().Text(cantidad.ToString());
                                table.Cell().Text($"₡ {total}");
                            }
                        });

                        //Subtotal
                        col.Item().PaddingTop(20)
                     .AlignRight().Text($"Iva: 13%").Bold().FontSize(12);
                        //Subtotal
                        col.Item().PaddingTop(10)
                     .AlignRight().Text($"Total Sin Iva: ₡ {factura.SubTotal:N2}").Bold().FontSize(12);
                        // Monto total de la factura
                        col.Item().PaddingTop(10)
                      .AlignRight().Text($"Total con Iva: ₡ {factura.MontoTotal:N2}").Bold().FontSize(16);
                    });

                    // Pie de página con el número de página
                    page.Footer().Row(row =>
                    {
                        row.RelativeItem().AlignLeft().Text("Gracias por su compra").FontSize(12).FontColor("#777777");
                        row.RelativeItem().AlignRight().Text(txt =>
                        {
                            txt.Span("Página ").FontSize(10).FontColor("#777777");
                            txt.CurrentPageNumber().FontSize(10).FontColor("#777777");
                            txt.Span(" de ").FontSize(10).FontColor("#777777");
                            txt.TotalPages().FontSize(10).FontColor("#777777");
                        });
                    });
                });
            }).GeneratePdf();

            // Devuelve el archivo PDF generado
            using (var stream = new MemoryStream(pdf))
            {
                try
                {
                    factura.UltimaFechaImpresion = DateTime.Now;

                    await _context.SaveChangesAsync();

                    return File(stream.ToArray(), "application/pdf", "Factura-" + factura.FacturaId + ".pdf");
                }
                catch (Exception ex)
                {
                    TempData["SweetAlertScript"] = "<script>Swal.fire({\r\n  title: \"Error\",\r\n  text: \"No se pudo imprimir la factura, reintentelo mas tarde.\",\r\n  icon: \"error\"\r\n, confirmButtonColor: \"#E14848\"});;</script>";
                }

                return RedirectToAction("Factura");

            }
        }

        [HttpGet]
        public async Task<IActionResult> Graficos()
        {

            try
            {
                var top4Productos = await _context.ArticuloFactura
                    .Include("Articulo")
                    .GroupBy(a => new { a.Articulo.ArticuloId, a.Articulo.Nombre }) // Agrupa por Id y Nombre del producto
                    .Select(g => new
                    {
                        ArticuloId = g.Key.ArticuloId,
                        Nombre = g.Key.Nombre,
                        CantidadTotal = g.Sum(a => a.CantidadArticulo) // Suma las cantidades para cada grupo
                    })
                    .OrderByDescending(g => g.CantidadTotal) // Ordena por CantidadTotal de manera descendente
                    .Take(4) // Toma solo los primeros 4 productos
                    .ToListAsync();

                var top4Categorias = await _context.ArticuloFactura
                    .Include(a => a.Articulo)
                    .ThenInclude(a => a.Categoria) // Incluye la categoría relacionada
                    .GroupBy(a => new { a.Articulo.Categoria.CategoriaId, a.Articulo.Categoria.Nombre }) // Agrupa por categoría
                    .Select(g => new
                    {
                        CategoriaId = g.Key.CategoriaId,
                        NombreCategoria = g.Key.Nombre,
                        CantidadTotal = g.Sum(a => a.CantidadArticulo) // Suma las cantidades para cada categoría
                    })
                    .OrderByDescending(g => g.CantidadTotal) // Ordena por CantidadTotal de manera descendente
                    .Take(4) // Toma solo las 4 categorías principales
                    .ToListAsync();

                var facturas = await _context.Factura.ToListAsync();

                // Luego, agrupa y procesa los datos en memoria
                var ventasPorMes = facturas
                    .GroupBy(f => new {
                        Year = f.FechaCreacion.Year,
                        Month = f.FechaCreacion.Month
                    })
                    .Select(g => new
                    {
                        Fecha = new DateTime(g.Key.Year, g.Key.Month, 1),
                        TotalVentas = g.Sum(f => f.MontoTotal)
                    })
                    .OrderBy(g => g.Fecha)
                    .ToList();

                return Json(new { success = true, productos = top4Productos, categorias = top4Categorias, ventas = ventasPorMes });


            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Ocurrió un error al tratar de obtener los datos." });
            }



        }




        [HttpGet]
		public async Task<IActionResult> ObtenerArticulos()
		{

			var articulos = await _context.Articulo.ToListAsync();
			var imagenes = await _context.Imagen.ToListAsync();
			var categorias = await _context.Categoria.ToListAsync();

			foreach (var articulo in articulos)
			{
				articulo.Imagen = imagenes.FirstOrDefault(a => a.ImagenId == articulo.codigoImagen);
				articulo.Categoria = categorias.FirstOrDefault(a => a.CategoriaId == articulo.idCategoria);
			}


			return Json(articulos);

		}

		public UserViewModel Conversion(Usuario usuario, string rol = "") {
			var usuarioConvertido = new UserViewModel
            {
				UsuarioId = usuario.Id,
				Nombre = usuario.UserName,
				Correo = usuario.Email,
				Rol = rol,
				Estado = usuario.State,
				Telefono = usuario.PhoneNumber,
			};

			return usuarioConvertido;
		}
	}
}
