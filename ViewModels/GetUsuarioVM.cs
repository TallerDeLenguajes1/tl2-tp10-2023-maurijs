using System.ComponentModel.DataAnnotations;
using tp10.Models;
namespace Tp11.ViewModels;
public class GetUsuarioViewModel{
    private int id;

    [Required(ErrorMessage = "Este campo es requerido.")]
    public int Id { get => id; set => id = value; }
    private string nombre;
    [Required(ErrorMessage = "Este campo es requerido.")]
    public string Nombre { get => nombre; set => nombre = value; }
    public GetUsuarioViewModel(Usuario usuario)
    {
        id = usuario.Id;
        nombre = usuario.Nombre;
    }
    public static List<GetUsuarioViewModel> ToViewModel(List<Usuario> usuarios)
    {
        var listaUsuariosVM = new List<GetUsuarioViewModel>();

        foreach (var usuario in usuarios)
        {
            var usuarioVM = new GetUsuarioViewModel(usuario);
            listaUsuariosVM.Add(usuarioVM);
        }
        return listaUsuariosVM;
    }
    
}