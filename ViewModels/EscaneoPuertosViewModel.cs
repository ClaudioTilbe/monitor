using Monitor.Commands;
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
    public class EscaneoPuertosViewModel : INotifyPropertyChanged
    {


        // =========================================================
        // 🔹 INotifyPropertyChanged
        // =========================================================

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));



        // =========================================================
        // 🔹 DEPENDENCIAS / SERVICIOS
        // =========================================================

        private readonly IConfiguracionAppService _configService;
        private NmapService _nmapService;



        // =========================================================
        // 🔹 ESTADO UI
        // =========================================================

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



        // =========================================================
        // 🔹 PROPIEDADES
        // =========================================================

        private string _ip = "";
        public string IP
        {
            get => _ip;
            set { _ip = value; OnPropertyChanged(); }
        }

        private string _output;
        public string Output
        {
            get => _output;
            set { _output = value; OnPropertyChanged(); }
        }

        private bool _isEscaneando;
        public bool IsEscaneando
        {
            get => _isEscaneando;
            set
            {
                _isEscaneando = value;
                OnPropertyChanged();
            }
        }



        // =========================================================
        // 🔹 COMMANDS
        // =========================================================

        public ICommand IniciarEscaneoCommand { get; }



        // =========================================================
        // 🔹 CONSTRUCTOR
        // =========================================================

        public EscaneoPuertosViewModel(IConfiguracionAppService configService)
        {
            _configService = configService;

            IniciarEscaneoCommand = new RelayCommand(
                async _ => await EjecutarEscaneo(),
                _ => !IsEscaneando
            );

            CargarConfiguracion();

            MostrarMensaje("¡Bienvenido a Monitor!", "info");
        }



        // =========================================================
        // 🔹 LÓGICA
        // =========================================================

        private void CargarConfiguracion()
        {
            var config = _configService.ObtenerConfiguracion();

            if (config == null || string.IsNullOrWhiteSpace(config.RutaNmap))
            {
                MostrarMensaje("No se encontró la configuración de Nmap", "error");
                return;
            }

            _nmapService = new NmapService(config.RutaNmap);
        }

        private async Task EjecutarEscaneo()
        {
            if (!ValidarIP(IP, out string error))
            {
                MostrarMensaje(error, "error");
                return;
            }

            if (_nmapService == null)
            {
                MostrarMensaje("Nmap no está configurado correctamente", "error");
                return;
            }

            try
            {
                IsEscaneando = true;

                Output = "Escaneando...";
                MostrarMensaje("Ejecutando escaneo...", "info");

                var resultado = await _nmapService.EscanearAsync(IP);

                Output = _nmapService.FormatearResultado(resultado);

                MostrarMensaje("Escaneo completado", "ok");
            }
            catch (Exception ex)
            {
                MostrarMensaje($"Error: {ex.Message}", "error");
            }
            finally
            {
                IsEscaneando = false;
            }
        }



        // =========================================================
        // 🔹 VALIDACIONES
        // =========================================================

        private bool ValidarIP(string ip, out string mensajeError)
        {
            mensajeError = "";

            if (string.IsNullOrWhiteSpace(ip))
            {
                mensajeError = "La IP es obligatoria";
                return false;
            }

            if (!System.Net.IPAddress.TryParse(ip, out var direccion))
            {
                mensajeError = "Formato de IP inválido";
                return false;
            }

            if (direccion.AddressFamily != System.Net.Sockets.AddressFamily.InterNetwork)
            {
                mensajeError = "Solo se permiten IPv4";
                return false;
            }

            return true;
        }



        // =========================================================
        // 🔹 MENSAJES
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
