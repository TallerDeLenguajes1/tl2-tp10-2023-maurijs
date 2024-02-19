using System.ComponentModel.DataAnnotations;
using tp10.Models;
namespace Tp11.ViewModels;
public class ElementoUsuarioViewModel{
    private int id;

    [Required(ErrorMessage = "Este campo es requerido.")]
    public int Id { get => id; set => id = value; }
    private string nombre;
    [Required(ErrorMessage = "Este campo es requerido.")]
    public string Nombre { get => nombre; set => nombre = value; }
    public ElementoUsuarioViewModel(Usuario usuario)
    {
        id = usuario.Id;
        nombre = usuario.Nombre;
    }
    public static List<ElementoUsuarioViewModel> ToViewModel(List<Usuario> usuarios)
    {
        var listaUsuariosVM = new List<ElementoUsuarioViewModel>();

        foreach (var usuario in usuarios)
        {
            var usuarioVM = new ElementoUsuarioViewModel(usuario);
            listaUsuariosVM.Add(usuarioVM);
        }
        return listaUsuariosVM;
    }
    
}

public class ListarUsuariosViewModel
{
    public List<ElementoUsuarioViewModel> ListaUsuarios {get;set;}
    public bool IsAdmin {get;set;}
    public int IdUsuarioLogueado {get;set;}
}