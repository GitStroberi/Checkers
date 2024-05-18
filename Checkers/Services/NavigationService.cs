using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Checkers.Core;

namespace Checkers.Services
{
    public interface INavigationService
    {
        ViewModel CurrentView { get; }
        void NavigateTo<T>() where T : ViewModel;

        T GetViewModel<T>() where T : ViewModel;

        void ExecuteCommandOnViewModel<T>(Action<T> action, T viewModel) where T : ViewModel;

    }
    internal class NavigationService : ObservableObject, INavigationService
    {
        private readonly Func<Type, ViewModel> _viewModelFactory;
        private ViewModel _currentView;
        public ViewModel CurrentView
        {
            get => _currentView;
            private set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }

        public NavigationService(Func<Type, ViewModel> viewModelFactory)
        {
            _viewModelFactory = viewModelFactory;
        }
        public void NavigateTo<TViewModel>() where TViewModel : ViewModel
        {
            ViewModel viewModel = _viewModelFactory.Invoke(typeof(TViewModel));
            CurrentView = viewModel;
        }

        public TViewModel GetViewModel<TViewModel>() where TViewModel : ViewModel
        {
            return (TViewModel)_viewModelFactory.Invoke(typeof(TViewModel));
        }

        public void ExecuteCommandOnViewModel<TViewModel>(Action<TViewModel> action, TViewModel viewModel) where TViewModel : ViewModel
        {
            action.Invoke(viewModel);
        }
    }
}
