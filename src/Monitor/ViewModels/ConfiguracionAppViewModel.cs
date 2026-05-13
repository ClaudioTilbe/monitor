using Monitor.Commands;
using Monitor.Models;
using Monitor.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace Monitor.ViewModels
{
    public class ConfiguracionAppViewModel : INotifyPropertyChanged
    {

        // 🔹 INotify
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));



        private readonly IConfiguracionAppService _configService;

        public ConfiguracionAppViewModel(IConfiguracionAppService configService)
        {
            _configService = configService;

            GuardarCommand = new RelayCommand(_ => Guardar());

            CargarConfiguracion();

            MostrarMensaje("¡Bienvenido a Monitor!", "info");
        }



        // 🔹 PROPIEDADES
        private string _gateway;
        public string Gateway
        {
            get => _gateway;
            set { _gateway = value; OnPropertyChanged(); }
        }

        private string _rutaNmap;
        public string RutaNmap
        {
            get => _rutaNmap;
            set { _rutaNmap = value; OnPropertyChanged(); }
        }

        private string _rutaVNC;
        public string RutaVNC
        {
            get => _rutaVNC;
            set { _rutaVNC = value; OnPropertyChanged(); }
        }


        private string _mensaje;
        public string Mensaje
        {
            get => _mensaje;
            set { _mensaje = value; OnPropertyChanged(); }
        }

        private Brush _mensajeColor = Brushes.White;
        public Brush MensajeColor
        {
            get => _mensajeColor;
            set { _mensajeColor = value; OnPropertyChanged(); }
        }



        // 🔹 COMMAND
        public ICommand GuardarCommand { get; }




        // 🔹 MÉTODOS
        private void CargarConfiguracion()
        {
            var config = _configService.ObtenerConfiguracion();

            if (config != null)
            {
                Gateway = config.Gateway;
                RutaNmap = config.RutaNmap;
                RutaVNC = config.RutaVNC;
            }
        }

        private void Guardar()
        {
            if (string.IsNullOrWhiteSpace(Gateway) || string.IsNullOrWhiteSpace(RutaNmap) || string.IsNullOrWhiteSpace(RutaVNC))
            {
                MostrarMensaje("Todos los campos son obligatorios", "error");
                return;
            }

            var config = new ConfiguracionApp
            {
                Gateway = Gateway,
                RutaNmap = RutaNmap,
                RutaVNC = RutaVNC    
            };

            _configService.ActualizarConfiguracion(config);

            MostrarMensaje("Configuración guardada correctamente", "ok");
        }




        // =========================================================
        // 🔹 HELPERS
        // =========================================================
        private void MostrarMensaje(string texto, string tipo = "info")
        {
            Mensaje = texto;

            switch (tipo)
            {
                case "ok": MensajeColor = Brushes.LightGreen; break;
                case "error": MensajeColor = Brushes.IndianRed; break;
                case "warning": MensajeColor = Brushes.Gold; break;
                default: MensajeColor = Brushes.White; break;
            }
        }







    }



}
