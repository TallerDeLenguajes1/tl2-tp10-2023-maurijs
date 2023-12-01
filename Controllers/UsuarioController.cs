using System.Diagnostics;
using EspacioRepositorios;
using EspacioTareas;
using Microsoft.AspNetCore.Mvc;
using tp10.Models;

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
        var usuarios = repositoryUsuario.GetAll();
        return View(usuarios);
    }

    [HttpGet]
    public IActionResult AgregarUsuario(){ //Si agrego parametros envia un bad request
        return View(new Usuario());
    }

    [HttpPost]
    public IActionResult AgregarUsuarioFromForm([FromForm] Usuario usuario){
        repositoryUsuario.CrearUsuario(usuario);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult EditarUsuario(int idUsuario){  
        return View(repositoryUsuario.GetUsuarioById(idUsuario));
    }

    [HttpPost]
    public IActionResult EditarUsuarioFromForm([FromForm] Usuario usuario, int id){
        usuario.Id = id;
        repositoryUsuario.ModificarUsuario(usuario);
        return RedirectToAction("Index");
    }

    public IActionResult DeleteUsuario(int idUsuario){
        repositoryUsuario.EliminarUsuario(idUsuario);
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
