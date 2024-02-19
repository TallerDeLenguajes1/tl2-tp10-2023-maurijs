namespace tp10.Models
{
    public class Tablero {

        private int id;
        private int idPropietario;
        private string nombre;
        private string descripcion;
        private string nombrePropietario;
        public int Id { get => id; set => id = value; }
        public int IdUsuarioPropietario { get => idPropietario; set => idPropietario = value; }
        public string Nombre { get => nombre; set => nombre = value; }
        public string Descripcion { get => descripcion; set => descripcion = value; }
        public string NombreUsuarioPropietario { get => nombrePropietario; set => nombrePropietario = value; }
    }
}