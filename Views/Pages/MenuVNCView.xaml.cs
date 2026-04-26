using Monitor.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Monitor.Views.Pages
{

    public partial class MenuVNCView : UserControl
    {

        public MenuVNCView()
        {
            InitializeComponent();

            Loaded += OnLoaded;
            Unloaded += OnUnloaded;
        }


        private void OnGuiaLeftClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is Button btn && btn.ContextMenu != null)
            {
                btn.ContextMenu.PlacementTarget = btn;
                btn.ContextMenu.IsOpen = true;
            }
        }


        //Inicio - Fin de monitreo
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is MenuVNCViewModel vm)
            {
                vm.IniciarMonitoreo();
            }
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is MenuVNCViewModel vm)
            {
                vm.DetenerMonitoreo();
            }
        }


    }
}
