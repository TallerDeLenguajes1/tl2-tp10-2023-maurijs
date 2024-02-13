using tp10.Models;
namespace EspacioRepositorios
{
    public interface ITareaRepository
    {
        Tarea CrearTarea(Tarea tarea);
        List<Tarea> GetAllTareasDeUsuario(int idUsuario);
        List<Tarea> GetAllTareasDeTablero(int idTablero);
        List<Tarea> GetTareasFromTableros(int idUsuario);
        List<Tarea> GetAllTareas();
        Tarea GetTareaById(int id);
        Tarea ModificarTarea(Tarea tarea);
        int EliminarTarea(int idTarea);
        int AsignarUsuarioATarea(int idUsuario, int idTarea);

    }
}