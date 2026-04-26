using Monitor.Models;
using Monitor.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitor.Services
{
    public class AccesoVNCService : IAccesoVNCService
    {


        private readonly IAccesoVNCRepository _repo;

        public AccesoVNCService(IAccesoVNCRepository repo)
        {
            _repo = repo;
        }



        public void Agregar(AccesoVNC unAcceso)
        {
             _repo.Insertar(unAcceso);
        }


        public void Eliminar(int modulo, int fila, int columna)
        {
          
            _repo.Eliminar(modulo, fila, columna);
        }


        public List<AccesoVNC> Listado(int modulo)
        {
            return _repo.ListadoPorModulo(modulo);
        }



    }


}
