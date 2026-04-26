using Monitor.Data;
using Monitor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitor.Repositories
{
    public class AccesoVNCRepository : IAccesoVNCRepository
    {
        private readonly DatabaseContext _context;

        public AccesoVNCRepository(DatabaseContext context)
        {
            _context = context;
        }


        public void Insertar(AccesoVNC acceso)
        {
            using var connection = _context.GetConnection();
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = @"
                                    INSERT INTO AccesoVNC (IP, Alias, Modulo, Fila, Columna)
                                    VALUES ($ip, $alias, $modulo, $fila, $columna);
                                ";

            command.Parameters.AddWithValue("$ip", acceso.IP);
            command.Parameters.AddWithValue("$alias", acceso.Alias);
            command.Parameters.AddWithValue("$modulo", acceso.Modulo);
            command.Parameters.AddWithValue("$fila", acceso.Fila);
            command.Parameters.AddWithValue("$columna", acceso.Columna);

            command.ExecuteNonQuery();
        }

        public void Eliminar(int modulo, int fila, int columna)
        {
            using var connection = _context.GetConnection();
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = @"
                                    DELETE FROM AccesoVNC
                                    WHERE Modulo = $modulo
                                    AND Fila = $fila
                                    AND Columna = $columna;
                                ";

            command.Parameters.AddWithValue("$modulo", modulo);
            command.Parameters.AddWithValue("$fila", fila);
            command.Parameters.AddWithValue("$columna", columna);

            command.ExecuteNonQuery();
        }



        public List<AccesoVNC> ListadoPorModulo(int modulo)
        {
            var lista = new List<AccesoVNC>();

            using var connection = _context.GetConnection();
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = @"
                                    SELECT IP, Alias, Modulo, Fila, Columna
                                    FROM AccesoVNC
                                    WHERE Modulo = $modulo;
                                ";

            command.Parameters.AddWithValue("$modulo", modulo);

            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                var acceso = new AccesoVNC
                {
                    IP = reader.GetString(0),
                    Alias = reader.GetString(1),
                    Modulo = reader.GetInt32(2),
                    Fila = reader.GetInt32(3),
                    Columna = reader.GetInt32(4)
                };

                lista.Add(acceso);
            }

            return lista;
        }




    }


}
