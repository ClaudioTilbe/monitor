using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitor.Data
{
    public class DbInitializer
    {

        public static void Initialize(DatabaseContext db)
        {
            using var connection = db.GetConnection();
            connection.Open();

            var command = connection.CreateCommand();

            command.CommandText = @"

            -- ===============================
            -- AccesoVNC
            -- ===============================
            CREATE TABLE IF NOT EXISTS AccesoVNC (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                IP TEXT NOT NULL,
                Alias TEXT NOT NULL,
                Modulo INTEGER NOT NULL,
                Fila INTEGER NOT NULL,
                Columna INTEGER NOT NULL
            );

            CREATE UNIQUE INDEX IF NOT EXISTS idx_accesovnc_posicion 
            ON AccesoVNC (Modulo, Fila, Columna);


            -- ===============================
            -- AccesoVNCTitulo
            -- ===============================
            CREATE TABLE IF NOT EXISTS AccesoVNCTitulo (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Texto TEXT NOT NULL,
                Modulo INTEGER NOT NULL,
                Fila INTEGER NOT NULL,
                Columna INTEGER NOT NULL
            );

            CREATE UNIQUE INDEX IF NOT EXISTS idx_accesovnc_titulo 
            ON AccesoVNCTitulo (Modulo, Fila, Columna);


            -- ===============================
            -- Balanzas
            -- ===============================
            CREATE TABLE IF NOT EXISTS Balanzas (
                NumeroBalanza INTEGER PRIMARY KEY,
                IP TEXT NOT NULL UNIQUE,
                Alias TEXT NOT NULL,
                Fila INTEGER NOT NULL,
                Columna INTEGER NOT NULL
            );


            -- ===============================
            -- Dispositivo
            -- ===============================
            CREATE TABLE IF NOT EXISTS Dispositivo (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                IP TEXT NOT NULL,
                Alias TEXT NOT NULL,
                Modulo INTEGER NOT NULL,
                Fila INTEGER NOT NULL,
                Columna INTEGER NOT NULL
            );

            CREATE UNIQUE INDEX IF NOT EXISTS idx_dispositivo_posicion 
            ON Dispositivo (Modulo, Fila, Columna);


            -- ===============================
            -- DispositivoTitulo
            -- ===============================
            CREATE TABLE IF NOT EXISTS DispositivoTitulo (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Texto TEXT NOT NULL,
                Modulo INTEGER NOT NULL,
                Fila INTEGER NOT NULL,
                Columna INTEGER NOT NULL
            );

            CREATE UNIQUE INDEX IF NOT EXISTS idx_dispositivo_titulo_posicion 
            ON DispositivoTitulo (Modulo, Fila, Columna);

            -- ===============================
            -- Configuración App
            -- ===============================
            CREATE TABLE IF NOT EXISTS ConfiguracionApp (
                Id INTEGER PRIMARY KEY CHECK (Id = 1),
                Gateway TEXT NOT NULL,
                RutaNmap TEXT NOT NULL,
                RutaVNC TEXT NOT NULL
            );



             -- 🔥 INSERT DEFAULT (SOLO SI NO EXISTE)
            INSERT OR IGNORE INTO ConfiguracionApp (Id, Gateway, RutaNmap)
            VALUES (1, '192.168.1.1', 'C:\Program Files (x86)\Nmap\Nmap.exe');

            ";

            try
            {
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Error al inicializar la base de datos:\n" + ex.Message);
                throw;
            }
        }

    }
}
