using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using tp10.Models;
namespace Tp11.ViewModels;

public class AddTareaViewModel{

    public List<Tablero> tablerosDisponibles {get; set;}
    public List<Usuario> usuariosParaAsignar {get; set;}
    private int idTablero;
    [Required(ErrorMessage = "Este campo es requerido.")]
    public int IdTablero { get => idTablero; set => idTablero = value; }
    private string nombre;
    [Required(ErrorMessage = "Este campo es requerido.")]
    public string Nombre { get => nombre; set => nombre = value; }
    private string descripcion;
    [Required(ErrorMessage = "Este campo es requerido.")]
    public string Descripcion { get => descripcion; set => descripcion = value; }
    private string color;
    [Required(ErrorMessage = "Este campo es requerido.")]
    public string Color { get => color; set => color = value; }
    
    [Required(ErrorMessage = "Este campo es requerido.")]
    public int IdUsuarioAsignado {get; set;}
    public AddTareaViewModel(){}

    public AddTareaViewModel(Tarea t)
    {
        nombre = t.Nombre;
        descripcion = t.Descripcion;
        color = t.Color;
        idTablero = t.IdTablero;
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
            Nombre = nombre,
            Descripcion = descripcion,
            IdTablero = idTablero,
            Color = color,
        };
        return tarea;
    }
}