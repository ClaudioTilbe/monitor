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
    public class DispositivosViewModel : INotifyPropertyChanged
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
        // 🔹 SERVICIOS / DEPENDENCIAS
        // =========================================================

        private readonly IDispositivoService _dispositivoService;
        private readonly IDispositivoTituloService _tituloService;
        private readonly IDialogService _dialogService;
        private readonly IPingService _pingService;



        // =========================================================
        // 🔹 ESTADO UI
        // =========================================================

        public Visibility PuedeIrAnterior =>
            ModuloActual > 1 ? Visibility.Visible : Visibility.Collapsed;

        public Visibility PuedeIrSiguiente =>
            ModuloActual < TotalModulos ? Visibility.Visible : Visibility.Collapsed;

        private bool _mostrarGuias;
        public bool MostrarGuias
        {
            get => _mostrarGuias;
            set
            {
                _mostrarGuias = value;
                OnPropertyChanged();
            }
        }



        // =========================================================
        // 🔹 MÓDULOS
        // =========================================================

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




        // =========================================================
        // 🔹 MENSAJES
        // =========================================================

        private string _mensaje;
        public string Mensaje
        {
            get => _mensaje;
            set
            {
                _mensaje = value;
                OnPropertyChanged();
            }
        }

        private Brush _mensajeColor = Brushes.White;
        public Brush MensajeColor
        {
            get => _mensajeColor;
            set
            {
                _mensajeColor = value;
                OnPropertyChanged();
            }
        }



        // =========================================================
        // 🔹 MONITOREO
        // =========================================================

        private CancellationTokenSource _cts;



        // =========================================================
        // 🔹 COLECCIONES
        // =========================================================

        public ObservableCollection<GuiaItem> Guias { get; set; }
        public ObservableCollection<object> ElementosGrid { get; set; }



        // =========================================================
        // 🔹 COMMANDS
        // =========================================================

        public ICommand ToggleGuiasCommand { get; }
        public ICommand AbrirIPCommand { get; }

        public ICommand AgregarDispositivoCommand { get; }
        public ICommand AgregarTituloCommand { get; }
        public ICommand DesasociarCommand { get; }

        public ICommand ModuloSiguienteCommand { get; }
        public ICommand ModuloAnteriorCommand { get; }



        // =========================================================
        // 🔹 CONSTRUCTOR
        // =========================================================

        public DispositivosViewModel(
            IDispositivoService dispositivoService,
            IDispositivoTituloService tituloService,
            IDialogService dialogService,
            IPingService pingService)
        {
            _dispositivoService = dispositivoService;
            _tituloService = tituloService;
            _dialogService = dialogService;
            _pingService = pingService;

            ToggleGuiasCommand = new RelayCommand(_ => MostrarGuias = !MostrarGuias);
            AbrirIPCommand = new RelayCommand(OnAbrirDispositivoIP);

            AgregarDispositivoCommand = new RelayCommand(OnAgregarDispositivo);
            AgregarTituloCommand = new RelayCommand(OnAgregarTitulo);
            DesasociarCommand = new RelayCommand(OnDesasociar);

            ModuloSiguienteCommand = new RelayCommand(_ => IrModuloSiguiente(), _ => ModuloActual < TotalModulos);
            ModuloAnteriorCommand = new RelayCommand(_ => IrModuloAnterior(), _ => ModuloActual > 1);

            GenerarGuias();
            CargarElementos();
            IniciarMonitoreo();

            MostrarMensaje("¡Bienvenido a Monitor!", "info");
        }



        // =========================================================
        // 🔹 UI EVENTS
        // =========================================================

        private void OnAbrirDispositivoIP(object obj)
        {
            if (obj is not Dispositivo dispositivo)
                return;

            try
            {
                string url = dispositivo.IP;

                if (!url.StartsWith("http://") && !url.StartsWith("https://"))
                    url = $"http://{url}";

                Process.Start(new ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true
                });

                MostrarMensaje($"Abriendo {url}", "info");
            }
            catch (Exception ex)
            {
                MostrarMensaje($"Error al abrir: {ex.Message}", "error");
            }
        }



        // =========================================================
        // 🔹 CRUD
        // =========================================================

        private void OnAgregarDispositivo(object obj)
        {
            if (obj is not GuiaItem celda)
                return;

            var vm = new AgregarDispositivoViewModel();
            bool? result = _dialogService.ShowDialog<AgregarDispositivoWindow>(vm);

            if (result != true || !vm.Aceptado)
            {
                MostrarMensaje("Operación cancelada", "warning");
                return;
            }

            try
            {
                var dispositivo = new Dispositivo
                {
                    IP = vm.IP,
                    Alias = vm.Alias,
                    Modulo = ModuloActual,
                    Fila = celda.Row,
                    Columna = celda.Column
                };

                _dispositivoService.Agregar(dispositivo);

                CargarElementos();
                MostrarMensaje($"Dispositivo agregado en [{celda.Row}, {celda.Column}]", "ok");
            }
            catch (Exception ex)
            {
                MostrarMensaje($"Error: {ex.Message}", "error");
            }
        }

        private void OnAgregarTitulo(object obj)
        {
            if (obj is not GuiaItem celda)
                return;

            var vm = new AgregarDispositivoTituloViewModel();
            bool? result = _dialogService.ShowDialog<AgregarDispositivoTituloWindow>(vm);

            if (result != true || !vm.Aceptado)
            {
                MostrarMensaje("Operación cancelada", "warning");
                return;
            }

            try
            {
                var titulo = new DispositivoTitulo
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
            if (obj is not GuiaItem celda)
                return;

            try
            {
                var elemento = ElementosGrid.FirstOrDefault(x =>
                    (x is Dispositivo v && v.Fila == celda.Row && v.Columna == celda.Column) ||
                    (x is DispositivoTitulo t && t.Fila == celda.Row && t.Columna == celda.Column)
                );

                if (elemento is Dispositivo)
                {
                    _dispositivoService.Eliminar(ModuloActual, celda.Row, celda.Column);
                    MostrarMensaje("Dispositivo eliminado con exito", "ok");
                }
                else if (elemento is DispositivoTitulo)
                {
                    _tituloService.Eliminar(ModuloActual, celda.Row, celda.Column);
                    MostrarMensaje("Título eliminado con exito", "ok");
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
        // 🔹 MÓDULOS
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
            {
                for (int c = 0; c < Columnas; c++)
                {
                    Guias.Add(new GuiaItem { Row = r, Column = c });
                }
            }

            Guias = new ObservableCollection<GuiaItem>(
                Guias.OrderBy(x => x.Row).ThenBy(x => x.Column)
            );

            OnPropertyChanged(nameof(Guias));
        }

        private void CargarElementos()
        {
            var dispositivos = _dispositivoService.Listado(ModuloActual);
            var titulos = _tituloService.Listado(ModuloActual);

            int total = Filas * Columnas;
            var lista = Enumerable.Repeat<object>(null, total).ToList();

            foreach (var d in dispositivos)
            {
                int index = d.Fila * Columnas + d.Columna;
                lista[index] = d;
            }

            foreach (var t in titulos)
            {
                int index = t.Fila * Columnas + t.Columna;
                lista[index] = t;
            }

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
            catch (OperationCanceledException) { }
            catch (Exception ex)
            {
                Debug.WriteLine($"ERROR MONITOREO: {ex}");
            }
        }

        private async Task MonitorearAsync(CancellationToken token)
        {
            if (ElementosGrid == null)
                return;

            var lista = ElementosGrid
                .OfType<Dispositivo>()
                .Where(x => !string.IsNullOrWhiteSpace(x.IP))
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
