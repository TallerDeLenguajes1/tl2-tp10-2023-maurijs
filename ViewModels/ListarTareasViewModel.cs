using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using EspacioRepositorios;
using tp10.Models;
namespace Tp11.ViewModels;

public class ElementoTareaViewModel{ // elementoTareaViewModel

    [Required(ErrorMessage = "Este campo es requerido.")]
    public int Id { get; set; }
    [Required(ErrorMessage = "Este campo es requerido.")]
    public int IdTablero { get; set; }
    [Required(ErrorMessage = "Este campo es requerido.")]
    public string Nombre { get; set; }
    [Required(ErrorMessage = "Este campo es requerido.")]
    public EstadoTarea Estado { get; set; }
    [Required(ErrorMessage = "Este campo es requerido.")]
    public string Descripcion { get; set; }
    [Required(ErrorMessage = "Este campo es requerido.")]
    public string Color{ get; set; }
    [Required(ErrorMessage = "Este campo es requerido.")]
    public int IdUsuarioAsignado { get; set; }
    [Required(ErrorMessage = "Este campo es requerido.")]
    public string NombreUsuarioAsignado {get;set;}
    [Required(ErrorMessage = "Este campo es requerido.")]
    public string NombreUsuarioPropietario {get;set;}
    [Required(ErrorMessage = "Este campo es requerido.")]
    public int IdPropietario {get;set;}
    
    public ElementoTareaViewModel(){}

    public ElementoTareaViewModel(Tarea t)
    {
        Id = t.Id;
        Nombre = t.Nombre;
        Descripcion = t.Descripcion;
        Color = t.Color;
        IdUsuarioAsignado = t.IdUsuarioAsignado;
        IdTablero = t.IdTablero;
        Estado = t.Estado;
        NombreUsuarioAsignado = t.NombreUsuarioAsignado;
        NombreUsuarioPropietario = t.NombreUsuarioPropietario;
        IdPropietario = t.IdPropietario;
    }


    // Devuelve su modelo correspondiente
    public Tarea ToModel()
    {
        var tarea = new Tarea
        {
            Nombre = Nombre,
            Descripcion = Descripcion,
            Id = Id,
            IdTablero = IdTablero,
            Color = Color,
            Estado = Estado, 
            IdUsuarioAsignado = IdUsuarioAsignado,
            NombreUsuarioAsignado = NombreUsuarioAsignado,
            NombreUsuarioPropietario = NombreUsuarioPropietario
        };
        return tarea;
    }
}

public class ListarTareasViewModel
{
    public bool IsAdmin {get;set;}
    public int IdUsuarioLogueado {get;set;}
    public List<ElementoTareaViewModel> ListaTareas {get; set;}
    public bool VerTableroIndividual {get;set;} //Solo es true cuando quiero ver las tareas de un solo tablero
    public string NombreDelTablero {get;set;} //Solo para cuando quiero ver todas las tareas de un determinado tablero
    public static List<ElementoTareaViewModel> ToViewModel(List<Tarea> listaTareas)
    {
        var listaTareaVM = new List<ElementoTareaViewModel>();
        foreach (var tarea in listaTareas)
        {
            var TareaVM = new ElementoTareaViewModel(tarea);
            listaTareaVM.Add(TareaVM);
        }
        return listaTareaVM;
    }
}


//Cuando borro un usuario, todas las tareas donde el estaba asignado (que no sean de sus tableros), deberian figurar como sin asignal (nulo)