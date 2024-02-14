    using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using tp10.Models;
namespace Tp11.ViewModels;

public class UpdateTableroViewModel{
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
    
    public UpdateTableroViewModel(){}
    public UpdateTableroViewModel(Tablero t)
    {
        id = t.Id;
        nombre = t.Nombre;
        descripcion = t.Descripcion;
        idUsuarioPropietario = t.IdUsuarioPropietario;
    }

}