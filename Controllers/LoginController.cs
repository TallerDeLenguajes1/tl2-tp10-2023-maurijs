
using EspacioRepositorios;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Tp11.ViewModels;
using EspacioTareas;
namespace tp10.Controllers;
    
public class LoginController : Controller{

    //List<Login> listaDeTiposDelogins = new List<Login>();
    private readonly ILogger<LoginController> _logger;
    private readonly UsuarioRepository repo;
    public LoginController(ILogger<LoginController> logger)
    {
        _logger = logger;
        var repo = new UsuarioRepository();
    }

    public IActionResult Index()
    {
        LoginViewModel loginVM = new LoginViewModel();
        return View(loginVM);
    }
    public IActionResult Login(LoginViewModel login)//endpoint de control de acceso
    {
            var usuario = repo.GetUsuarioByPassAndName(login.Nombre, login.Contrasenia);
            logearUsuario(usuario);
            return RedirectToRoute(new { controller = "Usuario", action = "Index" }); //Me lleva al index de usuario
            
    }
    private void logearUsuario(EspacioTareas.Usuario usuarioPorLoguear)
    {
        HttpContext.Session.SetString("Nombre", usuarioPorLoguear.Nombre);
        HttpContext.Session.SetString("Id", usuarioPorLoguear.Id.ToString());
        HttpContext.Session.SetString("Contrasenia", usuarioPorLoguear.Contrasenia);
        HttpContext.Session.SetString("Rol", Enum.GetName(usuarioPorLoguear.Rol));
    }
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error(){
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
