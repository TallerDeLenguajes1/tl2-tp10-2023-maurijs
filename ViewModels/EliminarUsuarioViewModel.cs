using System.ComponentModel.DataAnnotations;
using tp10.Models;
namespace Tp11.ViewModels;

public class EliminarUsuarioViewModel{
    private int id;
    [Required(ErrorMessage = "Este campo es requerido.")]
    public int Id { get => id; set => id = value; }   

    public EliminarUsuarioViewModel(int id)
    {   
        this.id = id;
    }
}