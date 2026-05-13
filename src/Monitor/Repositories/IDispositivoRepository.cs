using Monitor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitor.Repositories
{
    public interface IDispositivoRepository
    {

        void Insertar(Dispositivo dispositivo);

        void Eliminar(int modulo, int fila, int columna);

        List<Dispositivo> ListadoPorModulo(int modulo);




    }


}
