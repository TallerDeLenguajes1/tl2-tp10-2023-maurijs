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

    //ERROR
    // Delete CASCADE a los usuarios y tableros. Y si borro el usuario, las tareas quedan sin asignar
    //Esto va fuera del viewModel (podria agregar en el modelo de tarea el nombre del usuario asignado, y el nombre del tablero al que pertenece)
    public List<ElementoTareaViewModel> ListaTareas {get; set;}
    public bool VerTableroIndividual {get;set;} //Solo es true cuando quiero ver las tareas de un solo tablero
    public string NombreDelTablero {get;set;} //Solo para cuando quiero ver todas las tareas de un determinado tablero
    public int IdPropietarioDelTablero {get;set;} //Solo para cuando quiero ver todas las tareas de un determinado tablero
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