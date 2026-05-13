using Monitor.ViewModels.Dialogs;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Monitor.Views.Dialogs
{

    public partial class AgregarAccesoVNCWindow : Window
    {


        public AgregarAccesoVNCWindow()
        {
            InitializeComponent();

            Loaded += (s, e) =>
            {
                if (DataContext is AgregarAccesoVNCViewModel vm)
                {
                    vm.CloseAction = result =>
                    {
                        DialogResult = result;
                        Close();
                    };
                }
            };
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Fade in
            var fade = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromMilliseconds(200)
            };

            this.BeginAnimation(Window.OpacityProperty, fade);

            // Zoom in
            var scaleAnim = new DoubleAnimation
            {
                From = 0.9,
                To = 1,
                Duration = TimeSpan.FromMilliseconds(200)
            };

            scaleTransform.BeginAnimation(ScaleTransform.ScaleXProperty, scaleAnim);
            scaleTransform.BeginAnimation(ScaleTransform.ScaleYProperty, scaleAnim);

            //Para hacer focus en txtIP
            txtIP.Focus();
            Keyboard.Focus(txtIP);
        }


        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                CerrarConAnimacion(false);
            }
        }





        private void CerrarConAnimacion(bool resultado)
        {
            var fade = new DoubleAnimation
            {
                From = 1,
                To = 0,
                Duration = TimeSpan.FromMilliseconds(150)
            };

            var scaleAnim = new DoubleAnimation
            {
                From = 1,
                To = 0.9,
                Duration = TimeSpan.FromMilliseconds(150)
            };

            fade.Completed += (s, e) =>
            {
                DialogResult = resultado;
                Close();
            };

            //  aplicar animaciones
            this.BeginAnimation(Window.OpacityProperty, fade);

            scaleTransform.BeginAnimation(ScaleTransform.ScaleXProperty, scaleAnim);
            scaleTransform.BeginAnimation(ScaleTransform.ScaleYProperty, scaleAnim);
        }



    }
}
