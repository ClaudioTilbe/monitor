using Monitor.Data;
using Monitor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitor.Repositories
{
    public class DispositivoTituloRepository : IDispositivoTituloRepository
    {

        private readonly DatabaseContext _context;

        public DispositivoTituloRepository(DatabaseContext context)
        {
            _context = context;
        }



        public void Insertar(DispositivoTitulo titulo)
        {
            using var connection = _context.GetConnection();
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = @"
                                   INSERT INTO DispositivoTitulo (Texto, Modulo, Fila, Columna)
                                   VALUES ($texto, $modulo, $fila, $columna)
                                   ON CONFLICT(Modulo, Fila, Columna)
                                   DO UPDATE SET Texto = excluded.Texto;
                                   ";

            command.Parameters.AddWithValue("$texto", titulo.Texto);
            command.Parameters.AddWithValue("$modulo", titulo.Modulo);
            command.Parameters.AddWithValue("$fila", titulo.Fila);
            command.Parameters.AddWithValue("$columna", titulo.Columna);

            command.ExecuteNonQuery();
        }



        public void Eliminar(int modulo, int fila, int columna)
        {
            using var connection = _context.GetConnection();
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = @"
                                    DELETE FROM DispositivoTitulo
                                    WHERE Modulo = $modulo
                                    AND Fila = $fila
                                    AND Columna = $columna;
                                ";

            command.Parameters.AddWithValue("$modulo", modulo);
            command.Parameters.AddWithValue("$fila", fila);
            command.Parameters.AddWithValue("$columna", columna);

            command.ExecuteNonQuery();
        }



        public List<DispositivoTitulo> ListadoPorModulo(int modulo)
        {
            var lista = new List<DispositivoTitulo>();

            using var connection = _context.GetConnection();
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = @"
                                    SELECT Texto, Modulo, Fila, Columna
                                    FROM DispositivoTitulo
                                    WHERE Modulo = $modulo;
                                ";

            command.Parameters.AddWithValue("$modulo", modulo);

            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                var titulo = new DispositivoTitulo
                {
                    Texto = reader.GetString(0),
                    Modulo = reader.GetInt32(1),
                    Fila = reader.GetInt32(2),
                    Columna = reader.GetInt32(3)
                };

                lista.Add(titulo);
            }

            return lista;
        }





    }


}
