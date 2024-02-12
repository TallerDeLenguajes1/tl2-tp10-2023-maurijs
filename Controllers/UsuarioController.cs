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
        var usuariosVM = GetUsuarioViewModel.ToViewModel(usuarios); 
        return View(usuariosVM);
    }

    [HttpGet]
    public IActionResult AgregarUsuario(){ //Si agrego parametros envia un bad request
        if(!IsLogged()) return RedirectToAction("Index", "Login");
        return View(new AddUsuarioViewModel());
    }

    [HttpPost]
    public IActionResult AgregarUsuarioFromForm(AddUsuarioViewModel usuarioVM){
        if(!IsLogged()) return RedirectToAction("Index", "Login");
        if(!ModelState.IsValid) return RedirectToAction("Index");
        try{
            var usuario = usuarioVM.ToModel();
            usuarioRepository.CrearUsuario(usuario);
        }
        catch(Exception ex)
        {
            _logger.LogError(ex.ToString());
        } 
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult EditarUsuario(int idUsuario){  
        if(!IsLogged())return RedirectToAction("Index", "Login");
        if(!IsAdmin()) return RedirectToAction("Index");
        var usuario = usuarioRepository.GetUsuarioById(idUsuario);
        return View(new AddUsuarioViewModel(usuario));
    }

    [HttpPost]
    public IActionResult EditarUsuarioFromForm(AddUsuarioViewModel usuarioVM){
        if(!IsLogged()) return RedirectToAction("Index", "Login");
        if(IsAdmin()) 
        {
            try{
                var usuario = usuarioVM.ToModel();
                usuarioRepository.ModificarUsuario(usuario); 
            } catch(Exception ex){
                _logger.LogError(ex.ToString());
            }
        }
        return RedirectToAction("Index");
    }

    public IActionResult EliminarUsuario(int idUsuario){
        try{
        //Si no esta logueado debe loguearse
            if(!IsLogged()) return RedirectToAction("Index", "Login");
            var usuarioAEliminar = usuarioRepository.GetUsuarioById(idUsuario);
            //Solo se puede borrar si es administrador o si queres borrar tu propio usuario
            if(IsAdmin())
            {   //La vista requiere un UsuarioViewModel
                    if(usuarioAEliminar != null) return View(new AddUsuarioViewModel(usuarioAEliminar));
            }  
        }catch(Exception ex){
            _logger.LogError(ex.ToString());
        }
        return RedirectToAction("Index");
    }

    public IActionResult EliminarFromFormulario(AddUsuarioViewModel usuarioVM)
    {
        try
        {
            if(!IsLogged()) return RedirectToAction("Index", "Login"); 
            //Si no es admin o si el usuario que quiere eliminar no es el mismo entonces sale
            usuarioRepository.EliminarUsuario(usuarioVM.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
        }
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
