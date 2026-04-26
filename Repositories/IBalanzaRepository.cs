using Monitor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitor.Repositories
{
    public interface IBalanzaRepository
    {

        void Insertar(Balanza balanza);

        void EliminarPorPosicion(int fila, int columna);

        List<Balanza> Listado();



    }


}
