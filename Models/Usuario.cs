namespace EspacioTareas
{
    public class Usuario
    {
        int id;
        string nombre;

        public int Id { get => id; set => id = value; }
        public string Nombre { get => nombre; set => nombre = value; }
        public Usuario(){}
        public Usuario(int id, string nombre)
        {
            this.id = id;
            this.nombre = nombre;
        }

    }
    
}