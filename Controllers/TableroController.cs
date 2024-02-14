namespace tp10.Controllers;
using System.Diagnostics;
using EspacioRepositorios;
using Microsoft.AspNetCore.Mvc;
using Tp11.ViewModels;
using tp10.Models;

public class TableroController : Controller
{
    private readonly ILogger<TableroController> _logger;

    private readonly ITableroRepository tableroRepository;
    private readonly ITareaRepository tareaRepository;
    private readonly IUsuarioRepository usuarioRepository;
    private int IdUsuarioLogueado => Convert.ToInt32(HttpContext.Session.GetString("Id"));

    public TableroController(ILogger<TableroController> logger, ITableroRepository tableroRepository, ITareaRepository tareaRepository,IUsuarioRepository usuarioRepository)
    {
        _logger = logger;
        this.tableroRepository = tableroRepository;
        this.tareaRepository = tareaRepository;
        this.usuarioRepository = usuarioRepository;
    }

   public IActionResult Index(){
        if(!IsLogged()) return RedirectToAction("Index", "Login");
        List<Tablero> listaTableros;
        if (IsAdmin())
        {
            listaTableros = tableroRepository.GetAllTableros();
        } else{
            //No es admin
            // Tableros que sean propiedad del usuario logueado
            listaTableros = tableroRepository.GetAllTablerosDeUsuario(IdUsuarioLogueado);
            var tareas = tareaRepository.GetAllTareasDeUsuario(IdUsuarioLogueado);
            // Si un tablero posee una tarea del usuario logueado (por mas que no sea el propietario) debe mostrarse en el index
            foreach (var tarea in tareas)
            {
                var tablero = tableroRepository.GetTableroById(tarea.IdTablero);
                if (!listaTableros.Contains(tablero))
                {
                    listaTableros.Add(tablero);
                }
            } 
        }
        return View(GetTableroViewModel.ToViewModel(listaTableros));
    }

    [HttpGet]
    public IActionResult AgregarTablero(){ //Si agrego parametros envia un bad request
        if(!IsLogged()) return RedirectToAction("Index", "Login");
        var tableroVM = new AddTableroViewModel
        {
            IdUsuarioPropietario = IdUsuarioLogueado,
        };
        return View(tableroVM); 
    }

    [HttpPost]
    public IActionResult AgregarTableroFromForm(AddTableroViewModel tableroVM){
        // Si el modelo (TableroVM) no es valido vuelve al index
        if(!IsLogged()) return RedirectToAction("Index", "Login");
        if(!ModelState.IsValid) return RedirectToAction("Index");
        try{
            //Convierto el tablero view model a Tablero
            var tablero = tableroVM.ToModel();
            tableroRepository.CrearTablero(tablero);
            return RedirectToAction("Index");       
        }   
        catch (Exception ex) {
            _logger.LogError($"Error: {ex.ToString}");
        }
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult EditarTablero(int idTablero){  
        if(!IsLogged()) return RedirectToAction("Index", "Login");
        try{
            var tablero = tableroRepository.GetTableroById(idTablero);
            return View(new UpdateTableroViewModel(tablero));
        }  
        catch (Exception ex){
            _logger.LogError($"Error: {ex.ToString}");
        }
        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult EditarTableroFromForm(UpdateTableroViewModel tableroVM){
        if(!IsLogged()) return RedirectToAction("Index", "Login");
        try{
            var tablero = tableroVM.ToModel();
            tableroRepository.ModificarTablero(tablero);
            return RedirectToAction("Index");
        }
        catch (Exception ex) {
            _logger.LogError($"Error: {ex.ToString}");
        }
        return RedirectToAction("Index");
    }

    public IActionResult DeleteTablero(int idTablero){
        if(!IsLogged()) return RedirectToAction("Index", "Login");
        try{
            tableroRepository.EliminarTablero(idTablero);
            return RedirectToAction("Index");
        }
        catch (Exception ex) {
            _logger.LogError($"Error: {ex.ToString}");
        }
        return RedirectToAction("Index");
        
    }

    public IActionResult Privacy(){
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error(){
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
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

}
