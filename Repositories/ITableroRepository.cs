using EspacioTareas;

namespace EspacioRepositorios
{
    interface ITableroRepository
    {
        Tablero CrearTablero(Tablero T);
        Tablero ModificarTablero(int idTablero, Tablero T);
        Tablero GetTableroById(int idTablero);
        List<Tablero> GetAllTableros();
        List<Tablero >GetAllTablerosDeUsuario(int idUsuario);
        int EliminarTablero(int idTablero);
    }
}