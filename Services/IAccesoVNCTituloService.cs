using Monitor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitor.Services
{
    public interface IAccesoVNCTituloService
    {

        void Agregar(AccesoVNCTitulo unAcceso);

        void Eliminar(int modulo, int fila, int columna);

        List<AccesoVNCTitulo> Listado(int modulo);



    }

}
