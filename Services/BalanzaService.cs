using Monitor.Models;
using Monitor.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitor.Services
{
    public class BalanzaService : IBalanzaService
    {

        private readonly IBalanzaRepository _repo;

        public BalanzaService(IBalanzaRepository repo)
        {
            _repo = repo;
        }



        public void Agregar(Balanza unaBalanza)
        {
            _repo.Insertar(unaBalanza);
        }


        public void Eliminar(int fila, int columna)
        {

            _repo.EliminarPorPosicion(fila, columna);
        }


        public List<Balanza> Listado()
        {
            return _repo.Listado();
        }


        public bool ExisteNumero(int numero)
        {
            return _repo.Listado()
                .Any(x => x.NumeroBalanza == numero);
        }

    }


}
