using Microsoft.Extensions.DependencyInjection;
using Monitor.Data;
using Monitor.Repositories;
using Monitor.Services;
using Monitor.ViewModels;
using Monitor.ViewModels.Dialogs;
using Monitor.Views.Dialogs;
using Monitor.Views.Pages;
using System.Configuration;
using System.Data;
using System.Windows;



namespace Monitor
{

    public partial class App : Application
    {

        public static IServiceProvider ServiceProvider { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {

            try
            {
                base.OnStartup(e);

                var services = new ServiceCollection();



                // =========================================================
                // DATABASE
                // =========================================================
                services.AddScoped<DatabaseContext>(); 



                // =========================================================
                // REPOSITORIES
                // =========================================================
                services.AddScoped<IConfiguracionAppRepository, ConfiguracionAppRepository>();
                services.AddScoped<IBalanzaRepository, BalanzaRepository>();
                services.AddScoped<IAccesoVNCRepository, AccesoVNCRepository>();
                services.AddScoped<IAccesoVNCTituloRepository, AccesoVNCTituloRepository>();
                services.AddScoped<IDispositivoRepository, DispositivoRepository>();
                services.AddScoped<IDispositivoTituloRepository, DispositivoTituloRepository>();



                // =========================================================
                // SERVICES
                // =========================================================
                services.AddScoped<IAccesoVNCService, AccesoVNCService>();
                services.AddScoped<IAccesoVNCTituloService, AccesoVNCTituloService>();

                services.AddScoped<IConfiguracionAppService, ConfiguracionAppService>();
                services.AddScoped<IBalanzaService, BalanzaService>();

                services.AddScoped<IDispositivoService, DispositivoService>();
                services.AddScoped<IDispositivoTituloService, DispositivoTituloService>();

                services.AddSingleton<IPingService, PingService>(); 
                services.AddScoped<IDialogService, DialogService>();

                services.AddSingleton<IOverlayService, OverlayService>();



                // =========================================================
                // VIEWMODELS
                // =========================================================
                services.AddSingleton<MainViewModel>();

                services.AddTransient<MenuVNCViewModel>();
                services.AddTransient<BalanzasViewModel>();
                services.AddTransient<DispositivosViewModel>();
                services.AddTransient<AnalisisRedViewModel>();
                services.AddTransient<EscaneoPuertosViewModel>();
                services.AddTransient<ConfiguracionAppViewModel>();



                // =========================================================
                // DIALOGS
                // =========================================================
                services.AddTransient<AgregarAccesoVNCWindow>();
                services.AddTransient<AgregarAccesoVNCViewModel>();

                services.AddTransient<AgregarAccesoVNCTituloWindow>();
                services.AddTransient<AgregarAccesoVNCTituloViewModel>();

                services.AddTransient<AgregarBalanzaWindow>();
                services.AddTransient<AgregarBalanzaViewModel>();

                services.AddTransient<AgregarDispositivoWindow>();
                services.AddTransient<AgregarDispositivoViewModel>();

                services.AddTransient<AgregarDispositivoTituloWindow>();
                services.AddTransient<AgregarDispositivoTituloViewModel>();



                // =========================================================
                // BUILD PROVIDER
                // =========================================================
                var provider = services.BuildServiceProvider();



                // =========================================================
                // INIT DB
                // =========================================================
                using (var scope = provider.CreateScope())
                {
                    var db = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                    DbInitializer.Initialize(db);
                }

                ServiceProvider = provider;



                // =========================================================
                // START APP
                // =========================================================
                var mainWindow = new MainWindow
                {
                    DataContext = ServiceProvider.GetRequiredService<MainViewModel>()
                };

                mainWindow.Show();

            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error fatal al iniciar la aplicación:\n\n{ex}",
                    "ERROR",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);

                Shutdown();
            }
        }


    }

}
