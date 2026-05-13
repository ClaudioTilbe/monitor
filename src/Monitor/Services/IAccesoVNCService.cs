using Monitor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitor.Services
{
    public interface IAccesoVNCService
    {

        void Agregar(AccesoVNC unAcceso);

        void Eliminar(int modulo, int fila, int columna);

        List<AccesoVNC> Listado(int modulo);



    }

}
