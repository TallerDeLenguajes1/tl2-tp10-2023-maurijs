using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using tp10.Models;
namespace Tp11.ViewModels;

public class AddTareaViewModel{
    private int id;
    [Required(ErrorMessage = "Este campo es requerido.")]
    public int Id { get => id; set => id = value; }
    private int idTablero;
    [Required(ErrorMessage = "Este campo es requerido.")]
    public int IdTablero { get => idTablero; set => idTablero = value; }
    private string nombre;
    [Required(ErrorMessage = "Este campo es requerido.")]
    public string Nombre { get => nombre; set => nombre = value; }
    private EstadoTarea estado;
    [Required(ErrorMessage = "Este campo es requerido.")]
    public EstadoTarea Estado { get => estado; set => estado = value; }
    private string descripcion;
    [Required(ErrorMessage = "Este campo es requerido.")]
    public string Descripcion { get => descripcion; set => descripcion = value; }
    private string color;
    [Required(ErrorMessage = "Este campo es requerido.")]
    public string Color { get => color; set => color = value; }
    private int idUsuarioAsignado;
    [Required(ErrorMessage = "Este campo es requerido.")]
    public int IdUsuarioAsignado { get => idUsuarioAsignado; set => idUsuarioAsignado = value; }
    
    public AddTareaViewModel(){}

    public AddTareaViewModel(Tarea t)
    {
        id = t.Id;
        nombre = t.Nombre;
        descripcion = t.Descripcion;
        color = t.Color;
        idUsuarioAsignado = t.IdUsuarioAsignado;
        idTablero = t.IdTablero;
        estado = t.Estado;
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
            Id = id,
            IdTablero = idTablero,
            Color = color,
            Estado = estado, 
            IdUsuarioAsignado = idUsuarioAsignado
        };
        return tarea;
    }
}