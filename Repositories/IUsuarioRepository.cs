using EspacioTareas;
namespace EspacioRepositorios
{
    public interface IUsuarioRepository
    {
        Usuario CrearUsuario(Usuario user);
        List<Usuario> GetAll();
        Usuario ModificarUsuario(int id, Usuario user);
        Usuario GetUsuarioById(int id);
        int EliminarUsuario(int id);
    }
}