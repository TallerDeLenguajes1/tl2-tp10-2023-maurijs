namespace tp10.Models
{
    public enum Rol {
        operador, 
        administrador
    }
    public class Usuario
    {
        private int id;
        private string nombre;
        private Rol rol;
        private string contrasenia;

        public int Id { get => id; set => id = value; }
        public string Nombre { get => nombre; set => nombre = value; }
        public Rol Rol { get => rol; set => rol = value; }
        public string Contrasenia { get => contrasenia; set => contrasenia = value; }

        public Usuario(){}
        public Usuario(int id, string nombre, Rol rol, string contrasenia)
        {
            this.id = id;
            this.nombre = nombre;
            this.rol = rol;
            this.contrasenia = contrasenia;
        }

    }
    
}