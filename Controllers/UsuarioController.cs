using System.Diagnostics;
using EspacioRepositorios;
using Microsoft.AspNetCore.Mvc;
using tp10.Models;
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
        if(!IsLogged()) return NotFound();
        var usuarios = repositoryUsuario.GetAll();
        var usuariosVM = ListarUsuarioViewModel.FromlistaTolistaViewModel(usuarios); 
        return View(usuariosVM);
    }

    [HttpGet]
    public IActionResult AgregarUsuario(){ //Si agrego parametros envia un bad request
        if(!IsLogged()) return NotFound();
        return View(new Usuario());
    }

    [HttpPost]
    public IActionResult AgregarUsuarioFromForm([FromForm] Usuario usuario){
        if(!IsLogged()) return NotFound();
        repositoryUsuario.CrearUsuario(usuario);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult EditarUsuario(int idUsuario){  
        if(!IsLogged()) return NotFound();
        if(!IsAdmin()) return RedirectToAction("Index");
        var usuario = repositoryUsuario.GetUsuarioById(idUsuario);
        var usuarioVM = new UsuarioViewModel(usuario);
        return View(usuarioVM);
    }

    [HttpPost]
    public IActionResult EditarUsuarioFromForm([FromForm] Usuario usuario, int id){
        if(!IsLogged()) return NotFound();
        if(IsAdmin()) 
        {
            usuario.Id = id;
            repositoryUsuario.ModificarUsuario(usuario);
        }
        return RedirectToAction("Index");
    }

    public IActionResult EliminarUsuario(int idUsuario){
        // Si no se aclara que Login es el controller buscaria una accion en el controller actual
        //Si no esta logueado debe loguearse
        if(!IsLogged()) return NotFound();
        var usuarioAEliminar = repositoryUsuario.GetUsuarioById(idUsuario);
        //Solo se puede borrar si es administrador o si queres borrar tu propio usuario
        if(IsAdmin())
        {   //La vista requiere un UsuarioViewModel
            if(usuarioAEliminar != null) return View(new UsuarioViewModel(usuarioAEliminar));
        }  
        return RedirectToAction("Index", "Login");
    }

    public IActionResult EliminarFromFormulario(Usuario usuario)
    {
        if(!IsLogged()) return NotFound(); 
        //Si no es admin o si el usuario que quiere eliminar no es el mismo entonces sale
        repositoryUsuario.EliminarUsuario(usuario.Id);
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
        if (HttpContext.Session != null) return true;
        return false;
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error(){
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
