    using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using tp10.Models;
namespace Tp11.ViewModels;

public class AddTableroViewModel{

    [Required(ErrorMessage = "Este campo es requerido.")]
    private int idUsuarioPropietario;
    [Required(ErrorMessage = "Este campo es requerido.")]

    public int IdUsuarioPropietario { get => idUsuarioPropietario; set => idUsuarioPropietario = value; }
    private string nombre;
    [Required(ErrorMessage = "Este campo es requerido.")]

    public string Nombre { get => nombre; set => nombre = value; }
    private string descripcion;  
    [Required(ErrorMessage = "Este campo es requerido.")]

    public string Descripcion { get => descripcion; set => descripcion = value; }

    public Tablero ToModel()
    {
        var tablero = new Tablero
        {
            Nombre = nombre,
            Descripcion = descripcion,
            IdUsuarioPropietario = idUsuarioPropietario 
        };
        return tablero;
    }
    
    public AddTableroViewModel(){}
    public AddTableroViewModel(Tablero t)
    {
        nombre = t.Nombre;
        descripcion = t.Descripcion;
        idUsuarioPropietario = t.IdUsuarioPropietario;
    }

    public static List<AddTableroViewModel> ToViewModel(List<Tablero> tableros)
    {
        List<AddTableroViewModel> ListarTableroVM = new List<AddTableroViewModel>();
        
        foreach (var tablero in tableros)
        {
            var newTVM = new AddTableroViewModel(tablero);
            ListarTableroVM.Add(newTVM);
        }
        return ListarTableroVM;
    }
}