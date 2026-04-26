using Monitor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitor.Repositories
{
    public interface IConfiguracionAppRepository
    {

        ConfiguracionApp ObtenerConfiguracion();

        void ActualizarConfiguracion(ConfiguracionApp config);





    }


}
