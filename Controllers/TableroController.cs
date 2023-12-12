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

    public TableroController(ILogger<TableroController> logger, ITableroRepository tableroRepository)
    {
        _logger = logger;
        this.tableroRepository = tableroRepository;
    }

   public IActionResult Index(){
        if(!IsLogged()) return RedirectToAction("Index", "Login");
        List<Tablero> listaTableros;
        if (IsAdmin())
        {
            listaTableros = tableroRepository.GetAllTableros();
        } else{
            //No es admin
            var idUsuario = Convert.ToInt32(HttpContext.Session.GetString("Id"));
            listaTableros = tableroRepository.GetAllTablerosDeUsuario(idUsuario);
        }
        return View(TableroViewModel.ToViewModel(listaTableros));
        /*En este caso, estás devolviendo la vista sin especificar el nombre de la vista, pero estás pasando un objeto de tipo List<Tablero> como modelo a esa vista. Esto asume que hay una vista con el mismo nombre del método de acción que está siendo ejecutado. Por ejemplo, si tu método de acción se llama Detalle y devuelves View(producto), ASP.NET MVC buscará automáticamente una vista llamada "Detalle" para renderizar y le pasará el objeto Producto como modelo.*/
    }

    [HttpGet]
    public IActionResult AgregarTablero(){ //Si agrego parametros envia un bad request
        if(!IsLogged()) return RedirectToAction("Index", "Login");
        if(!IsAdmin()) return RedirectToAction("Index");
        return View(new TableroViewModel()); 
    }

    [HttpPost]
    public IActionResult AgregarTableroFromForm(TableroViewModel tableroVM){
        // Si el modelo (TableroVM) no es valido vuelve al index
        if(!IsLogged()) return RedirectToAction("Index", "Login");
        if(!IsAdmin()) return RedirectToAction("Index");
        if(!ModelState.IsValid) return RedirectToAction("Index");
        //Convierto el tablero view model a Tablero
        var tablero = tableroVM.ToModel();
        tableroRepository.CrearTablero(tablero);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult EditarTablero(int idTablero){  
        if(!IsLogged()) return RedirectToAction("Index", "Login");
        if(!IsAdmin()) return RedirectToAction("Index");
        var tablero = tableroRepository.GetTableroById(idTablero);
        return View(new TableroViewModel(tablero));
    }

    [HttpPost]
    public IActionResult EditarTableroFromForm(TableroViewModel tableroVM){
        if(!IsLogged()) return RedirectToAction("Index", "Login");
        if(!IsAdmin()) return RedirectToAction("Index");
        var tablero = tableroVM.ToModel();
        tableroRepository.ModificarTablero(tablero);
        return RedirectToAction("Index");
    }

    public IActionResult DeleteTablero(int idTablero){
        if(!IsLogged()) return RedirectToAction("Index", "Login");
        if(!IsAdmin()) return RedirectToAction("Index");
        tableroRepository.EliminarTablero(idTablero);
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
