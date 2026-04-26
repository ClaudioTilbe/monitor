using Monitor.Data;
using Monitor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitor.Repositories
{
    public interface IAccesoVNCRepository  
    {

        void Insertar(AccesoVNC acceso);

        void Eliminar(int modulo, int fila, int columna);

        List<AccesoVNC> ListadoPorModulo(int modulo);




    }
}
