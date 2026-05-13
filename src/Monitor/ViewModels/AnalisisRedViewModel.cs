using Monitor.Commands;
using Monitor.Models;
using Monitor.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace Monitor.ViewModels
{
    public class AnalisisRedViewModel : INotifyPropertyChanged
    {

        // =========================
        // 🔹 INotifyPropertyChanged
        // =========================

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));


        // =========================================================
        // 🔹 DEPENDENCIAS / SERVICIOS
        // =========================================================

        private readonly IPingService _pingService;


        // =========================================================
        // 🔹 CAMPOS PRIVADOS
        // =========================================================

        private int totalIPs = 255;
        private int progresoActual = 0;
        private CancellationTokenSource _cts;


        // =========================================================
        // 🔹 PROPIEDADES (BINDING)
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

        private string _subred;
        public string Subred
        {
            get => _subred;
            set { _subred = value; OnPropertyChanged(); }
        }

        public ObservableCollection<ResultadoPing> Resultados { get; set; }

        private int _progreso;
        public int Progreso
        {
            get => _progreso;
            set { _progreso = value; OnPropertyChanged(); }
        }

        private string _progresoTexto;
        public string ProgresoTexto
        {
            get => _progresoTexto;
            set { _progresoTexto = value; OnPropertyChanged(); }
        }


        // =========================================================
        // 🔹 COMANDOS
        // =========================================================

        public ICommand IniciarEscaneoCommand { get; }


        // =========================================================
        // 🔹 CONSTRUCTOR
        // =========================================================

        public AnalisisRedViewModel(IPingService pingService)
        {
            _pingService = pingService;

            IniciarEscaneoCommand = new RelayCommand(async _ => await IniciarEscaneo());

            Resultados = new ObservableCollection<ResultadoPing>();

            MostrarMensaje("¡Bienvenido a Monitor!", "info");
        }


        // =========================================================
        // 🔹 LÓGICA PRINCIPAL
        // =========================================================

        private async Task IniciarEscaneo()
        {
            if (!ValidarSubred(Subred, out string error))
            {
                MostrarMensaje(error, "error");
                return;
            }

            // Reset
            Resultados.Clear();
            progresoActual = 0;
            Progreso = 0;
            ProgresoTexto = $"0 / {totalIPs}";

            _cts = new CancellationTokenSource();

            var animacion = AnimarTextoEscaneo(_cts.Token);

            var resultados = await EscanearSubredAsync(Subred);

            _cts.Cancel();

            foreach (var r in resultados)
                Resultados.Add(r);

            MostrarMensaje($"Finalizado! {resultados.Count} dispositivos encontrados.", "ok");
        }

        private async Task<List<ResultadoPing>> EscanearSubredAsync(string subredBase)
        {
            var resultados = new List<ResultadoPing>();
            var semaphore = new SemaphoreSlim(20);

            var tareas = Enumerable.Range(1, 255).Select(async i =>
            {
                await semaphore.WaitAsync();

                try
                {
                    string ip = $"{subredBase}.{i}";

                    bool ok = await _pingService.HacerPingAsync(ip);

                    if (ok)
                    {
                        lock (resultados)
                        {
                            resultados.Add(new ResultadoPing
                            {
                                IP = ip,
                                Activo = true,
                                TiempoRespuesta = 0
                            });
                        }
                    }
                }
                finally
                {
                    semaphore.Release();

                    int progreso = Interlocked.Increment(ref progresoActual);

                    Progreso = progreso;
                    ProgresoTexto = $"{progreso} / {totalIPs}";
                }
            });

            await Task.WhenAll(tareas);

            return resultados
                .OrderBy(r => System.Net.IPAddress.Parse(r.IP).GetAddressBytes(), new ByteArrayComparer())
                .ToList();
        }

        private async Task AnimarTextoEscaneo(CancellationToken token)
        {
            int puntos = 0;

            while (!token.IsCancellationRequested)
            {
                puntos = (puntos % 3) + 1;
                Mensaje = "Escaneando" + new string('.', puntos);

                try
                {
                    await Task.Delay(1000, token);
                }
                catch { break; }
            }
        }


        // =========================================================
        // 🔹 VALIDACIONES
        // =========================================================

        private bool ValidarSubred(string subred, out string mensajeError)
        {
            mensajeError = "";

            if (string.IsNullOrWhiteSpace(subred))
            {
                mensajeError = "La subred es obligatoria";
                return false;
            }

            var partes = subred.Split('.');

            if (partes.Length != 3)
            {
                mensajeError = "Formato inválido. Ej: 192.168.1";
                return false;
            }

            foreach (var parte in partes)
            {
                if (!int.TryParse(parte, out int valor) || valor < 0 || valor > 255)
                {
                    mensajeError = "Valores inválidos (0-255)";
                    return false;
                }
            }

            return true;
        }


        // =========================================================
        // 🔹 HELPERS
        // =========================================================

        public class ByteArrayComparer : IComparer<byte[]>
        {
            public int Compare(byte[] x, byte[] y)
            {
                for (int i = 0; i < x.Length; i++)
                {
                    int result = x[i].CompareTo(y[i]);
                    if (result != 0)
                        return result;
                }
                return 0;
            }
        }

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
