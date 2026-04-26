using Monitor.Models;
using Monitor.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitor.Services
{
    public class DispositivoService : IDispositivoService
    {


        private readonly IDispositivoRepository _repo;

        public DispositivoService(IDispositivoRepository repo)
        {
            _repo = repo;
        }



        public void Agregar(Dispositivo unDispositivo)
        {
            _repo.Insertar(unDispositivo);
        }


        public void Eliminar(int modulo, int fila, int columna)
        {

            _repo.Eliminar(modulo, fila, columna);
        }


        public List<Dispositivo> Listado(int modulo)
        {
            return _repo.ListadoPorModulo(modulo);
        }


    }


}
