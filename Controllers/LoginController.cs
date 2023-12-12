namespace tp10.Controllers;
using System.Diagnostics;
using EspacioRepositorios;
using Microsoft.AspNetCore.Mvc;
using Tp11.ViewModels;
using tp10.Models;

public class LoginController : Controller
{
    private readonly ILogger<UsuarioController> _logger;
    private readonly IUsuarioRepository usuarioRepository;
    public LoginController(ILogger<UsuarioController> logger, IUsuarioRepository usuarioRepository)
    {
        _logger = logger;
        this.usuarioRepository = usuarioRepository;
    }

    public IActionResult Index()
    {
        LoginViewModel loginVM = new LoginViewModel();
        return View(loginVM);
    }
    public IActionResult Login(LoginViewModel login)//endpoint de control de acceso
    {
            if(!ModelState.IsValid) RedirectToAction("Index"); // verifica que el LoginViewModel sea valido
            var usuario = usuarioRepository.GetUsuarioByPassAndName(login.Nombre, login.Contrasenia);
            LoguearUsuario(usuario);
            return RedirectToAction("Index", "Usuario"); //Me lleva al index de usuario

    }
    private void LoguearUsuario(Usuario usuarioPorLoguear)
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
