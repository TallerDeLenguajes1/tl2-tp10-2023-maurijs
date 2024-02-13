using System.Diagnostics;
using EspacioRepositorios;
using Microsoft.AspNetCore.Mvc;
using tp10.Models;
using Tp11.ViewModels;

namespace tp10.Controllers;

public class TareaController : Controller
{
    private readonly ILogger<TareaController> _logger;
    private readonly ITareaRepository tareaRepository;
    private readonly ITableroRepository tableroRepository;
    private readonly IUsuarioRepository usuarioRepository;
    private int IdUsuarioLogueado => Convert.ToInt32(HttpContext.Session.GetString("Id"));
    
    //Inyeccion de dependencias
    public TareaController(ILogger<TareaController> logger, ITareaRepository tareaRepository, ITableroRepository tableroRepository, IUsuarioRepository usuarioRepository){
        _logger = logger;
        this.tableroRepository = tableroRepository;
        this.tareaRepository = tareaRepository;
        this.usuarioRepository = usuarioRepository;
    }


    //Muestra Usuarios
    public IActionResult Index(){
        if(!IsLogged()) return RedirectToAction("Index", "Login");

        var listaTareas = new List<Tarea>();
        var tareasViewModel = new GetTareasViewModel();

        try{
            if (IsAdmin())
            {
                listaTareas = tareaRepository.GetAllTareas();
                tareasViewModel.TodasLastareas = GetTareasViewModel.ToViewModel(listaTareas); 
            }else {
                //Tareas asignadas al usuario
                listaTareas = tareaRepository.GetAllTareasDeUsuario(IdUsuarioLogueado);
                tareasViewModel.TareasAsignadasAlUsuario = GetTareasViewModel.ToViewModel(listaTareas); 

                //Tareas pertenecientes a sus tableros (pueden o no estar asignadas a el mismo)
                var tareasDeSusTableros = tareaRepository.GetTareasFromTableros(IdUsuarioLogueado);
                tareasViewModel.TareasFromTablerosDelUsuario = GetTareasViewModel.ToViewModel(tareasDeSusTableros); 
                
            }
            return View(tareasViewModel);
        } 
        catch (Exception ex)
        {
            _logger.LogError($"Error: {ex.ToString}");
        }
        return RedirectToAction("Index");
    }


    [HttpGet]
    public IActionResult AgregarTarea(){ //Si agrego parametros envia un bad request
        if(!IsLogged()) return RedirectToAction("Index", "Login");
        var Disponibles = tableroRepository.GetAllTablerosDeUsuario(IdUsuarioLogueado);
        var addTareaVM = new AddTareaViewModel
        {
            tablerosDisponibles = Disponibles
        };
        return View(addTareaVM);
    }

    [HttpPost]
    public IActionResult AgregarTareaFromForm(AddTareaViewModel tareaVM)
    {
        if(!IsLogged()) return RedirectToAction("Index", "Login");
        if(!ModelState.IsValid) return RedirectToAction("Index");;
        try
        {
            var tarea = tareaVM.ToModel();
            tareaRepository.CrearTarea(tarea);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error: {ex.ToString}");
        }
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult UpdateTarea(int idTarea){  
        try
        {
            if(!IsLogged()) return RedirectToAction("Index", "Login");
            var tarea = tareaRepository.GetTareaById(idTarea);
            if(tarea == null) return RedirectToAction("Index");
            var tareaVM = new UpdateTareaViewModel(tarea); 
            return View(tareaVM);
        }
        catch (Exception ex) 
        {
            _logger.LogError($"Error: {ex.ToString}");
        }
        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult EditarTareaFromForm(GetTareaViewModel Model){
        if(!IsLogged()) return RedirectToAction("Index", "Login");
        if(!ModelState.IsValid) return RedirectToAction("Index");
        try
        {
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
        catch (Exception ex)
        {
            _logger.LogError($"Error: {ex.ToString}");
        }
        return BadRequest();
    }

    public IActionResult AsignarTarea(int idTarea)
    {
        if(!IsLogged()) return RedirectToAction("Index", "Login");
        var usuarios = usuarioRepository.GetAll();
        var AsignarTareaVM = new AsignarTareaViewModel(idTarea, usuarios);
        return View(AsignarTareaVM);
    }

    [HttpPost]
    public IActionResult AsignarTareaFromForm(AsignarTareaViewModel AsignarTareaVM)
    {
        if(!IsLogged()) return RedirectToAction("Index", "Login");
        if(!ModelState.IsValid) return RedirectToAction("Index");
        try{
            var tarea = tareaRepository.GetTareaById(AsignarTareaVM.IdTarea);
            tarea.IdUsuarioAsignado = AsignarTareaVM.IdUsuarioAsignado;
            var resultado = tareaRepository.ModificarTarea(tarea);
            return RedirectToAction("Index");
        }catch (Exception ex)
        {
            _logger.LogError($"Error: {ex.ToString}");
        }
        return BadRequest();
    }


    public IActionResult EliminarTarea(int idTarea){
        if(!IsLogged()) return RedirectToAction("Index", "Login");
        try
        {
            tareaRepository.EliminarTarea(idTarea);
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error: {ex.ToString}");
        }
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
        if (HttpContext.Session != null && !string.IsNullOrEmpty(HttpContext.Session.GetString("Nombre"))) return true;
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
