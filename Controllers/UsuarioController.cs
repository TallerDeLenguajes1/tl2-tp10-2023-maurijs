using System.Diagnostics;
using EspacioRepositorios;
using Microsoft.AspNetCore.Mvc;
using tp10.Models;
using Tp11.ViewModels;

namespace tp10.Controllers;

public class UsuarioController : Controller
{
    private readonly ILogger<UsuarioController> _logger;
    private readonly IUsuarioRepository usuarioRepository;

    public UsuarioController(ILogger<UsuarioController> logger, IUsuarioRepository usuarioRepository ){
        _logger = logger;
        this.usuarioRepository = usuarioRepository;
    }

    //Muestra Usuarios
    public IActionResult Index(){
        //Si no esta loggeado redirecciona al index de login
        if(!IsLogged()) return RedirectToAction("Index", "Login");
        var usuarios = usuarioRepository.GetAll();
        var usuariosVM = ListarUsuarioViewModel.ToViewModel(usuarios); 
        return View(usuariosVM);
    }

    [HttpGet]
    public IActionResult AgregarUsuario(){ //Si agrego parametros envia un bad request
        if(!IsLogged()) return RedirectToAction("Index", "Login");
        return View(new UsuarioViewModel());
    }

    [HttpPost]
    public IActionResult AgregarUsuarioFromForm(UsuarioViewModel usuarioVM){
        if(!IsLogged()) return RedirectToAction("Index", "Login");
        if(!ModelState.IsValid) return RedirectToAction("Index");
        var usuario = usuarioVM.ToModel();
        usuarioRepository.CrearUsuario(usuario);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult EditarUsuario(int idUsuario){  
        if(!IsLogged())return RedirectToAction("Index", "Login");
        if(!IsAdmin()) return RedirectToAction("Index");
        var usuario = usuarioRepository.GetUsuarioById(idUsuario);
        var usuarioVM = new UsuarioViewModel(usuario);
        return View(usuarioVM);
    }

    [HttpPost]
    public IActionResult EditarUsuarioFromForm(UsuarioViewModel usuarioVM){
        if(!IsLogged()) return RedirectToAction("Index", "Login");
        if(IsAdmin()) 
        {
            var usuario = usuarioVM.ToModel();
            usuarioRepository.ModificarUsuario(usuario);
        }
        return RedirectToAction("Index");
    }

    public IActionResult EliminarUsuario(int idUsuario){
        // Si no se aclara que Login es el controller buscaria una accion en el controller actual
        //Si no esta logueado debe loguearse
        if(!IsLogged()) return RedirectToAction("Index", "Login");
        var usuarioAEliminar = usuarioRepository.GetUsuarioById(idUsuario);
        //Solo se puede borrar si es administrador o si queres borrar tu propio usuario
        if(IsAdmin())
        {   //La vista requiere un UsuarioViewModel
            if(usuarioAEliminar != null) return View(new UsuarioViewModel(usuarioAEliminar));
        }  
        return RedirectToAction("Index", "Login");
    }

    public IActionResult EliminarFromFormulario(UsuarioViewModel usuarioVM)
    {
        if(!IsLogged()) return RedirectToAction("Index", "Login"); 
        //Si no es admin o si el usuario que quiere eliminar no es el mismo entonces sale
        usuarioRepository.EliminarUsuario(usuarioVM.Id);
        return RedirectToAction("Index");   
    }

    public IActionResult Privacy(){
        return View();
    }
    
    private bool IsAdmin()
    {
        if(HttpContext.Session != null && HttpContext.Session.GetString("Rol") ==  Enum.GetName(Rol.administrador)){
            return true;
        }
        return false;
    }
    private bool IsLogged()
    {
        if (HttpContext.Session != null && !string.IsNullOrEmpty(HttpContext.Session.GetString("Nombre")) ) return true;
        return false;
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error(){
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
