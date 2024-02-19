using System.Data.SQLite;
using tp10.Models;
namespace EspacioRepositorios
{
    public class TareaRepository : ITareaRepository
    {
        private readonly string cadenaDeConexion;

        //Inyeccion de dependencias (cadenaDeConexion esta declarado en Program.cs y toma su valor en appsettings.json)
        public TareaRepository(string cadenaDeConexion)
        {
            this.cadenaDeConexion = cadenaDeConexion;
        }

        public Tarea CrearTarea(Tarea tarea)
        {
            var query = $"INSERT INTO Tarea (id_tablero, nombre, estado, descripcion, color, id_usuario_asignado) VALUES (@id_tablero, @nombre, @estado, @descripcion, @color, @id_usuario_asignado)";
            using (SQLiteConnection connection = new SQLiteConnection(cadenaDeConexion))
            {
                try{ 
                    connection.Open();
                    //var command = connection.CreateCommand();
                    //command.CommandText = "consulta";
                    using(var command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.Add(new SQLiteParameter("@id_tablero", tarea.IdTablero));
                        command.Parameters.Add(new SQLiteParameter("@estado", tarea.Estado));
                        command.Parameters.Add(new SQLiteParameter("@nombre", tarea.Nombre));
                        command.Parameters.Add(new SQLiteParameter("@descripcion", tarea.Descripcion));
                        command.Parameters.Add(new SQLiteParameter("@color", tarea.Color));
                        if(tarea.IdUsuarioAsignado != -1){
                            command.Parameters.Add(new SQLiteParameter("@id_usuario_asignado",tarea.IdUsuarioAsignado));
                        }else {
                            command.Parameters.Add(new SQLiteParameter("@id_usuario_asignado",null));   
                        }
                        try{ // consultar si es necesario dos bloques try-catch
                            command.ExecuteNonQuery();
                        }
                        catch (Exception ex){
                            Console.WriteLine($"Ha ocurrido un error al realizar la consulta: {ex.Message}");
                        }
                    } 
                }
                catch {
                    throw;
                }  
                finally{
                    connection.Close(); 
                }
            }
            return tarea;
        }

        public int EliminarTarea(int idTarea)
        {
            int filasAfectadas = 0;
            using (SQLiteConnection connection = new SQLiteConnection(cadenaDeConexion))
            {
                try
                {
                    connection.Open();

                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "DELETE FROM Tarea WHERE id_tarea = @idTarea";
                        command.Parameters.Add(new SQLiteParameter("@idTarea", idTarea));
                        filasAfectadas = command.ExecuteNonQuery();
                    }
                }
                catch {
                    throw;
                }
                finally
                {
                    connection.Close(); // Asegura que se cierre la conexión, independientemente de si hay una excepción o no
                }
            }

            return filasAfectadas;
        }



