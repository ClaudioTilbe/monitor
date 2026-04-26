using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Monitor.Services
{
    public interface IDialogService
    {

        bool? ShowDialog<TWindow>(object viewModel) where TWindow : Window;

    }
}
