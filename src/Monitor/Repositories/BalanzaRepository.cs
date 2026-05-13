using Monitor.Data;
using Monitor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitor.Repositories
{
    public class BalanzaRepository : IBalanzaRepository
    {


        private readonly DatabaseContext _context;

        public BalanzaRepository(DatabaseContext context)
        {
            _context = context;
        }




        public void Insertar(Balanza balanza)
        {
            using var connection = _context.GetConnection();
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = @"
                INSERT INTO Balanzas (NumeroBalanza, IP, Alias, Fila, Columna)
                VALUES ($numero, $ip, $alias, $fila, $columna);
            ";

            command.Parameters.AddWithValue("$numero", balanza.NumeroBalanza);
            command.Parameters.AddWithValue("$ip", balanza.IP);
            command.Parameters.AddWithValue("$alias", balanza.Alias);
            command.Parameters.AddWithValue("$fila", balanza.Fila);
            command.Parameters.AddWithValue("$columna", balanza.Columna);

            command.ExecuteNonQuery();
        }



        public void EliminarPorPosicion(int fila, int columna)
        {
            using var connection = _context.GetConnection();
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = @"
                DELETE FROM Balanzas
                WHERE Fila = $fila AND Columna = $columna;
            ";

            command.Parameters.AddWithValue("$fila", fila);
            command.Parameters.AddWithValue("$columna", columna);

            command.ExecuteNonQuery();
        }



        public List<Balanza> Listado()
        {
            var lista = new List<Balanza>();

            using var connection = _context.GetConnection();
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = @"
                SELECT NumeroBalanza, IP, Alias, Fila, Columna
                FROM Balanzas;
            ";

            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                var balanza = new Balanza
                {
                    NumeroBalanza = reader.GetInt32(0),
                    IP = reader.GetString(1),
                    Alias = reader.GetString(2),
                    Fila = reader.GetInt32(3),
                    Columna = reader.GetInt32(4)
                };

                lista.Add(balanza);
            }

            return lista;
        }






    }


}
