    using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using tp10.Models;
namespace Tp11.ViewModels;

public class GetTableroViewModel{
    private int id;
    // atributos de validacion
    [Required(ErrorMessage = "Este campo es requerido.")]  // El campo id es requerido obligatoriamente    
    public int Id { get => id; set => id = value; }
    private int idUsuarioPropietario;
    [Required(ErrorMessage = "Este campo es requerido.")]

    public int IdUsuarioPropietario { get => idUsuarioPropietario; set => idUsuarioPropietario = value; }
    private string nombre;
    [Required(ErrorMessage = "Este campo es requerido.")]

    public string Nombre { get => nombre; set => nombre = value; }
    private string descripcion;  
    [Required(ErrorMessage = "Este campo es requerido.")]

    public string Descripcion { get => descripcion; set => descripcion = value; }

    [Required(ErrorMessage = "Este campo es requerido.")]
    public string NombrePropietario {get; set;}

    public Tablero ToModel()
    {
        var tablero = new Tablero
        {
            Nombre = nombre,
            Descripcion = descripcion,
            Id = id,
            IdUsuarioPropietario = idUsuarioPropietario 
        };
        return tablero;
    }
    
    public GetTableroViewModel(){}
    public GetTableroViewModel(Tablero t)
    {
        id = t.Id;
        nombre = t.Nombre;
        descripcion = t.Descripcion;
        idUsuarioPropietario = t.IdUsuarioPropietario;
    }

    public static List<GetTableroViewModel> ToViewModel(List<Tablero> tableros)
    {
        List<GetTableroViewModel> ListarTableroVM = new List<GetTableroViewModel>();
        
        foreach (var tablero in tableros)
        {
            var newTVM = new GetTableroViewModel(tablero);
            ListarTableroVM.Add(newTVM);
        }
        return ListarTableroVM;
    }

}

public class GetTablerosViewModel
{
    public List<GetTableroViewModel> ListaTableros {get;set;}
    public bool IsAdmin {get;set;}
    public bool VerTablerosDeUsuarioIndividual;
    public string NombreUsuario {get;set;}
}