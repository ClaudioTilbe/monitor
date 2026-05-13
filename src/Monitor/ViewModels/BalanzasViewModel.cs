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
    public class BalanzasViewModel : INotifyPropertyChanged
    {


        // =========================================================
        // 🔹 INotifyPropertyChanged
        // =========================================================

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));



        // =========================================================
        // 🔹 CONFIGURACIÓN
        // =========================================================

        public int Filas { get; set; } = 14;
        public int Columnas { get; set; } = 6;



        // =========================================================
        // 🔹 DEPENDENCIAS / SERVICIOS
        // =========================================================

        private readonly IBalanzaService _balanzaService;
        private readonly IDialogService _dialogService;
        private readonly IPingService _pingService;



        // =========================================================
        // 🔹 CAMPOS PRIVADOS
        // =========================================================

        private CancellationTokenSource _cts;



        // =========================================================
        // 🔹 PROPIEDADES (BINDING)
        // =========================================================

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
        // 🔹 COLECCIONES
        // =========================================================

        public ObservableCollection<GuiaItem> Guias { get; set; }
        public ObservableCollection<object> ElementosGrid { get; set; }



        // =========================================================
        // 🔹 COMANDOS
        // =========================================================

        public ICommand ToggleGuiasCommand { get; }

        public ICommand AgregarVNCCommand { get; }
        public ICommand DesasociarCommand { get; }



        // =========================================================
        // 🔹 CONSTRUCTOR
        // =========================================================

        public BalanzasViewModel(
            IBalanzaService balanzaService,
            IDialogService dialogService,
            IPingService pingService)
        {
            _balanzaService = balanzaService;
            _dialogService = dialogService;
            _pingService = pingService;

            ToggleGuiasCommand = new RelayCommand(_ => MostrarGuias = !MostrarGuias);

            AgregarVNCCommand = new RelayCommand(OnAsociarIP);
            DesasociarCommand = new RelayCommand(OnDesasociarIP);

            GenerarGuias();
            CargarElementos();

            IniciarMonitoreo();

            MostrarMensaje("¡Bienvenido a Monitor!", "info");
        }



        // =========================================================
        // 🔹 LÓGICA UI
        // =========================================================

        private void GenerarGuias()
        {
            Guias = new ObservableCollection<GuiaItem>();

            for (int r = 0; r < Filas; r++)
            {
                for (int c = 0; c < Columnas; c++)
                {
                    Guias.Add(new GuiaItem
                    {
                        Row = r,
                        Column = c
                    });
                }
            }

            Guias = new ObservableCollection<GuiaItem>(
                Guias.OrderBy(x => x.Row).ThenBy(x => x.Column)
            );

            OnPropertyChanged(nameof(Guias));
        }




        // =========================================================
        // 🔹 ACCIONES (CRUD)
        // =========================================================

        private void OnAsociarIP(object obj)
        {
            if (obj is not GuiaItem celda)
                return;

            var vm = new AgregarBalanzaViewModel();

            bool? result = _dialogService.ShowDialog<AgregarBalanzaWindow>(vm);

            if (result != true || !vm.Aceptado)
            {
                MostrarMensaje("Operación cancelada", "warning");
                return;
            }

            try
            {

                int numero = Convert.ToInt32(vm.NumeroBalanza);

                //Validacion
                if (_balanzaService.ExisteNumero(numero))
                {
                    MostrarMensaje($"La balanza {numero} ya existe", "warning");
                    return;
                }

                var balanza = new Balanza
                {
                    NumeroBalanza = numero,
                    IP = vm.IP,
                    Alias = vm.Alias,
                    Fila = celda.Row,
                    Columna = celda.Column
                };

                _balanzaService.Agregar(balanza);

                CargarElementos();

                MostrarMensaje($"Balanza agregada en [{celda.Row}, {celda.Column}]", "ok");
            }
            catch (Exception ex)
            {
                MostrarMensaje($"Error: {ex.Message}", "error");
            }
        }

        private void OnDesasociarIP(object obj)
        {
            if (obj is not GuiaItem celda)
                return;

            try
            {
                var elemento = ElementosGrid
                    .FirstOrDefault(x =>
                        x is Balanza b &&
                        b.Fila == celda.Row &&
                        b.Columna == celda.Column
                    );

                if (elemento is Balanza)
                {
                    _balanzaService.Eliminar(celda.Row, celda.Column);
                    MostrarMensaje("Balanza eliminada", "ok");
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
        // 🔹 DATA GRID
        // =========================================================

        private void CargarElementos()
        {
            var balanzas = _balanzaService.Listado();

            int total = Filas * Columnas;

            var lista = Enumerable.Repeat<object>(null, total).ToList();

            foreach (var balanza in balanzas)
            {
                int index = balanza.Fila * Columnas + balanza.Columna;
                lista[index] = balanza;
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
                // esperado
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ERROR MONITOREO: {ex}");
            }
        }

        public void DetenerMonitoreo()
        {
            _cts?.Cancel();
        }

        private async Task MonitorearAsync(CancellationToken token)
        {
            if (ElementosGrid == null)
                return;

            var lista = ElementosGrid
                .OfType<Balanza>()
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

                    if (dispatcher == null)
                        return;

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
