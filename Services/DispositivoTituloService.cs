using Monitor.Models;
using Monitor.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitor.Services
{
    public class DispositivoTituloService : IDispositivoTituloService
    {

        private readonly IDispositivoTituloRepository _repo;

        public DispositivoTituloService(IDispositivoTituloRepository repo)
        {
            _repo = repo;
        }



        public void Agregar(DispositivoTitulo unDispositivoT)
        {
            _repo.Insertar(unDispositivoT);
        }


        public void Eliminar(int modulo, int fila, int columna)
        {

            _repo.Eliminar(modulo, fila, columna);
        }


        public List<DispositivoTitulo> Listado(int modulo)
        {
            return _repo.ListadoPorModulo(modulo);
        }

    }


}
