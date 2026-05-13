using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Monitor.Services
{
    public class NavigationService : INavigationService
    {

        private readonly Func<Type, object> _viewModelFactory;
        private readonly Action<object> _setCurrentView;

        public NavigationService(
            Func<Type, object> viewModelFactory,
            Action<object> setCurrentView)
        {
            _viewModelFactory = viewModelFactory;
            _setCurrentView = setCurrentView;
        }

        public void NavigateTo<TViewModel>() where TViewModel : class
        {
            var viewModel = (TViewModel)_viewModelFactory(typeof(TViewModel));
            _setCurrentView(viewModel);
        }


    }

}
