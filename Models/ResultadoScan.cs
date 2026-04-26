using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitor.Models
{
    public class ResultadoScan
    {
        public string IP { get; set; }
        public string MAC { get; set; }
        public string Hostname { get; set; }
        public string SistemaOperativo { get; set; }
        public string Comando { get; set; }

        public List<PuertoInfo> Puertos { get; set; } = new List<PuertoInfo>();
    }




}
