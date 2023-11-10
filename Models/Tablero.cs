namespace EspacioTareas
{
    public class Tablero {

        private int id;
        private int idUsuarioPropietario;
        private string nombre;
        private string descripcion;
        public int Id { get => id; set => id = value; }
        public int IdUsuarioPropietario { get => idUsuarioPropietario; set => idUsuarioPropietario = value; }
        public string Nombre { get => nombre; set => nombre = value; }
        public string Descripcion { get => descripcion; set => descripcion = value; }
    }
}