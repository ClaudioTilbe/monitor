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
    public class AgregarDispositivoTituloViewModel : INotifyPropertyChanged
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

        private string _texto;

        private string _mensaje = "Ingresa el título";
        private Brush _mensajeColor = Brushes.White;



        // =========================================================
        // 🔹 PROPIEDADES (BINDING)
        // =========================================================

        // 🔹 DATO PRINCIPAL
        public string Texto
        {
            get => _texto;
            set
            {
                _texto = value;
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

        public AgregarDispositivoTituloViewModel()
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

            if (string.IsNullOrWhiteSpace(Texto))
            {
                mensajeError = "El título es obligatorio";
                return false;
            }

            if (Texto.Length > 100)
            {
                mensajeError = "Máx 100 caracteres";
                return false;
            }

            return true;
        }
    }

}
