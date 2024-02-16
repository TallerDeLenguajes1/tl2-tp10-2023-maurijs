using tp10.Models;
namespace EspacioRepositorios
{
    public interface ITareaRepository
    {
        Tarea CrearTarea(Tarea tarea);

        //Tareas asignadas al usuario
        List<Tarea> GetTareasAsignadasAUsuario(int idUsuario);
        List<Tarea> GetAllTareasDeTablero(int? idTablero);

        //Tareas pertenecientes a sus tableros (que pueden estar asignadas a otro usuario)
        List<Tarea> GetTareasFromTablerosDelUsuario(int idUsuario);
        List<Tarea> GetAllTareas();
        Tarea GetTareaById(int id);
        Tarea ModificarTarea(Tarea tarea);
        int EliminarTarea(int idTarea);
        int AsignarUsuarioATarea(int idUsuario, int idTarea);

    }
}