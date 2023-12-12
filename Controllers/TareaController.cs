using System.Diagnostics;
using EspacioRepositorios;
using Microsoft.AspNetCore.Mvc;
using tp10.Models;
using Tp11.ViewModels;

namespace tp10.Controllers;

public class TareaController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly TareaRepository tareaRepository;

    public TareaController(ILogger<HomeController> logger){
        _logger = logger;
        tareaRepository = new TareaRepository();
    }


    //Muestra Usuarios
    public IActionResult Index(){
        if(!IsLogged()) return RedirectToRoute(new { controller = "Login", action = "Index"});
        var listaTareas = new List<Tarea>();
        if (IsAdmin())
        {
            listaTareas = tareaRepository.GetAllTareas();
        }
        else{
            //No es admin
            var idUsuario = Convert.ToInt32(HttpContext.Session.GetString("Id"));
            listaTareas = tareaRepository.GetAllTareasDeUsuario(idUsuario);
        }
        return View(TareaViewModel.ToViewModel(listaTareas));
    }

    [HttpGet]
    public IActionResult AgregarTarea(){ //Si agrego parametros envia un bad request
        if(!IsLogged()) return RedirectToAction("Index", "Login");
        return View(new TareaViewModel());
    }

    [HttpPost]
    public IActionResult AgregarTareaFromForm(TareaViewModel tareaVM){
        if(!IsLogged()) return RedirectToAction("Index", "Login");
        if(!IsAdmin()) return RedirectToAction("Index");
        if(!ModelState.IsValid) return RedirectToAction("Index");;
        var tarea = tareaVM.ToModel();
        tareaRepository.CrearTarea(tarea);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult EditarTarea(int idTarea){  
        if(!IsLogged()) return RedirectToAction("Index", "Login");
        var tarea = tareaRepository.GetTareaById(idTarea);
        if(tarea == null) return RedirectToAction("Index");
        var tareaVM = new TareaViewModel(tarea); 
        return View(tareaVM);
    }

    [HttpPost]
    public IActionResult EditarTareaFromForm(TareaViewModel Model){
        if(!IsLogged()) return RedirectToAction("Index", "Login");
        if(!ModelState.IsValid) return RedirectToAction("Index");

        var tareaAEditar = tareaRepository.GetTareaById(Model.Id);
        if(tareaAEditar == null) return RedirectToAction("Index");
        tareaAEditar.Nombre = Model.Nombre;
        tareaAEditar.IdTablero = Model.IdTablero;
        tareaAEditar.Descripcion = Model.Descripcion;
        tareaAEditar.IdUsuarioAsignado = Model.IdUsuarioAsignado;
        tareaAEditar.Color = Model.Color;
        tareaAEditar.Estado = Model.Estado;
        tareaRepository.ModificarTarea(tareaAEditar);
        return RedirectToAction("Index");
    }

    public IActionResult EliminarTarea(int idTarea){
        if(!IsLogged()) return RedirectToAction("Index", "Login");
        tareaRepository.EliminarTarea(idTarea);
        return RedirectToAction("Index");
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

    public IActionResult Privacy(){
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error(){
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

}
