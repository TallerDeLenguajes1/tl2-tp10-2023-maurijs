namespace tp10.Models
{
    public enum EstadoTarea
    {
        ToDo,
        Doing, 
        Review,
        Done
    }
    
    public class Tarea {
    private int id;
    private int idTablero;
    private string nombre;
    private string descripcion;
    private string color;
    private EstadoTarea estado; 
    private int idUsuarioAsignado;
    private string nombreUsuarioAsignado;
    private int idPropietario;
    private string nombreUsuarioPropietario;
    public int Id { get => id; set => id = value; }
    public string Nombre { get => nombre; set => nombre = value; }
    public string Descripcion { get => descripcion; set => descripcion = value; }
    public string Color { get => color; set => color = value; }
    public EstadoTarea Estado { get => estado; set => estado = value; }
    public int IdUsuarioAsignado { get => idUsuarioAsignado; set => idUsuarioAsignado = value; }
    public int IdTablero { get => idTablero; set => idTablero = value; }
    public string NombreUsuarioAsignado { get => nombreUsuarioAsignado; set => nombreUsuarioAsignado = value; }
    public string NombreUsuarioPropietario { get => nombreUsuarioPropietario; set => nombreUsuarioPropietario = value; }
    public int IdPropietario { get => idPropietario; set => idPropietario = value; }
    }   
}