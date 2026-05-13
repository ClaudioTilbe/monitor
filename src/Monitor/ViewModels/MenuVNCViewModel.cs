using Monitor.Commands;
using Monitor.Models;
using Monitor.Services;
using Monitor.ViewModels.Dialogs;
using Monitor.Views.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Monitor.ViewModels
{

    public class MenuVNCViewModel : INotifyPropertyChanged
    {


        // =========================================================
        // 🔹 INotifyPropertyChanged
        // =========================================================

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));




        // =========================================================
        // 🔹 CONFIGURACIÓN
        // =========================================================

        public int Filas { get; set; } = 14;
        public int Columnas { get; set; } = 6;
        public int TotalModulos { get; set; } = 10;




        // =========================================================
        // 🔹 SERVICIOS
        // =========================================================

        private readonly IAccesoVNCService _accesoService;
        private readonly IAccesoVNCTituloService _tituloService;
        private readonly IDialogService _dialogService;
        private readonly IPingService _pingService;
        private readonly IConfiguracionAppService _configService;





        // =========================================================
        // 🔹 ESTADO / PROPIEDADES
        // =========================================================

        private bool _mostrarGuias;
        public bool MostrarGuias
        {
            get => _mostrarGuias;
            set { _mostrarGuias = value; OnPropertyChanged(); }
        }

        private int _moduloActual = 1;
        public int ModuloActual
        {
            get => _moduloActual;
            set
            {
                _moduloActual = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(PuedeIrAnterior));
                OnPropertyChanged(nameof(PuedeIrSiguiente));
            }
        }

        public Visibility PuedeIrAnterior =>
            ModuloActual > 1 ? Visibility.Visible : Visibility.Collapsed;

        public Visibility PuedeIrSiguiente =>
            ModuloActual < TotalModulos ? Visibility.Visible : Visibility.Collapsed;




        // =========================================================
        // 🔹 MENSAJES UI
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
        // 🔹 COLECCIONES
        // =========================================================

        public ObservableCollection<GuiaItem> Guias { get; set; }
        public ObservableCollection<object> ElementosGrid { get; set; }




        // =========================================================
        // 🔹 COMANDOS
        // =========================================================

        public ICommand ToggleGuiasCommand { get; }

        public ICommand AgregarVNCCommand { get; }
        public ICommand AgregarTituloCommand { get; }
        public ICommand DesasociarCommand { get; }


        public ICommand ModuloSiguienteCommand { get; }
        public ICommand ModuloAnteriorCommand { get; }

        public ICommand AbrirVNCCommand { get; }




        // =========================================================
        // 🔹 MONITOREO
        // =========================================================

        private CancellationTokenSource _cts;



        // =========================================================
        // 🔹 CONSTRUCTOR
        // =========================================================

        public MenuVNCViewModel(
            IAccesoVNCService accesoService,
            IAccesoVNCTituloService tituloService,
            IDialogService dialogService,
            IPingService pingService,
            IConfiguracionAppService configService)
        {
            _accesoService = accesoService;
            _tituloService = tituloService;
            _dialogService = dialogService;
            _pingService = pingService;
            _configService = configService;

            ToggleGuiasCommand = new RelayCommand(_ => MostrarGuias = !MostrarGuias);

            AgregarVNCCommand = new RelayCommand(OnAgregarVNC);
            AgregarTituloCommand = new RelayCommand(OnAgregarTitulo);
            DesasociarCommand = new RelayCommand(OnDesasociar);

            ModuloSiguienteCommand = new RelayCommand(_ => IrModuloSiguiente(), _ => ModuloActual < TotalModulos);
            ModuloAnteriorCommand = new RelayCommand(_ => IrModuloAnterior(), _ => ModuloActual > 1);

            AbrirVNCCommand = new RelayCommand(OnAbrirVNC);

            GenerarGuias();
            CargarElementos();

            MostrarMensaje("¡Bienvenido a Monitor!", "info");
        }



        // =========================================================
        // 🔹 UI / INTERACCIONES
        // =========================================================

        private void OnAbrirVNC(object obj)
        {
            if (obj is not AccesoVNC vnc) return;

            try
            {
                MostrarMensaje($"Conectando a {vnc.IP}...", "info");

                var config = _configService.ObtenerConfiguracion();

                if (config == null || string.IsNullOrWhiteSpace(config.RutaVNC))
                {
                    MostrarMensaje("Ruta de VNC no configurada", "error");
                    return;
                }

                if (!File.Exists(config.RutaVNC))
                {
                    MostrarMensaje("La ruta de VNC no es válida", "error");
                    return;
                }

                Process.Start(new ProcessStartInfo
                {
                    FileName = config.RutaVNC,
                    Arguments = vnc.IP,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MostrarMensaje($"Error: {ex.Message}", "error");
            }
        }



        // =========================================================
        // 🔹 CRUD
        // =========================================================

        private void OnAgregarVNC(object obj)
        {
            if (obj is not GuiaItem celda) return;

            if (CeldaOcupada(celda.Row, celda.Column))
            {
                MostrarMensaje("La celda ya está ocupada", "warning");
                return;
            }

            var vm = new AgregarAccesoVNCViewModel();
            bool? result = _dialogService.ShowDialog<AgregarAccesoVNCWindow>(vm);

            if (result != true || !vm.Aceptado)
            {
                MostrarMensaje("Operación cancelada", "warning");
                return;
            }

            try
            {
                var acceso = new AccesoVNC
                {
                    IP = vm.IP,
                    Alias = vm.Alias,
                    Modulo = ModuloActual,
                    Fila = celda.Row,
                    Columna = celda.Column
                };

                _accesoService.Agregar(acceso);

                CargarElementos();
                MostrarMensaje($"VNC agregado en [{celda.Row}, {celda.Column}]", "ok");
            }
            catch (Exception ex)
            {
                MostrarMensaje($"Error: {ex.Message}", "error");
            }
        }

        private void OnAgregarTitulo(object obj)
        {
            if (obj is not GuiaItem celda) return;

            if (CeldaOcupada(celda.Row, celda.Column))
            {
                MostrarMensaje("La celda ya está ocupada", "warning");
                return;
            }

            var vm = new AgregarAccesoVNCTituloViewModel();
            bool? result = _dialogService.ShowDialog<AgregarAccesoVNCTituloWindow>(vm);

            if (result != true || !vm.Aceptado)
            {
                MostrarMensaje("Operación cancelada", "warning");
                return;
            }

            try
            {
                var titulo = new AccesoVNCTitulo
                {
                    Texto = vm.Texto,
                    Modulo = ModuloActual,
                    Fila = celda.Row,
                    Columna = celda.Column
                };

                _tituloService.Agregar(titulo);
                CargarElementos();

                MostrarMensaje($"Título agregado en [{celda.Row}, {celda.Column}]", "ok");
            }
            catch (Exception ex)
            {
                MostrarMensaje($"Error: {ex.Message}", "error");
            }
        }

        private void OnDesasociar(object obj)
        {
            if (obj is not GuiaItem celda) return;

            try
            {
                var elemento = ElementosGrid.FirstOrDefault(x =>
                    (x is AccesoVNC v && v.Fila == celda.Row && v.Columna == celda.Column) ||
                    (x is AccesoVNCTitulo t && t.Fila == celda.Row && t.Columna == celda.Column));

                if (elemento is AccesoVNC vnc)
                {
                    _accesoService.Eliminar(ModuloActual, celda.Row, celda.Column);

                    MostrarMensaje("Acceso VNC eliminado", "ok");
                }
                else if (elemento is AccesoVNCTitulo)
                {
                    _tituloService.Eliminar(ModuloActual, celda.Row, celda.Column);
                    MostrarMensaje("Título eliminado", "ok");
                }
                else
                {
                    MostrarMensaje("No hay nada en esa celda", "warning");
                    return;
                }

                CargarElementos();
            }
            catch (Exception ex)
            {
                MostrarMensaje($"Error: {ex.Message}", "error");
            }
        }



        // =========================================================
        // 🔹 NAVEGACIÓN DE MÓDULOS
        // =========================================================

        private void IrModuloSiguiente()
        {
            if (ModuloActual < TotalModulos)
            {
                ModuloActual++;
                OnCambioModulo();
            }
        }

        private void IrModuloAnterior()
        {
            if (ModuloActual > 1)
            {
                ModuloActual--;
                OnCambioModulo();
            }
        }

        private void OnCambioModulo()
        {
            CargarElementos();
            DetenerMonitoreo();
            IniciarMonitoreo();

            MostrarMensaje($"Estás en el módulo {ModuloActual}", "info");
            CommandManager.InvalidateRequerySuggested();
        }



        // =========================================================
        // 🔹 DATA
        // =========================================================

        private void GenerarGuias()
        {
            Guias = new ObservableCollection<GuiaItem>();

            for (int r = 0; r < Filas; r++)
                for (int c = 0; c < Columnas; c++)
                    Guias.Add(new GuiaItem { Row = r, Column = c });

            Guias = new ObservableCollection<GuiaItem>(
                Guias.OrderBy(x => x.Row).ThenBy(x => x.Column));

            OnPropertyChanged(nameof(Guias));
        }

        private void CargarElementos()
        {
            var accesos = _accesoService.Listado(ModuloActual);
            var titulos = _tituloService.Listado(ModuloActual);

            int total = Filas * Columnas;
            var lista = Enumerable.Repeat<object>(null, total).ToList();

            foreach (var vnc in accesos)
                lista[vnc.Fila * Columnas + vnc.Columna] = vnc;

            foreach (var t in titulos)
                lista[t.Fila * Columnas + t.Columna] = t;

            ElementosGrid = new ObservableCollection<object>(lista);
            OnPropertyChanged(nameof(ElementosGrid));
        }



        // =========================================================
        // 🔹 MONITOREO
        // =========================================================

        public void IniciarMonitoreo()
        {
            DetenerMonitoreo();
            _cts = new CancellationTokenSource();
            _ = MonitoreoLoop(_cts.Token);
        }

        public void DetenerMonitoreo()
        {
            _cts?.Cancel();
        }

        private async Task MonitoreoLoop(CancellationToken token)
        {
            try
            {
                while (!token.IsCancellationRequested)
                {
                    await MonitorearAsync(token);
                    await Task.Delay(3000, token);
                }
            }
            catch (OperationCanceledException)
            {
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ERROR MONITOREO: {ex}");
            }
        }

        private async Task MonitorearAsync(CancellationToken token)
        {
            if (ElementosGrid == null) return;

            var lista = ElementosGrid
                .OfType<AccesoVNC>()
                .Where(x => x != null && !string.IsNullOrWhiteSpace(x.IP))
                .ToList();

            var semaphore = new SemaphoreSlim(10);

            var tareas = lista.Select(async item =>
            {
                await semaphore.WaitAsync(token);

                try
                {
                    bool online = await _pingService.HacerPingAsync(item.IP);

                    var dispatcher = Application.Current?.Dispatcher;
                    if (dispatcher == null) return;

                    await dispatcher.InvokeAsync(() =>
                    {
                        item.EstaOnline = online;
                    });
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Ping error {item.IP}: {ex.Message}");
                }
                finally
                {
                    semaphore.Release();
                }
            });

            await Task.WhenAll(tareas);
        }



        // =========================================================
        // 🔹 UTILIDADES
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


        private bool CeldaOcupada(int fila, int columna)
        {
            return ElementosGrid?.Any(x =>
                (x is AccesoVNC v && v.Fila == fila && v.Columna == columna) ||
                (x is AccesoVNCTitulo t && t.Fila == fila && t.Columna == columna)
            ) == true;
        }





    }


}
