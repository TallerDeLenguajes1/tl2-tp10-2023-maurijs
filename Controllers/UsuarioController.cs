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
    private readonly ITableroRepository tableroRepository;
    private readonly ITareaRepository tareaRepository;
    private int IdUsuarioLogueado => Convert.ToInt32(HttpContext.Session.GetString("Id"));

    public UsuarioController(ILogger<UsuarioController> logger, IUsuarioRepository usuarioRepository, ITableroRepository tableroRepository, ITareaRepository tareaRepository){
        _logger = logger;
        this.usuarioRepository = usuarioRepository;
        this.tableroRepository = tableroRepository;
        this.tareaRepository = tareaRepository;
    }

    //Muestra Usuarios
    public IActionResult Index(){
        //Si no esta loggeado redirecciona al index de login
        if(!IsLogged()) return RedirectToAction("Index", "Login");
        try{
            var usuarios = usuarioRepository.GetAll();
            var ViewModel = new ListarUsuariosViewModel
            {
                ListaUsuarios = ElementoUsuarioViewModel.ToViewModel(usuarios),
                IsAdmin = IsAdmin(),
                IdUsuarioLogueado = IdUsuarioLogueado
            };
            return View(ViewModel);
        }catch(Exception ex){
            _logger.LogError(ex.ToString());
            return RedirectToAction("Index");
        }
    }

    [HttpGet]
    public IActionResult AgregarUsuario(){ //Si agrego parametros envia un bad request
        if(!IsLogged()) return RedirectToAction("Index", "Login");
        if(!IsAdmin()) return RedirectToAction("Index", "Home");
        return View(new AddUsuarioViewModel());
    }

    [HttpPost]
    public IActionResult AgregarUsuarioFromForm(AddUsuarioViewModel usuarioVM){
        if(!IsLogged()) return RedirectToAction("Index", "Login");
        if(!IsAdmin()) return RedirectToAction("Index", "Home");
        if(!ModelState.IsValid) return RedirectToAction("Index");
        try{
            var usuario = usuarioVM.ToModel();
            usuarioRepository.CrearUsuario(usuario);
        }
        catch(Exception ex)
        {
            _logger.LogError(ex.ToString());
            return RedirectToAction("Index");
        } 
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult EditarUsuario(int idUsuario){  
        if(!IsLogged())return RedirectToAction("Index", "Login");
        if(!IsAdmin()) return RedirectToAction("Index", "Home");
        var usuario = usuarioRepository.GetUsuarioById(idUsuario);
        var ViewModel = new UpdateUsuarioViewModel(usuario)
        {
            IdUsuarioLogueado = IdUsuarioLogueado
        };
        
        return View(ViewModel);
    }

    [HttpPost]
    public IActionResult EditarUsuarioFromForm(UpdateUsuarioViewModel usuarioVM){
        if(!IsLogged()) return RedirectToAction("Index", "Login");
        if(!IsAdmin()) return RedirectToAction("Index", "Home");
        try{
            var usuario = usuarioVM.ToModel();
            usuarioRepository.ModificarUsuario(usuario); 
            return RedirectToAction("Index");
        } catch(Exception ex){
            _logger.LogError(ex.ToString());
            return RedirectToAction("Index");
        }
    }

    public IActionResult EliminarUsuario(int idUsuario){
        //Si no esta logueado debe loguearse
        if(!IsLogged()) return RedirectToAction("Index", "Login");
        //Solo se puede borrar si es administrador
        if(!IsAdmin()) return RedirectToAction("Index", "Home");
        try{

            var usuarioAEliminar = usuarioRepository.GetUsuarioById(idUsuario);
            if(usuarioAEliminar != null)  return View(new EliminarUsuarioViewModel(idUsuario));
           return RedirectToAction("Index");    
        }catch(Exception ex){
            _logger.LogError(ex.ToString());
            return RedirectToAction("Index");
        }
    }

    public IActionResult EliminarFromFormulario(int idUsuario)
    {
        try
        {
            if(!IsLogged()) return RedirectToAction("Index", "Login");  
            if(!IsAdmin()) return RedirectToAction("Index", "Home");  

            EliminarTablerosDelUsuario(idUsuario);
            //A las tareas donde el usuario estaba asignado debo dejar el campo idUsuarioAsignado en nulll
            foreach (var tarea in tareaRepository.GetTareasAsignadasAUsuario(idUsuario))
            {   
                tarea.IdUsuarioAsignado = -1;
                tareaRepository.ModificarTarea(tarea);
            }

            usuarioRepository.EliminarUsuario(idUsuario);
            return RedirectToAction("Index"); 
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            return RedirectToAction("Index");   
        }
    }

    public void EliminarTablerosDelUsuario(int idUsuario)
    {
        foreach (var tablero in tableroRepository.GetAllTablerosDeUsuario(idUsuario))
            {   
                //Primero debo eliminar los tableros del usuario, y las tareas pertenecientes a esos tableros
                EliminarTareasDelTablero(tablero.Id);
                tableroRepository.EliminarTablero(tablero.Id);
            }
    }
    public void EliminarTareasDelTablero(int IdTablero)
    {
        foreach (var tarea in tareaRepository.GetAllTareasDeTablero(IdTablero))
        {   
            tareaRepository.EliminarTarea(tarea.Id);
        }
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
