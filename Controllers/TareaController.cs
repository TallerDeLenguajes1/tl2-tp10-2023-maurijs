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
        if(!IsLogged()) return RedirectToAction("Index", "Login");
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
        return View(TareaViewModel.ToListTareaVM(listaTareas));
    }

    [HttpGet]
    public IActionResult AgregarTarea(){ //Si agrego parametros envia un bad request
        if(!IsLogged()) return RedirectToAction("Index", "Login");
        return View(new Tarea());
    }

    [HttpPost]
    public IActionResult AgregarTareaFromForm([FromForm] Tarea tarea){
        if(!IsLogged()) return RedirectToAction("Index", "Login");
        
        tareaRepository.CrearTarea(tarea);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult EditarTarea(int idTarea){  
        if(!IsLogged()) return RedirectToAction("Index", "Login");
        return View(tareaRepository.GetTareaById(idTarea));
    }

    [HttpPost]
    public IActionResult EditarTareaFromForm([FromForm] Tarea tarea){
        if(!IsLogged()) return RedirectToAction("Index", "Login");
        tareaRepository.ModificarTarea(tarea);
        return RedirectToAction("Index");
    }

    public IActionResult DeleteTarea(int idTarea){
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
        if (HttpContext.Session != null) return true;
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
