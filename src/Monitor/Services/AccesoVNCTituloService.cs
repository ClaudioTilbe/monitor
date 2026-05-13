using Monitor.Models;
using Monitor.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitor.Services
{
    public class AccesoVNCTituloService : IAccesoVNCTituloService
    {

        private readonly IAccesoVNCTituloRepository _repo;

        public AccesoVNCTituloService(IAccesoVNCTituloRepository repo)
        {
            _repo = repo;
        }



        public void Agregar(AccesoVNCTitulo unAcceso)
        {
            _repo.Insertar(unAcceso);
        }


        public void Eliminar(int modulo, int fila, int columna)
        {

            _repo.Eliminar(modulo, fila, columna);
        }


        public List<AccesoVNCTitulo> Listado(int modulo)
        {
            return _repo.ListadoPorModulo(modulo);
        }

    }


}
