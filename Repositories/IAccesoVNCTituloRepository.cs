using Monitor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitor.Repositories
{
    public interface IAccesoVNCTituloRepository
    {

        void Insertar(AccesoVNCTitulo titulo);

        void Eliminar(int modulo, int fila, int columna);

        List<AccesoVNCTitulo> ListadoPorModulo(int modulo);



    }


}
