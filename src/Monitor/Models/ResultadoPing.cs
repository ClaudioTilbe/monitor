using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitor.Models
{
    public class ResultadoPing
    {
        public string IP { get; set; }
        public bool Activo { get; set; }
        public long TiempoRespuesta { get; set; }
    }


}
