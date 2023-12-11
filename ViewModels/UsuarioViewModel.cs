using System.ComponentModel.DataAnnotations;
using tp10.Models;
namespace Tp11.ViewModels;

public class UsuarioViewModel{
    private int id;
    [Required(ErrorMessage = "Este campo es requerido.")]
    public int Id { get => id; set => id = value; }   
    private string nombre;
    [Required(ErrorMessage = "Este campo es requerido.")]
    public string Nombre { get => nombre; set => nombre = value; }
    private string contrasenia;
    [Required(ErrorMessage = "Este campo es requerido.")]
    public string Contrasenia { get => contrasenia; set => contrasenia = value; }
    private Rol rol;
    [Required(ErrorMessage = "Este campo es requerido.")]
    public Rol Rol { get => rol; set => rol = value; }
    public UsuarioViewModel(Usuario usuario)
    {   
        nombre = usuario.Nombre;
        id = usuario.Id;
        contrasenia = usuario.Contrasenia;
        Rol = usuario.Rol;
    }
}