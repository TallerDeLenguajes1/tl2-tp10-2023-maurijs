using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using EspacioRepositorios;
using tp10.Models;
namespace Tp11.ViewModels;

public class GetTareaViewModel{

    [Required(ErrorMessage = "Este campo es requerido.")]
    public int Id { get; set; }
    [Required(ErrorMessage = "Este campo es requerido.")]
    public int IdTablero { get; set; }
    [Required(ErrorMessage = "Este campo es requerido.")]
    public string Nombre { get; set; }
    [Required(ErrorMessage = "Este campo es requerido.")]
    public EstadoTarea Estado { get; set; }
    [Required(ErrorMessage = "Este campo es requerido.")]
    public string Descripcion { get; set; }
    [Required(ErrorMessage = "Este campo es requerido.")]
    public string Color{ get; set; }
    [Required(ErrorMessage = "Este campo es requerido.")]
    public int IdUsuarioAsignado { get; set; }
    [Required(ErrorMessage = "Este campo es requerido.")]
    public string NombreUsuarioAsignado {get;set;}
    
    public GetTareaViewModel(){}

    public GetTareaViewModel(Tarea t)
    {
        Id = t.Id;
        Nombre = t.Nombre;
        Descripcion = t.Descripcion;
        Color = t.Color;
        IdUsuarioAsignado = t.IdUsuarioAsignado;
        IdTablero = t.IdTablero;
        Estado = t.Estado;
    }


    // Devuelve su modelo correspondiente
    public Tarea ToModel()
    {
        var tarea = new Tarea
        {
            Nombre = Nombre,
            Descripcion = Descripcion,
            Id = Id,
            IdTablero = IdTablero,
            Color = Color,
            Estado = Estado, 
            IdUsuarioAsignado = IdUsuarioAsignado
        };
        return tarea;
    }
}

public class GetTareasViewModel
{
    private readonly IUsuarioRepository _usuarioRepository;
    public bool IsAdmin {get;set;}
    public List<GetTareaViewModel> TareasAsignadasAlUsuario {get; set;}
    public List<GetTareaViewModel> TareasFromTablerosDelUsuario{get; set;}
    public List<GetTareaViewModel> TodasLasTareas{get; set;}
    //Recibe una lista de tareas y crea un lista de TareasViewModel

    public GetTareasViewModel(IUsuarioRepository usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
    }
    public static List<GetTareaViewModel> ToViewModel(List<Tarea> listaTareas)
    {
        var listaTareaVM = new List<GetTareaViewModel>();
        foreach (var tarea in listaTareas)
        {
            var TareaVM = new GetTareaViewModel(tarea);
            listaTareaVM.Add(TareaVM);
        }
        return listaTareaVM;
    }
    public void GetNombresDeUsuario()
    {
        var usuarios = _usuarioRepository.GetAll();
        //Solo si es admin se mostraran todas las tareas tareas
        if (IsAdmin)
        {   
            if (TodasLasTareas != null)
            {
                foreach (var tarea in TodasLasTareas)
                {
                    var nombre = usuarios.FirstOrDefault(u => u.Id == tarea.IdUsuarioAsignado).Nombre;
                    tarea.NombreUsuarioAsignado = nombre;
                }
            }    
        } else {
            if(TareasAsignadasAlUsuario != null)
            {
                foreach (var tarea in TareasAsignadasAlUsuario)
                {
                    var nombre = usuarios.FirstOrDefault(u => u.Id == tarea.IdUsuarioAsignado).Nombre;
                    tarea.NombreUsuarioAsignado = nombre ?? "Sin asignar";
                }
            }

            if(TareasFromTablerosDelUsuario != null)
            {
                foreach (var tarea in TareasFromTablerosDelUsuario)
                {
                    foreach (var u in usuarios)
                    {
                        if (u.Id == tarea.IdUsuarioAsignado)
                        {
                            tarea.NombreUsuarioAsignado = u.Nombre;
                            break;
                        }
                    }
                } 
            }
        }
    }
}