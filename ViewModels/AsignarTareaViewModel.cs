using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using tp10.Models;
namespace Tp11.ViewModels;

public class AsignarTareaViewModel
{
    [Required(ErrorMessage = "Este campo es requerido.")]
    public int IdTarea {get; set;}
    [Required(ErrorMessage = "Este campo es requerido.")]
    public int IdUsuarioAsignado {get; set;}
    
    public List<Usuario> usuarios;
    public AsignarTareaViewModel(){}
    public AsignarTareaViewModel(int IdTarea, List<Usuario> usuarios)
    {
        this.IdTarea = IdTarea;
        this.usuarios = usuarios;
        IdUsuarioAsignado = -1;
    }
}