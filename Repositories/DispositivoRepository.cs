using Monitor.Data;
using Monitor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitor.Repositories
{
    public class DispositivoRepository : IDispositivoRepository
    {

        private readonly DatabaseContext _context;

        public DispositivoRepository(DatabaseContext context)
        {
            _context = context;
        }




        public void Insertar(Dispositivo dispositivo)
        {
            using var connection = _context.GetConnection();
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = @"
            INSERT INTO Dispositivo (IP, Alias, Modulo, Fila, Columna)
            VALUES ($ip, $alias, $modulo, $fila, $columna);
        ";

            command.Parameters.AddWithValue("$ip", dispositivo.IP);
            command.Parameters.AddWithValue("$alias", dispositivo.Alias);
            command.Parameters.AddWithValue("$modulo", dispositivo.Modulo);
            command.Parameters.AddWithValue("$fila", dispositivo.Fila);
            command.Parameters.AddWithValue("$columna", dispositivo.Columna);

            command.ExecuteNonQuery();
        }

        public void Eliminar(int modulo, int fila, int columna)
        {
            using var connection = _context.GetConnection();
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = @"
                                    DELETE FROM Dispositivo
                                    WHERE Modulo = $modulo
                                    AND Fila = $fila
                                    AND Columna = $columna;
                                ";

            command.Parameters.AddWithValue("$modulo", modulo);
            command.Parameters.AddWithValue("$fila", fila);
            command.Parameters.AddWithValue("$columna", columna);

            command.ExecuteNonQuery();
        }



        public List<Dispositivo> ListadoPorModulo(int modulo)
        {
            var lista = new List<Dispositivo>();

            using var connection = _context.GetConnection();
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = @"
                                    SELECT IP, Alias, Modulo, Fila, Columna
                                    FROM Dispositivo
                                    WHERE Modulo = $modulo;
                                ";

            command.Parameters.AddWithValue("$modulo", modulo);

            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                var dispositivo = new Dispositivo
                {
                    IP = reader.GetString(0),
                    Alias = reader.GetString(1),
                    Modulo = reader.GetInt32(2),
                    Fila = reader.GetInt32(3),
                    Columna = reader.GetInt32(4)
                };

                lista.Add(dispositivo);
            }

            return lista;
        }



    }


}
