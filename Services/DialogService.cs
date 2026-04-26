using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Monitor.Services
{
    public class DialogService : IDialogService
    {

        private readonly IServiceProvider _provider;
        private readonly IOverlayService _overlay;

        public DialogService(IServiceProvider provider, IOverlayService overlay)
        {
            _provider = provider;
            _overlay = overlay;
        }

        public bool? ShowDialog<TView>(object viewModel) where TView : Window
        {
            var window = _provider.GetRequiredService<TView>();

            window.DataContext = viewModel;

            //  MOSTRAR OVERLAY
            _overlay.Show(null);

            var result = window.ShowDialog();

            //  OCULTAR OVERLAY
            _overlay.Hide();

            return result;
        }

    }
}
