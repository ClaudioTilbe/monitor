using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Monitor.Models
{
    public class Balanza : INotifyPropertyChanged
    {

        public int NumeroBalanza { get; set; }
        public string IP { get; set; }
        public string Alias { get; set; }



        public int Fila { get; set; }

        public int Columna { get; set; }




        private bool _estaOnline;
        public bool EstaOnline
        {
            get => _estaOnline;
            set
            {
                _estaOnline = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));


    }
}
