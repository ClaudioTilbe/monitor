using Monitor.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace Monitor.ViewModels.Dialogs
{
    public class AgregarBalanzaViewModel : INotifyPropertyChanged
    {
        // =========================================================
        // 🔹 INotifyPropertyChanged
        // =========================================================

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));



        // =========================================================
        // 🔹 CAMPOS PRIVADOS
        // =========================================================

        private string _numeroBalanza;
        private string _ip;
        private string _alias;

        private string _mensaje = "Ingresa los campos";
        private Brush _mensajeColor = Brushes.White;



        // =========================================================
        // 🔹 PROPIEDADES (BINDING)
        // =========================================================

        // 🔹 DATOS
        public string NumeroBalanza
        {
            get => _numeroBalanza;
            set
            {
                _numeroBalanza = value;
                OnPropertyChanged();
            }
        }

        public string IP
        {
            get => _ip;
            set
            {
                _ip = value;
                OnPropertyChanged();
            }
        }

        public string Alias
        {
            get => _alias;
            set
            {
                _alias = value;
                OnPropertyChanged();
            }
        }

        // 🔹 MENSAJE
        public string Mensaje
        {
            get => _mensaje;
            set
            {
                _mensaje = value;
                OnPropertyChanged();
            }
        }

        public Brush MensajeColor
        {
            get => _mensajeColor;
            set
            {
                _mensajeColor = value;
                OnPropertyChanged();
            }
        }

        // 🔹 CONTROL
        public bool Aceptado { get; private set; }

        public Action<bool?> CloseAction { get; set; }



        // =========================================================
        // 🔹 COMANDOS
        // =========================================================

        public ICommand AceptarCommand { get; }
        public ICommand CancelarCommand { get; }



        // =========================================================
        // 🔹 CONSTRUCTOR
        // =========================================================

        public AgregarBalanzaViewModel()
        {
            AceptarCommand = new RelayCommand(_ => Aceptar());
            CancelarCommand = new RelayCommand(_ => Cancelar());
        }



        // =========================================================
        // 🔹 ACCIONES
        // =========================================================

        private void Aceptar()
        {
            if (!ValidarDatos(out string error))
            {
                Mensaje = error;
                MensajeColor = Brushes.IndianRed;
                return;
            }

            Aceptado = true;
            CloseAction?.Invoke(true);
        }

        private void Cancelar()
        {
            Aceptado = false;
            CloseAction?.Invoke(false);
        }



        // =========================================================
        // 🔹 VALIDACIONES
        // =========================================================

        private bool ValidarDatos(out string mensajeError)
        {
            mensajeError = "";

            if (!System.Net.IPAddress.TryParse(IP, out _))
            {
                mensajeError = "La dirección IP no es válida";
                return false;
            }

            if (string.IsNullOrWhiteSpace(Alias))
            {
                mensajeError = "El alias es obligatorio";
                return false;
            }

            if (Alias.Length > 50)
            {
                mensajeError = "Máx 50 caracteres";
                return false;
            }

            // 🔹 VALIDACIÓN NÚMERO BALANZA
            if (!int.TryParse(NumeroBalanza, out int numero))
            {
                mensajeError = "El número de balanza debe ser numérico";
                return false;
            }

            if (numero < 0 || numero >= 1000)
            {
                mensajeError = "El número de balanza debe estar entre 0 y 999";
                return false;
            }

            return true;
        }
    }


}
