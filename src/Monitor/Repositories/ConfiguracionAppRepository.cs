using Monitor.Data;
using Monitor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitor.Repositories
{
    public class ConfiguracionAppRepository : IConfiguracionAppRepository
    {

        private readonly DatabaseContext _context;

        public ConfiguracionAppRepository(DatabaseContext context)
        {
            _context = context;
        }




        public ConfiguracionApp ObtenerConfiguracion()
        {
            using var connection = _context.GetConnection();
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = "SELECT Gateway, RutaNmap, RutaVNC FROM ConfiguracionApp WHERE Id = 1";

            using var reader = command.ExecuteReader();

            if (reader.Read())
            {
                return new ConfiguracionApp
                {
                    Gateway = reader.GetString(0),
                    RutaNmap = reader.GetString(1),
                    RutaVNC = reader.GetString(2)
                };
            }

            return null;
        }



        public void ActualizarConfiguracion(ConfiguracionApp config)
        {
            using var connection = _context.GetConnection();
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = @"
                                    INSERT INTO ConfiguracionApp (Id, Gateway, RutaNmap, RutaVNC)
                                    VALUES (1, @gateway, @rutaNmap, @rutaVNC)
                                    ON CONFLICT(Id) DO UPDATE SET
                                        Gateway = excluded.Gateway,
                                        RutaNmap = excluded.RutaNmap,
                                        RutaVNC = excluded.RutaVNC
                                ";

            command.Parameters.AddWithValue("@gateway", config.Gateway);
            command.Parameters.AddWithValue("@rutaNmap", config.RutaNmap);
            command.Parameters.AddWithValue("@rutaVNC", config.RutaVNC);

            command.ExecuteNonQuery();
        }

    }



}
