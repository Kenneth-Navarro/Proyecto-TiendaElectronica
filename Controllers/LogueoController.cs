using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Proyecto_TiendaElectronica.ViewModels;
using Proyecto_TiendaElectronica.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace Proyecto_TiendaElectronica.Controllers
{
    public class LogueoController : Controller
    {
        private readonly AppDBContext _context;
        private readonly UserManager<Usuario> _userManager;
        private readonly SignInManager<Usuario> _signInManager;

        public LogueoController(AppDBContext context, UserManager<Usuario> userManager, SignInManager<Usuario> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Index() {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }


            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(UserLogin model) {
            if (ModelState.IsValid) {
                var user = await _userManager.FindByNameAsync(model.UserName);

                if (user != null)
                {

                    if (!user.State)
                    {
                        TempData["SweetAlertScript"] = "<script>Swal.fire({\r\n  title: \"Error\",\r\n  text: \"El estado del usuario es inactivo.\",\r\n  icon: \"error\"\r\n, confirmButtonColor: \"#E14848\"});;</script>";
                        return View(model);
                    }

                    if (await _userManager.IsLockedOutAsync(user))
                    {
                        TempData["SweetAlertScript"] = "<script>Swal.fire({\r\n  title: \"Error\",\r\n  text: \"Su cuenta de usuario esta bloqueada.\",\r\n  icon: \"error\"\r\n, confirmButtonColor: \"#E14848\"});;</script>";
                        return View(model);
                    }

                    var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, lockoutOnFailure: true);

                    if (result.Succeeded)
                    {
                        var usuario = await _userManager.FindByNameAsync(model.UserName);

                        var roles = await _userManager.GetRolesAsync(usuario);

                        var rol = roles.FirstOrDefault();


                        return RedirectToAction("Index", "Home");



                    }
                    else if (!result.IsNotAllowed)
                    {
                        TempData["SweetAlertScript"] = "<script>Swal.fire({\r\n  title: \"Error\",\r\n  text: \"El usuario o la contraseña no coinciden.\",\r\n  icon: \"error\"\r\n, confirmButtonColor: \"#E14848\"});;</script>";
                    }
                    else
                    {
                        ModelState.AddModelError("", "No se pudo realizar el inicio de sesión, por favor intentelo más tarde.");
                        TempData["SweetAlertScript"] = "<script>Swal.fire({\r\n  title: \"Error\",\r\n  text: \"No se pudo realizar el inicio de sesión, por favor reintentelo más tarde.\",\r\n  icon: \"error\"\r\n, confirmButtonColor: \"#E14848\"});;</script>";
                    }
                }else{
                    TempData["SweetAlertScript"] = "<script>Swal.fire({\r\n  title: \"Error\",\r\n  text: \"El usuario de usuario ingresado no existe.\",\r\n  icon: \"error\"\r\n, confirmButtonColor: \"#E14848\"});;</script>";
                }
            }

            return View(model);
        }

        public async Task<IActionResult> Logout() {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Registro() {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registro(UserRegister model) {
            if (ModelState.IsValid) {
                var usuario = new Usuario { Id = model.UsuarioId, UserName = model.Nombre, Email = model.Correo, PhoneNumber = model.Telefono, State = true };

                try
                {
                    var result = await _userManager.CreateAsync(usuario, model.Contrasena);

                    if (result.Succeeded)
                    {

                        await _userManager.AddToRoleAsync(usuario, "Usuario");

                        return RedirectToAction("Index");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            if (error.Code == "DuplicateUserName")
                            {
                                ModelState.AddModelError("", "No se pudo realizar el registro del usuario, por favor intentelo más tarde.");
                                TempData["SweetAlertScript"] = "<script>Swal.fire({\r\n  title: \"Error\",\r\n  text: \"No se pudo realizar el registro, el nombre de usuario ingresado ya esta registrado. Por favor ingrese un nombre de usuario diferente.\",\r\n  icon: \"error\"\r\n, confirmButtonColor: \"#E14848\"});;</script>";
                            }
                            else
                            {
                                ModelState.AddModelError("", error.Description);
                                TempData["SweetAlertScript"] = "<script>Swal.fire({\r\n  title: \"Error\",\r\n  text: \"No se pudo realizar el registro. Por favor reintentelo más tarde.\",\r\n  icon: \"error\"\r\n, confirmButtonColor: \"#E14848\"});;</script>";
                            }
                        }




                    }
                }
                catch (DbUpdateException ex)
                {
                    if (ex.InnerException != null && ex.InnerException.Message.Contains("restricción PRIMARY KEY"))
                    {
                        TempData["SweetAlertScript"] = "<script>Swal.fire({\r\n  title: \"Error\",\r\n  text: \"No se pudo realizar el registro. El usuario con la cédula '" + model.UsuarioId + "' ya sido registrado anteriormente. Por favor ingrese un nuevo número de cédula.\",\r\n  icon: \"error\"\r\n, confirmButtonColor: \"#E14848\"});;</script>";
                    }
                    else {
                        TempData["SweetAlertScript"] = "<script>Swal.fire({\r\n  title: \"Error\",\r\n  text: \"No se pudo realizar el registro. Por favor reintentelo más tarde.\",\r\n  icon: \"error\"\r\n, confirmButtonColor: \"#E14848\"});;</script>";
                    }
                    
                }
                catch (Exception ex) {
                    TempData["SweetAlertScript"] = "<script>Swal.fire({\r\n  title: \"Error\",\r\n  text: \"No se pudo realizar el registro. Por favor reintentelo más tarde.\",\r\n  icon: \"error\"\r\n, confirmButtonColor: \"#E14848\"});;</script>";
                }
                

            }

            return View(model);

        }
    }
}
