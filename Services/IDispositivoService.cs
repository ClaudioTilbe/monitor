using Monitor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitor.Services
{
    public interface IDispositivoService
    {

        void Agregar(Dispositivo unDispositivo);

        void Eliminar(int modulo, int fila, int columna);

        List<Dispositivo> Listado(int modulo);



    }
}
