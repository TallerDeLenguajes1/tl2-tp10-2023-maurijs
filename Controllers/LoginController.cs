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

    [HttpGet]
    public IActionResult Index()
    {
        LoginViewModel loginVM = new LoginViewModel();
        return View(loginVM);
    }

    [HttpPost]
    public IActionResult Login(LoginViewModel login)//endpoint de control de acceso
    {
        try
        {
            if(!ModelState.IsValid) RedirectToAction("Index"); // verifica que el LoginViewModel sea valido
            var usuario = usuarioRepository.GetUsuarioByPassAndName(login.Nombre, login.Contrasenia);
            if(usuario.Nombre == null) {
                ModelState.AddModelError(string.Empty, "Usuario o contraseña incorrectos.");
                return View("Index");
            }
            LoguearUsuario(usuario);
            _logger.LogInformation("Usuario " + usuario.Nombre + " logueado exitosamente");
            return RedirectToAction("Index", "Home");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            _logger.LogWarning(
                "Intento de acceso invalido - Usuario: " + login.Nombre + "/Contrasenia: " +login.Contrasenia
            );
        }
        return RedirectToAction("Index"); //Me lleva al index de usuario
    }

    
    [HttpGet]
    public IActionResult Desloguear() {
        //Si no esta logueado
        if(HttpContext.Session.GetString("Nombre") == string.Empty) return RedirectToAction("Index");
        try { 
            var idUsuarioLogueado = Convert.ToInt32(HttpContext.Session.GetString("Id"));
            var usuarioLogueado = usuarioRepository.GetUsuarioById(idUsuarioLogueado);
            DesloguearUsuario();
            _logger.LogInformation("Usuario" + usuarioLogueado.Nombre + " deslogueado exitosamente");
            return RedirectToAction("Index");
        } catch (Exception e) {
            _logger.LogError(e.ToString());
            _logger.LogWarning("Couldn't unlog user");
            return RedirectToAction("Index", "Home");
        }
    }
    private void LoguearUsuario(Usuario usuarioPorLoguear)
    {
        HttpContext.Session.SetString("Nombre", usuarioPorLoguear.Nombre);
        HttpContext.Session.SetString("Id", usuarioPorLoguear.Id.ToString());
        HttpContext.Session.SetString("Contrasenia", usuarioPorLoguear.Contrasenia);
        HttpContext.Session.SetString("Rol", Enum.GetName(usuarioPorLoguear.Rol));
    }

    private void DesloguearUsuario() {
        HttpContext.Session.Remove("Id");
        HttpContext.Session.Remove("Nombre");
        HttpContext.Session.Remove("contrasenia");
        HttpContext.Session.Remove("Rol");
    } 


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error(){
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
