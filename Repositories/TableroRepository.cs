using System.Data.SqlClient;
using System.Data.SQLite;
using EspacioTareas;
namespace EspacioRepositorios
{
    public class TableroRepository : ITableroRepository
    {
        private readonly string cadenaConexion = "Data Source=DB/kanban.db;Cache=Shared";

        public Tablero CrearTablero(Tablero T)
        {
            using SQLiteConnection connection = new SQLiteConnection(cadenaConexion);
            try
            {
                connection.Open(); // va antes de crear el command
                using SQLiteCommand command = connection.CreateCommand();
                command.CommandText = "INSERT INTO Tablero (nombre, descripcion, id_usuario_propietario) VALUES (@nombre, @descripcion, @idUsuarioPropietario);";
                command.Parameters.Add(new SQLiteParameter("nombre", T.Nombre));
                command.Parameters.Add(new SQLiteParameter("descripcion", T.Descripcion));
                command.Parameters.Add(new SQLiteParameter("idUsuarioPropietario", T.IdUsuarioPropietario));
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ha ocurrido un error: {ex.Message}");
            }
            finally
            {
                connection.Close();
            }
            return T;
        }

        public int EliminarTablero(int idTablero)
        {
            int filasAfectadas = 0;
            using var connection = new SQLiteConnection(cadenaConexion);
            try
            {
                connection.Open();
                using var command = connection.CreateCommand();
                command.CommandText = "DELETE FROM Tablero WHERE id_tablero = @idTablero;";
                command.Parameters.Add(new SQLiteParameter("idTablero", idTablero));
                filasAfectadas = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ha ocurrido un error: {ex.Message}");
            }
            finally
            {
                connection.Close();
            }
            return filasAfectadas;
        }

        public List<Tablero> GetAllTableros()
        {
            var listaTableros = new List<Tablero>();
            using var connection = new SQLiteConnection(cadenaConexion);
            try
            {
                connection.Open();
                using var command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM Tablero";
                command.ExecuteNonQuery();
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var tablero = new Tablero
                        {
                            Nombre = reader["nombre"].ToString(),
                            Descripcion = reader["descripcion"].ToString(),
                            IdUsuarioPropietario = Convert.ToInt32(reader["id_usuario_propietario"]),
                            Id = Convert.ToInt32(reader["id_tablero"])
                        };
                        listaTableros.Add(tablero);
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ha ocurrido un error: {ex.Message}");
            }
            finally
            {
                connection.Close();
            }
            return listaTableros;
        }

        public List<Tablero> GetAllTablerosDeUsuario(int idUsuario)
        {
            var listaTableros = new List<Tablero>();
            using var connection = new SQLiteConnection(cadenaConexion);
            try
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM Tablero WHERE id_usuario_propietario = @idUsuario;";
                    command.Parameters.Add(new SQLiteParameter("idUsuario", idUsuario));
                    command.ExecuteNonQuery();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var tablero = new Tablero
                            {
                                Nombre = reader["nombre"].ToString(),
                                Descripcion = reader["descripcion"].ToString(),
                                Id = Convert.ToInt32(reader["id_tablero"]),
                                IdUsuarioPropietario = Convert.ToInt32(reader["id_usuario_propietario"])
                            };
                            listaTableros.Add(tablero);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ha ocurrido un error: {ex.Message}");
            }
            finally
            {
                connection.Close();
            }
            return listaTableros;
        }

        public Tablero GetTableroById(int idTablero)
        {
            var tablero = new Tablero();
            using var connection = new SQLiteConnection(cadenaConexion);
            try
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM Tablero WHERE id_tablero = @idTablero;";
                    command.Parameters.Add(new SQLiteParameter("idTablero", idTablero));
                    command.ExecuteNonQuery();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            tablero = new Tablero
                            {
                                Nombre = reader["nombre"].ToString(),
                                Descripcion = reader["descripcion"].ToString(),
                                Id = Convert.ToInt32(reader["id_tablero"]),
                                IdUsuarioPropietario = Convert.ToInt32(reader["id_usuario_propietario"])
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ha ocurrido un error: {ex.Message}");
            }
            finally
            {
                connection.Close();
            }
            return tablero;
        }

        public Tablero ModificarTablero(int idTablero, Tablero T)
        {
           using(var connection = new SQLiteConnection(cadenaConexion))
           {
                try{
                    connection.Open();
                    using(var command = connection.CreateCommand())
                    {
                        command.CommandText = "UPDATE Tablero SET nombre = @nombre, descripcion = @descripcion, id_usuario_propietario = @idUsuarioPropietario WHERE id_tablero = @idTablero ";
                        command.Parameters.Add(new SQLiteParameter("nombre",T.Nombre));
                        command.Parameters.Add(new SQLiteParameter("descripcion",T.Descripcion));
                        command.Parameters.Add(new SQLiteParameter("idUsuarioPropietario",T.IdUsuarioPropietario));
                        command.Parameters.Add(new SQLiteParameter("idTablero",T.Id));
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex){
                    Console.WriteLine($"Ha ocurrido un error: {ex.Message}");
                }
                finally
                {
                    connection.Close();
                }
                return T;
           }
        }
    }

}