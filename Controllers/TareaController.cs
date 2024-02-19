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
    public IActionResult Index(int? idTablero){
        try{
            if(!IsLogged()) return RedirectToAction("Index", "Login");

            var listaTareas = new List<Tarea>();
            var ViewModel = new ListarTareasViewModel {
                VerTableroIndividual = idTablero.HasValue,
                IsAdmin = IsAdmin(),
                IdUsuarioLogueado = IdUsuarioLogueado
            };

            if (idTablero.HasValue){
                listaTareas = tareaRepository.GetAllTareasDeTablero(idTablero);
                ViewModel.NombreDelTablero = tableroRepository.GetTableroById(idTablero).Nombre;
                ViewModel.ListaTareas = ListarTareasViewModel.ToViewModel(listaTareas);            
                return View(ViewModel);
            }

            if (IsAdmin())
            {
                listaTareas = tareaRepository.GetAllTareas();
                ViewModel.ListaTareas = ListarTareasViewModel.ToViewModel(listaTareas);
            }else {
                //Tareas asignadas al usuario
                var tareasDelUsuario = tareaRepository.GetTareasAsignadasAUsuario(IdUsuarioLogueado);
                //Tareas  pertenecientes a sus tableros
                listaTareas = tareaRepository.GetTareasFromTablerosDelUsuario(IdUsuarioLogueado);
                //Concateno ambas listas
                foreach (var tarea in tareasDelUsuario)
                {
                    if (!Contiene(listaTareas, tarea.Id))
                    {
                        listaTareas.Add(tarea);
                    }
                }

                ViewModel.ListaTareas = ListarTareasViewModel.ToViewModel(listaTareas);
            }
            return View(ViewModel);
        } 
        catch (Exception ex)
        {
            _logger.LogError($"Error: {ex.ToString()}");
            return RedirectToAction("Index");
        }
    }

    private bool Contiene(List<Tarea> listaTareas, int idTarea)
    {
        foreach (var tarea in listaTareas)
        {
            if (idTarea == tarea.Id) return true;
        }
        return false;
    }

    [HttpGet]
    public IActionResult AgregarTarea(){ //Si agrego parametros envia un bad request
        if(!IsLogged()) return RedirectToAction("Index", "Login");
        var cantidadDeTableros = tableroRepository.GetAllTablerosDeUsuario(IdUsuarioLogueado).Count();

        /*if(cantidadDeTableros == 0) { Otra solucion es establecer un mensaje
            ViewData["Error"] = "Debes crear un tablero primero para poder crear una tarea.";
            ViewBag.Mensaje = "Debes crear un tablero primero para poder crear una tarea.";
            TempData["Error"] = "Debes crear un tablero primero para poder crear una tarea."; // TempData es temporal, la variable se limpia luego de otra peticion
        }*/
        var addTareaVM = new AddTareaViewModel
        {
            TablerosDisponibles =  tableroRepository.GetAllTablerosDeUsuario(IdUsuarioLogueado),
            UsuariosParaAsignar = usuarioRepository.GetAll(),
            CantidadDeTableros = cantidadDeTableros
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
            var tareaVM = new UpdateTareaViewModel(tarea)
            {
                UsuariosParaAsignar = usuarioRepository.GetAll(),
                TablerosParaAsignar = tableroRepository.GetAllTablerosDeUsuario(tarea.IdPropietario),
                NombreTableroAsignado = tableroRepository.GetTableroById(tarea.IdTablero).Nombre
            }; 
            return View("UpdateTarea", tareaVM);
        }
        catch (Exception ex) 
        {
            _logger.LogError($"Error: {ex.ToString}");
            return RedirectToAction("Index");
        }
    }

    [HttpPost]
    public IActionResult EditarTareaFromForm(UpdateTareaViewModel Model){
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
