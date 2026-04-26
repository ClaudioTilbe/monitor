using Monitor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitor.Repositories
{
    public interface IDispositivoTituloRepository
    {

        void Insertar(DispositivoTitulo titulo);

        void Eliminar(int modulo, int fila, int columna);

        List<DispositivoTitulo> ListadoPorModulo(int modulo);



    }

}