        public Tarea GetTareaById(int idTarea)
        {
            var tarea = new Tarea();
            using (SQLiteConnection connection = new SQLiteConnection(cadenaDeConexion))
            {
                try{
                    connection.Open();
                    SQLiteCommand command = connection.CreateCommand();
                    command.CommandText = "SELECT t.id_tarea, t.id_tablero, t.id_usuario_asignado, t.nombre, t.descripcion, t.color, t.estado,  a.nombre_usuario AS usuario_asignado, p.nombre_usuario AS propietario, p.id AS id_propietario "
                                        + "FROM Tarea t "
                                        + "INNER JOIN Tablero m USING(id_tablero) "
                                        + "INNER JOIN Usuario p ON m.id_usuario_propietario = p.id "
                                        + "LEFT JOIN Usuario a ON a.id = t.id_usuario_asignado " //LEFT JOIN ya que puede haber tareas sin usuario asignado (toma valor null) 
                                        + "WHERE id_tarea = @idTarea;";
                    command.Parameters.Add(new SQLiteParameter("@idTarea", idTarea));
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            tarea.Id = Convert.ToInt32(reader["id_tarea"]);
                            tarea.IdTablero = Convert.ToInt32(reader["id_tablero"]);
                            tarea.Nombre = reader["nombre"].ToString();
                            tarea.Descripcion = reader["descripcion"].ToString();
                            tarea.Color = reader["color"].ToString();
                            tarea.Estado = (EstadoTarea)Convert.ToInt32(reader["estado"]);
                            tarea.NombreUsuarioPropietario = reader["propietario"].ToString();
                            tarea.IdPropietario = Convert.ToInt32(reader["id_propietario"]);
                            tarea.IdUsuarioAsignado = reader["id_usuario_asignado"] != DBNull.Value ? Convert.ToInt32(reader["id_usuario_asignado"]) : -1; // Valor predeterminado ppr si es null
                            tarea.NombreUsuarioAsignado = reader["usuario_asignado"] != DBNull.Value ? reader["usuario_asignado"].ToString() : "Sin asignar"; // Valor predeterminado por si es null
                        }
                    }
                }
                catch {
                    throw;
                }
                finally
                {
                    connection.Close(); // Asegura que se cierre la conexión, independientemente de si hay una excepción o no
                }
            }
            if(tarea == null) throw new Exception("Tarea no encontrada");
            return tarea;
        }
        public List<Tarea> GetAllTareasDeTablero(int? idTablero)
        {
            var listaTareas = new List<Tarea>();
            using (SQLiteConnection connection = new SQLiteConnection(cadenaDeConexion))
            {
                try
                {
                    connection.Open();
                    SQLiteCommand commando = connection.CreateCommand();
                    commando.CommandText = "SELECT DISTINCT id_tarea, t.nombre, estado, t.descripcion, color, t.id_usuario_asignado, t.id_tablero, a.nombre_usuario AS usuario_asignado, p.nombre_usuario AS propietario, p.id AS id_propietario  "
                                         + "FROM Tarea t "
                                         + "INNER JOIN Tablero m ON m.id_tablero = t.id_tablero "
                                         + "INNER JOIN Usuario p ON p.id = m.id_usuario_propietario "
                                         + "LEFT JOIN Usuario a ON a.id = t.id_usuario_asignado " //LEFT JOIN ya que puede haber tareas sin usuario asignado (toma valor null) 
                                         + "WHERE t.id_tablero = @idTablero;";
                    commando.Parameters.Add(new SQLiteParameter("@idTablero", idTablero));
                    using(SQLiteDataReader reader = commando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var tarea = new Tarea
                            {
                                Id = Convert.ToInt32(reader["id_tarea"]),
                                IdTablero = Convert.ToInt32(reader["id_tablero"]),
                                Nombre = reader["nombre"].ToString(),
                                Descripcion = reader["descripcion"].ToString(),
                                Color = reader["color"].ToString(),
                                Estado = (EstadoTarea)Convert.ToInt32(reader["estado"]),
                                NombreUsuarioPropietario = reader["propietario"].ToString(),
                                IdPropietario = Convert.ToInt32(reader["id_propietario"]),
                                IdUsuarioAsignado = reader["id_usuario_asignado"] != DBNull.Value ? Convert.ToInt32(reader["id_usuario_asignado"]) : -1, // Valor predeterminado ppr si es null
                                NombreUsuarioAsignado = reader["usuario_asignado"] != DBNull.Value ? reader["usuario_asignado"].ToString() : "Sin asignar", // Valor predeterminado por si es null
                            };
                            listaTareas.Add(tarea);
                        }
                    }
                }
                catch {
                    throw;
                }
                finally
                {
                    connection.Close(); // Asegura que se cierre la conexión, independientemente de si hay una excepción o no
                }
                return listaTareas;
            }
        }

        //Tareas asignadas al usuario

        public List<Tarea> GetTareasAsignadasAUsuario(int idUsuario)
        {
            var listaTareas = new List<Tarea>();
            using (SQLiteConnection connection = new SQLiteConnection(cadenaDeConexion)){
                try
                {
                    connection.Open();
                    using (SQLiteCommand commando = connection.CreateCommand()) {
                        commando.CommandText = "SELECT id_tarea, t.nombre, estado, t.descripcion, color, id_usuario_asignado, t.id_tablero, a.nombre_usuario AS usuario_asignado, p.nombre_usuario AS propietario, p.id AS id_propietario "
                                            +  "FROM Tarea t "
                                            +  "INNER JOIN Usuario a ON t.id_usuario_asignado= a.id "
                                            +  "INNER JOIN Tablero m USING(id_tablero) "
                                            +  "INNER JOIN Usuario p ON m.id_usuario_propietario = p.id " //Aca no es necesario LEFT JOIN
                                            +  "WHERE a.id = @idUsuario;";
                        commando.Parameters.Add(new SQLiteParameter("@idUsuario", idUsuario));
                        using(SQLiteDataReader reader = commando.ExecuteReader())
                        {
                            while (reader.Read()) {
                                var tarea = new Tarea
                                {
                                    Id = Convert.ToInt32(reader["id_tarea"]),
                                    IdTablero = Convert.ToInt32(reader["id_tablero"]),
                                    Nombre = reader["nombre"].ToString(),
                                    Color = reader["color"].ToString(),
                                    Estado = (EstadoTarea)Convert.ToInt32(reader["estado"]),
                                    Descripcion = reader["descripcion"].ToString(),
                                    NombreUsuarioPropietario = reader["propietario"].ToString(),
                                    IdPropietario = Convert.ToInt32(reader["id_propietario"]),
                                    IdUsuarioAsignado = reader["id_usuario_asignado"] != DBNull.Value ? Convert.ToInt32(reader["id_usuario_asignado"]) : -1, // Valor predeterminado ppr si es null
                                    NombreUsuarioAsignado = reader["usuario_asignado"] != DBNull.Value ? reader["usuario_asignado"].ToString() : "Sin asignar", // Valor predeterminado por si es null
                                };
                                listaTareas.Add(tarea);
                            }
                        }
                    }
                }
                catch {
                    throw;
                }
                finally
                {
                    connection.Close(); // Asegura que se cierre la conexión, independientemente de si hay una excepción o no
                }
                return listaTareas;
            }
        }

        public Tarea ModificarTarea(Tarea tarea)
        {
            using (SQLiteConnection connection = new SQLiteConnection(cadenaDeConexion)){
                
                try
                {
                    connection.Open();
                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        command.CommandText = $"UPDATE Tarea SET nombre = @nombre, descripcion = @descripcion, estado = @estado, color = @color, id_tablero = @idTablero, id_usuario_asignado = @idUsuarioAsignado WHERE id_tarea = @idTarea;";
                        command.Parameters.Add(new SQLiteParameter("@idTarea",tarea.Id));
                        command.Parameters.Add(new SQLiteParameter("@nombre",tarea.Nombre));
                        command.Parameters.Add(new SQLiteParameter("@descripcion",tarea.Descripcion));
                        command.Parameters.Add(new SQLiteParameter("@estado",(int)tarea.Estado));
                        command.Parameters.Add(new SQLiteParameter("@color", tarea.Color));
                        command.Parameters.Add(new SQLiteParameter("@idTablero",tarea.IdTablero));
                        if(tarea.IdUsuarioAsignado != -1) {
                            command.Parameters.Add(new SQLiteParameter("@idUsuarioAsignado",tarea.IdUsuarioAsignado));
                        } else{
                            command.Parameters.Add(new SQLiteParameter("@idUsuarioAsignado", null));
                        }
                        command.ExecuteNonQuery();
                    }
                }
                catch {
                    throw;
                }
                finally{
                    connection.Close();
                } 
                return tarea;
            }
        }
        public int AsignarUsuarioATarea(int idUsuario, int idTarea)
        {
            int filasAfectadas = 0;
            using (SQLiteConnection connection = new SQLiteConnection(cadenaDeConexion))
            {
                try
                {
                    connection.Open();
                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        if (idUsuario != -1) {  
                            command.CommandText = $"UPDATE Tarea SET id_usuario_asignado = @idUsuario WHERE id_tarea = @idTarea;";
                            command.Parameters.Add(new SQLiteParameter("@idUsuario", idUsuario));
                        } else { //La tarea no se asigna a nadie
                            command.CommandText = $"UPDATE Tarea SET id_usuario_asignado = NULL WHERE id_tarea = @idTarea;";
                        }
                        command.Parameters.Add(new SQLiteParameter("@idTarea", idTarea));
                        filasAfectadas = command.ExecuteNonQuery();
                    }
                }
                catch {
                    throw;
                }
                finally{
                    connection.Close();
                } 
                return filasAfectadas;
            }
        }

        public List<Tarea> GetAllTareas()
        {
            var listaTareas = new List<Tarea>();
            using var connection = new SQLiteConnection(cadenaDeConexion);
            try
            {
                connection.Open();
                using var command = connection.CreateCommand();
                command.CommandText = "SELECT t.id_tarea, t.id_tablero, t.id_usuario_asignado, t.nombre, t.descripcion, t.color, t.estado, a.nombre_usuario AS usuario_asignado, p.nombre_usuario AS propietario,  p.id AS id_propietario FROM Tarea t "
                                    + "INNER JOIN Usuario p ON m.id_usuario_propietario = p.id "
                                    + "INNER JOIN Tablero m USING(id_tablero) "
                                    + "LEFT JOIN Usuario a ON a.id = t.id_usuario_asignado"; //LEFT JOIN ya que puede haber tareas sin usuario asignado (toma valor null) 
                command.ExecuteNonQuery();
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var tarea = new Tarea
                        {
                            Id = Convert.ToInt32(reader["id_tarea"]),
                            IdTablero = Convert.ToInt32(reader["id_tablero"]),
                            Nombre = reader["nombre"].ToString(),
                            Color = reader["color"].ToString(),
                            Estado = (EstadoTarea)Convert.ToInt32(reader["estado"]),
                            Descripcion = reader["descripcion"].ToString(),
                            NombreUsuarioPropietario = reader["propietario"].ToString(),
                            IdPropietario = Convert.ToInt32(reader["id_propietario"]),
                            IdUsuarioAsignado = reader["id_usuario_asignado"] != DBNull.Value ? Convert.ToInt32(reader["id_usuario_asignado"]) : -1, // Valor predeterminado ppr si es null
                            NombreUsuarioAsignado = reader["usuario_asignado"] != DBNull.Value ? reader["usuario_asignado"].ToString() : "Sin asignar", // Valor predeterminado por si es null
                        };
                        listaTareas.Add(tarea);
                    }
                }

            }
            catch {
                throw;
            }
            finally
            {
                connection.Close();
            }
            return listaTareas;
        }

        //Tareas pertenecientes a sus tableros (que pueden estar asignadas a otro usuario)
        public List<Tarea> GetTareasFromTablerosDelUsuario(int idUsuario)
        {
            var listaTareas = new List<Tarea>();
            using var connection = new SQLiteConnection(cadenaDeConexion);
            try
            {
                connection.Open();
                using var command = connection.CreateCommand();
                command.CommandText = "SELECT t.id_tarea, t.id_tablero, t.id_usuario_asignado, t.nombre, t.descripcion, t.color, t.estado, a.nombre_usuario AS usuario_asignado, p.nombre_usuario AS propietario,  p.id AS id_propietario " 
                                    + "FROM Tarea t "
                                    + "INNER JOIN Tablero m USING(id_tablero) "
                                    + "INNER JOIN Usuario p ON m.id_usuario_propietario = p.id "
                                    + "LEFT JOIN Usuario a ON a.id = t.id_usuario_asignado "//LEFT JOIN ya que puede haber tareas sin usuario asignado (toma valor null) 
                                    + "WHERE m.id_usuario_propietario = @idUsuario;";
                command.Parameters.Add(new SQLiteParameter("@idUsuario", idUsuario));
                command.ExecuteNonQuery();
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var tarea = new Tarea
                        {
                            Id = Convert.ToInt32(reader["id_tarea"]),
                            IdTablero = Convert.ToInt32(reader["id_tablero"]),
                            Nombre = reader["nombre"].ToString(),
                            Color = reader["color"].ToString(),
                            Estado = (EstadoTarea)Convert.ToInt32(reader["estado"]),
                            Descripcion = reader["descripcion"].ToString(),
                            NombreUsuarioPropietario = reader["propietario"].ToString(),
                            IdPropietario = Convert.ToInt32(reader["id_propietario"]),
                            IdUsuarioAsignado = reader["id_usuario_asignado"] != DBNull.Value ? Convert.ToInt32(reader["id_usuario_asignado"]) : -1, // Valor predeterminado para IdUsuarioAsignado si es null
                            NombreUsuarioAsignado = reader["usuario_asignado"] != DBNull.Value ? reader["usuario_asignado"].ToString() : "Sin asignar", // Valor predeterminado para NombreUsuarioAsignado si es null
                        };
                        listaTareas.Add(tarea);
                    }
                }

            }
            catch {
                throw;
            }
            finally
            {
                connection.Close();
            }
            return listaTareas;
        }
    }
}