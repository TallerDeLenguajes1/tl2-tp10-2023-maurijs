using System.Data.SQLite;
using EspacioTareas;
namespace EspacioRepositorios
{
    public class UsuarioRepository : IUsuarioRepository
    {
        //La direccion de conexion debe estar en settings json
        private readonly string cadenaConexion = "Data Source=DB/kanban.sql;Cache=Shared";
        public Usuario CrearUsuario(Usuario user)
        {
            var query = "INSERT INTO Usuario (nombre_usuario) VALUES (@nombre)";
            using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
            {
                try{
                    connection.Open();
                    using (var command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.Add(new SQLiteParameter("@nombre", user.Nombre));
                        command.ExecuteNonQuery(); 
                    }
                }
                catch (Exception ex){
                    Console.WriteLine($"Ha ocurrido un error: {ex.Message}");
                }  
                finally{
                    connection.Close();
                }
            }
            return user;
        }
        public List<Usuario> GetAll()
        {
            List<Usuario> usuarios = new List<Usuario>();
            string queryString = "SELECT id, nombre_usuario FROM Usuario;";
            using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
            {
                try{
                    connection.Open();
                    using (SQLiteCommand command = new SQLiteCommand(queryString, connection))
                    {
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var usuario = new Usuario
                                {
                                    Id = Convert.ToInt32(reader["id"]),
                                    Nombre = reader["nombre_usuario"].ToString()
                                };
                                usuarios.Add(usuario);
                            }
                        }
                    }
                }
                catch (Exception ex){
                    Console.WriteLine($"Ha ocurrido un error: {ex.Message}");
                }  
                finally{
                    connection.Close();
                }
            }
            return usuarios;
        }
        public Usuario ModificarUsuario(Usuario user)
        {
            using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
            {
                try{
                    connection.Open();
                    using SQLiteCommand command = connection.CreateCommand();
                    command.CommandText = "UPDATE Usuario SET nombre_usuario = @nombre WHERE id = @id;";
                    command.Parameters.Add(new SQLiteParameter("@id", user.Id));
                    command.Parameters.Add(new SQLiteParameter("@nombre", user.Nombre));
                    command.ExecuteNonQuery();   
                } 
                catch (Exception ex){
                        Console.WriteLine($"Ha ocurrido un error en modificar usuario: {ex.Message}");
                }
                finally{
                        connection.Close();
                }
            }
            return user;
        }

        public Usuario GetUsuarioById(int idUsuario)
        {
            var usuario = new Usuario();
            using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion)){
                try{
                    connection.Open();
                    using SQLiteCommand command = connection.CreateCommand();
                    command.CommandText = "SELECT * FROM Usuario WHERE id = @idUsuario";
                    command.Parameters.Add(new SQLiteParameter("@idUsuario", idUsuario));
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            usuario.Id = Convert.ToInt32(reader["id"]);
                            usuario.Nombre = reader["nombre_usuario"].ToString();
                        }
                    }
                }
                catch (Exception ex){
                    Console.WriteLine($"Ha ocurrido un error en GetUsuarioById: {ex.Message}");
                }   
                finally{
                    connection.Close();
                }
            }
            return usuario;
        }
        public int EliminarUsuario(int id)
        {
            int filasAfectadas = 0;
            using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion)){
                try
                {
                    connection.Open();
                    using SQLiteCommand command = connection.CreateCommand();
                    command.CommandText = "DELETE FROM Usuario WHERE id = @id;";
                    command.Parameters.Add(new SQLiteParameter("id", id));
                    filasAfectadas = command.ExecuteNonQuery();
                }
                catch (Exception ex){
                    Console.WriteLine($"Ha ocurrido un error al acceder a la base de datos: {ex.Message}");
                }  
                finally{
                    connection.Close();
                } 
            }
            return filasAfectadas;
        }
    
        public Usuario GetUsuarioByPassAndName(string nombre, string password)
        {
            var usuario = new Usuario();
            using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion)){
                try{
                    connection.Open();
                    using SQLiteCommand command = connection.CreateCommand();
                    command.CommandText = "SELECT * FROM Usuario WHERE nombre_usuario = @nombre AND contrasenia = @password";
                    command.Parameters.Add(new SQLiteParameter("@nombre", nombre));
                    command.Parameters.Add(new SQLiteParameter("@password", password));
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            usuario.Id = Convert.ToInt32(reader["id"]);
                            usuario.Nombre = reader["nombre_usuario"].ToString();
                        }
                    }
                }
                catch (Exception ex){
                    Console.WriteLine($"Ha ocurrido un error: {ex.Message}");
                }   
                finally{
                    connection.Close();
                }
            }
            return usuario;
        }
    }
}