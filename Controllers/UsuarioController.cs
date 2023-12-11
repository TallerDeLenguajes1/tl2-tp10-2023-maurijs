using System.Diagnostics;
using EspacioRepositorios;
using EspacioTareas;
using Microsoft.AspNetCore.Mvc;
using Tp11.ViewModels;

namespace tp10.Controllers;

public class UsuarioController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly UsuarioRepository repositoryUsuario;

    public UsuarioController(ILogger<HomeController> logger){
        _logger = logger;
        repositoryUsuario = new UsuarioRepository();
    }

    //Muestra Usuarios
    public IActionResult Index(){
        //Si no esta loggeado redirecciona al index de login
        if(!IsLogged()) RedirectToAction("Index","Login");
        var usuarios = repositoryUsuario.GetAll();
        var usuariosVM = ListarUsuarioViewModel.FromlistaTolistaViewModel(usuarios); 
        return View(usuariosVM);
    }

    [HttpGet]
    public IActionResult AgregarUsuario(){ //Si agrego parametros envia un bad request
        return View(new Usuario());
    }

    [HttpPost]
    public IActionResult AgregarUsuarioFromForm([FromForm] Usuario usuario){
        repositoryUsuario.CrearUsuario(usuario);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult EditarUsuario(int idUsuario){  
        return View(repositoryUsuario.GetUsuarioById(idUsuario));
    }

    [HttpPost]
    public IActionResult EditarUsuarioFromForm([FromForm] Usuario usuario, int id){
        usuario.Id = id;
        repositoryUsuario.ModificarUsuario(usuario);
        return RedirectToAction("Index");
    }

    public IActionResult EliminarUsuario(int idUsuario){
        // Si no se aclara que Login es el controller buscaria una accion en el controller actual
        //Si no esta logueado debe loguearse
        if(!IsLogged()) return RedirectToAction("Index", "Login");
        var usuarioAEliminar = repositoryUsuario.GetUsuarioById(idUsuario);
        //Solo se puede borrar si es administrador o si queres borrar tu propio usuario
        if(IsAdmin() || idUsuario == Convert.ToInt32(HttpContext.Session.GetString("Id")))
        {
            if(usuarioAEliminar != null) return View(usuarioAEliminar);
        } 
        return NotFound();
    }

    public IActionResult EliminarFromFormulario(Usuario usuario)
    {
        if(!IsLogged()) return RedirectToAction("Index","Login"); 
        //Si no es admin o si el usuario que quiere eliminar no es el mismo entonces sale
        if(!IsAdmin() || usuario.Id != Convert.ToInt32(HttpContext.Session.GetString("Id"))) return RedirectToAction("Index"); 
        repositoryUsuario.EliminarUsuario(usuario.Id);
        return RedirectToAction("Index");
        
    }

    public IActionResult Privacy(){
        return View();
    }
    
    private bool IsAdmin() => HttpContext.Session.GetString("Rol") == Enum.GetName(Rol.administrador);
    private bool IsLogged() => HttpContext.Session == null && string.IsNullOrEmpty(HttpContext.Session.GetString("Usuario")); 
    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error(){
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
