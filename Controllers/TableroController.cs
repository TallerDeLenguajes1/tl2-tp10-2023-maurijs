namespace tp10.Controllers;
using System.Diagnostics;
using EspacioRepositorios;
using Microsoft.AspNetCore.Mvc;
using Tp11.ViewModels;
using EspacioTareas;

public class TableroController : Controller
{
    private readonly ILogger<TableroController> _logger;

    private readonly TableroRepository tableroRepository;

    public TableroController(ILogger<TableroController> logger)
    {
        _logger = logger;
        tableroRepository = new TableroRepository();
    }

   public IActionResult Index(){
        var tableros = tableroRepository.GetAllTableros();
        return View(tableros);
        /*En este caso, estás devolviendo la vista sin especificar el nombre de la vista, pero estás pasando un objeto de tipo List<Tablero> como modelo a esa vista. Esto asume que hay una vista con el mismo nombre del método de acción que está siendo ejecutado. Por ejemplo, si tu método de acción se llama Detalle y devuelves View(producto), ASP.NET MVC buscará automáticamente una vista llamada "Detalle" para renderizar y le pasará el objeto Producto como modelo.*/
    }

    [HttpGet]
    public IActionResult AgregarTablero(){ //Si agrego parametros envia un bad request
        return View(new TableroViewModel()); 
    }

    [HttpPost]
    public IActionResult AgregarTableroFromForm(TableroViewModel tableroVM){
        // Si el modelo no es valido vuelve al index
        if(!ModelState.IsValid) return RedirectToAction("Index");
        //Convierto el tablero view model a Tablero
        var tablero = tableroVM.ToModel();
        tableroRepository.CrearTablero(tablero);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult EditarTablero(int idTablero){  
        return View(tableroRepository.GetTableroById(idTablero));
    }

    [HttpPost]
    public IActionResult EditarTableroFromForm([FromForm] Tablero tablero){
        tableroRepository.ModificarTablero(tablero);
        return RedirectToAction("Index");
    }

    public IActionResult DeleteTablero(int idTablero){
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

}
