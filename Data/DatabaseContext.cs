using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Data.Sqlite;
using System.IO;




namespace Monitor.Data
{
    public class DatabaseContext
    {

        private string connectionString;

        public DatabaseContext()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;

            string dbFolder = Path.Combine(baseDir, "Database");

            // Crea carpeta si no existe
            if (!Directory.Exists(dbFolder))
                Directory.CreateDirectory(dbFolder);

            string dbPath = Path.Combine(dbFolder, "monitorBD.db");

            connectionString = $"Data Source={dbPath}";
        }

        public SqliteConnection GetConnection()
        {
            return new SqliteConnection(connectionString);
        }


    }

}
