using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using tp10.Models;
namespace Tp11.ViewModels;

public class AddTareaViewModel{
    public List<Tablero> TablerosDisponibles {get; set;}
    public List<Usuario> UsuariosParaAsignar {get; set;}
    public int CantidadDeTableros {get;set;}

    [Required(ErrorMessage = "Este campo es requerido.")]
    public int IdTablero { get; set ; }
    
    [Required(ErrorMessage = "Este campo es requerido.")]
    public string Nombre { get; set; }
    [Required(ErrorMessage = "Este campo es requerido.")]
    public string Descripcion { get; set; }
    [Required(ErrorMessage = "Este campo es requerido.")]
    public string Color { get; set ; }
    
    [Required(ErrorMessage = "Este campo es requerido.")]
    public int IdUsuarioAsignado {get; set;}
    public AddTareaViewModel(){}

    public AddTareaViewModel(Tarea t)
    {
        Nombre = t.Nombre;
        Descripcion = t.Descripcion;
        Color = t.Color;
        IdTablero = t.IdTablero;
    }



    //Recibe una lista de tareas y crea un lista de TareasViewModel
    public static List<AddTareaViewModel> ToViewModel(List<Tarea> listaTareas)
    {
        var listaTareaVM = new List<AddTareaViewModel>();
        foreach (var tarea in listaTareas)
        {
            var TareaVM = new AddTareaViewModel(tarea);
            listaTareaVM.Add(TareaVM);
        }
        return listaTareaVM;
    }

    // Devuelve su modelo correspondiente
    public Tarea ToModel()
    {
        var tarea = new Tarea
        {
            Nombre = Nombre,
            Descripcion = Descripcion,
            IdTablero = IdTablero,
            Color = Color,
            IdUsuarioAsignado = IdUsuarioAsignado
        };
        return tarea;
    }
}