using tp10.Models;

namespace EspacioRepositorios
{
    interface ITableroRepository
    {
        Tablero CrearTablero(Tablero T);
        Tablero ModificarTablero(Tablero T);
        Tablero GetTableroById(int idTablero);
        List<Tablero> GetAllTableros();
        List<Tablero >GetAllTablerosDeUsuario(int idUsuario);
        int EliminarTablero(int idTablero);
    }
}