using Monitor.Models;
using Monitor.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitor.Services
{
    public class ConfiguracionAppService : IConfiguracionAppService
    {

        private readonly IConfiguracionAppRepository _repo;

        public ConfiguracionAppService(IConfiguracionAppRepository repo)
        {
            _repo = repo;
        }



        public ConfiguracionApp ObtenerConfiguracion()
        {
             return _repo.ObtenerConfiguracion();
        }


        public void ActualizarConfiguracion(ConfiguracionApp config)
        {

            _repo.ActualizarConfiguracion(config);
        }



    }


}
