using EspacioTareas;
namespace EspacioRepositorios
{
    public interface IUsuarioRepository
    {
        Usuario CrearUsuario(Usuario user);
        List<Usuario> GetAll();
        Usuario ModificarUsuario(Usuario user);
        Usuario GetUsuarioById(int id);
        int EliminarUsuario(int id);
        Usuario GetUsuarioByPassAndName(string nombre, string password);

    }
}