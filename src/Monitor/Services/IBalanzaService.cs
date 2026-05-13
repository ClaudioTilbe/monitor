using Monitor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitor.Services
{
    public interface IBalanzaService
    {

        void Agregar(Balanza unaBalanza);

        void Eliminar(int fila, int columna);

        List<Balanza> Listado();

        bool ExisteNumero(int numero);

    }

}
