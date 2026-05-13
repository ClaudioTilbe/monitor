using Microsoft.Extensions.DependencyInjection;
using Monitor.Commands;
using Monitor.Data;
using Monitor.Repositories;
using Monitor.Services;
using Monitor.Views.Pages;
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
    public class MainViewModel : INotifyPropertyChanged
    {

        // =========================================================
        // 🔹 INotifyPropertyChanged
        // =========================================================

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));



        // =========================================================
        // 🔹 Variables
        // =========================================================

        private object _currentView;
        private readonly INavigationService _navigation;
        private readonly IServiceProvider _serviceProvider;

        private readonly IConfiguracionAppService _configService;
        private readonly IPingService _pingService;

        private string _gateway;

        private string _usuarioInfo;
        private Brush _usuarioColor;
        private bool _isCollapsed;

        private bool _isOverlayVisible;
        private object _overlayContent;

        private string _titulo;




        // =========================================================
        // 🔹 PROPIEDADES (BINDING)
        // =========================================================

        public object CurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
                OnPropertyChanged();
                ActualizarTitulo(value);
            }
        }

        public string UsuarioInfo
        {
            get => _usuarioInfo;
            set { _usuarioInfo = value; OnPropertyChanged(); }
        }

        public Brush UsuarioColor
        {
            get => _usuarioColor;
            set { _usuarioColor = value; OnPropertyChanged(); }
        }

        public bool IsCollapsed
        {
            get => _isCollapsed;
            set
            {
                _isCollapsed = value;
                OnPropertyChanged();

                if (_isCollapsed)
                {
                    // COLLAPSE → inmediato
                    ActualizarUsuarioTexto();
                }
                else
                {
                    // EXPAND → esperar a que termine animación
                    _ = ActualizarUsuarioConDelay();
                }
            }
        }

        public bool IsOverlayVisible
        {
            get => _isOverlayVisible;
            set { _isOverlayVisible = value; OnPropertyChanged(); }
        }

        public object OverlayContent
        {
            get => _overlayContent;
            set { _overlayContent = value; OnPropertyChanged(); }
        }

        public string Titulo
        {
            get => _titulo;
            set { _titulo = value; OnPropertyChanged(); }
        }




        // =========================================================
        // 🔹 COMMANDS
        // =========================================================

        public ICommand GoMenuVNCCommand { get; }
        public ICommand GoBalanzasCommand { get; }
        public ICommand GoDispositivosCommand { get; }
        public ICommand GoAnalisisRedCommand { get; }
        public ICommand GoEscaneoPuertosCommand { get; }
        public ICommand GoConfiguracionAppCommand { get; }

        public ICommand ToggleMenuCommand { get; }




        // =========================================================
        // 🔹 CONSTRUCTOR
        // =========================================================

        public MainViewModel(
            IConfiguracionAppService configService,
            IPingService pingService,
            IServiceProvider serviceProvider)
        {
            _configService = configService;
            _pingService = pingService;
            _serviceProvider = serviceProvider;

            _navigation = new NavigationService(
                type => _serviceProvider.GetRequiredService(type),
                view => CurrentView = view
            );

            // Commands
            ToggleMenuCommand = new RelayCommand(_ => IsCollapsed = !IsCollapsed);

            GoMenuVNCCommand = new RelayCommand(_ => _navigation.NavigateTo<MenuVNCViewModel>());
            GoBalanzasCommand = new RelayCommand(_ => _navigation.NavigateTo<BalanzasViewModel>());
            GoConfiguracionAppCommand = new RelayCommand(_ => _navigation.NavigateTo<ConfiguracionAppViewModel>());
            GoDispositivosCommand = new RelayCommand(_ => _navigation.NavigateTo<DispositivosViewModel>());
            GoAnalisisRedCommand = new RelayCommand(_ => _navigation.NavigateTo<AnalisisRedViewModel>());
            GoEscaneoPuertosCommand = new RelayCommand(_ => _navigation.NavigateTo<EscaneoPuertosViewModel>());

            CargarConfiguracion();

            _ = InicializarUsuario();
            _ = MonitorearUsuario();
        }

        public void Inicializar()
        {
            _navigation.NavigateTo<MenuVNCViewModel>();
        }



        // =========================================================
        // 🔹 CONFIGURACIÓN
        // =========================================================

        private void CargarConfiguracion()
        {
            try
            {
                var config = _configService.ObtenerConfiguracion();
                _gateway = config?.Gateway ?? "192.168.1.1";
            }
            catch
            {
                _gateway = "192.168.1.1";
            }
        }



        // =========================================================
        // 🔹 LÓGICA DE USUARIO
        // =========================================================

        public void ActualizarUsuarioTexto()
        {
            string usuario = Environment.UserName;
            string ipLocal = ObtenerIPLocal();

            UsuarioInfo = IsCollapsed
                ? usuario
                : $"Usuario: {usuario} | IP: {ipLocal}";
        }


        private async Task ActualizarUsuarioConDelay()
        {
            await Task.Delay(300);

            ActualizarUsuarioTexto();
        }


        private async Task InicializarUsuario()
        {
            bool hayConexion = await VerificarConexion();

            string usuario = Environment.UserName;
            string ipLocal = ObtenerIPLocal();

            UsuarioInfo = IsCollapsed
                ? usuario
                : $"Usuario: {usuario} | IP: {ipLocal}";

            UsuarioColor = hayConexion
                ? Brushes.LightGreen
                : Brushes.IndianRed;
        }

        private async Task MonitorearUsuario()
        {
            while (true)
            {
                bool hayConexion = await VerificarConexion();

                string usuario = Environment.UserName;
                string ipLocal = ObtenerIPLocal();

                UsuarioInfo = IsCollapsed
                    ? usuario
                    : $"Usuario: {usuario} | IP: {ipLocal}";

                UsuarioColor = hayConexion
                    ? Brushes.LightGreen
                    : Brushes.IndianRed;

                await Task.Delay(5000);
            }
        }

        private async Task<bool> VerificarConexion()
        {
            if (string.IsNullOrWhiteSpace(_gateway))
                return false;

            return await _pingService.HacerPingAsync(_gateway);
        }

        private string ObtenerIPLocal()
        {
            var host = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());

            var ip = host.AddressList
                .FirstOrDefault(x => x.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);

            return ip?.ToString() ?? "Sin IP";
        }



        // =========================================================
        // 🔹 OVERLAY
        // =========================================================

        public void MostrarOverlay(object contenido)
        {
            OverlayContent = contenido;
            IsOverlayVisible = true;
        }

        public void OcultarOverlay()
        {
            IsOverlayVisible = false;
            OverlayContent = null;
        }




        // =========================================================
        // 🔹 NAVEGACIÓN / UI
        // =========================================================

        private void ActualizarTitulo(object viewModel)
        {
            switch (viewModel)
            {
                case MenuVNCViewModel:
                    Titulo = "Menu VNC";
                    break;

                case BalanzasViewModel:
                    Titulo = "Balanzas";
                    break;

                case DispositivosViewModel:
                    Titulo = "Dispositivos";
                    break;

                case AnalisisRedViewModel:
                    Titulo = "Analisis de red";
                    break;

                case EscaneoPuertosViewModel:
                    Titulo = "Escaneo de puertos";
                    break;

                case ConfiguracionAppViewModel:
                    Titulo = "Configuracion";
                    break;

                default:
                    Titulo = "Testing page";
                    break;
            }
        }




       
    }




}



