using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using tp10.Models;
namespace Tp11.ViewModels;

public class UpdateTareaViewModel{

    public List<Usuario> UsuariosParaAsignar {get;set;}
    public List<Tablero> TablerosParaAsignar {get;set;}

    [Required(ErrorMessage = "Este campo es requerido.")]
    public int Id {get;set;}
    [Required(ErrorMessage = "Este campo es requerido.")]
    public int IdTablero {get;set;}
    [Required(ErrorMessage = "Este campo es requerido.")]
    public string Nombre {get;set;}
    [Required(ErrorMessage = "Este campo es requerido.")]
    public EstadoTarea Estado {get;set;}
    [Required(ErrorMessage = "Este campo es requerido.")]
    public string Descripcion {get;set;}
    [Required(ErrorMessage = "Este campo es requerido.")]
    public string Color {get;set;}
    [Required(ErrorMessage = "Este campo es requerido.")]
    public int IdUsuarioAsignado {get;set;}
    
    public UpdateTareaViewModel(){}

    public UpdateTareaViewModel(Tarea t)
    {
        Id = t.Id;
        Nombre = t.Nombre;
        Descripcion = t.Descripcion;
        Color = t.Color;
        IdUsuarioAsignado = t.IdUsuarioAsignado;
        IdTablero = t.IdTablero;
        Estado = t.Estado;
    }

    //Recibe una lista de tareas y crea un lista de TareasViewModel
    public static List<UpdateTareaViewModel> ToViewModel(List<Tarea> listaTareas)
    {
        var listaTareaVM = new List<UpdateTareaViewModel>();
        foreach (var tarea in listaTareas)
        {
            var TareaVM = new UpdateTareaViewModel(tarea);
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
            Id = Id,
            IdTablero = IdTablero,
            Color = Color,
            Estado = Estado, 
            IdUsuarioAsignado = IdUsuarioAsignado
        };
        return tarea;
    }
}