using Monitor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitor.Services
{
    public interface IDispositivoTituloService
    {

        void Agregar(DispositivoTitulo unDispositivoT);

        void Eliminar(int modulo, int fila, int columna);

        List<DispositivoTitulo> Listado(int modulo);



    }

}
