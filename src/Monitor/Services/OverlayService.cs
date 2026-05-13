using Monitor.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitor.Services
{
    public class OverlayService : IOverlayService
    {

        private readonly MainViewModel _mainVM;

        public OverlayService(MainViewModel mainVM)
        {
            _mainVM = mainVM;
        }

        public void Show(object content)
        {
            _mainVM.MostrarOverlay(content);
        }

        public void Hide()
        {
            _mainVM.OcultarOverlay();
        }

    }
}
