using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Checkers.Core;
using Checkers.MVVM.Model;
using Checkers.Services;

namespace Checkers.MVVM.ViewModel
{
    public class MainViewModel : Core.ViewModel
    {
        private INavigationService _navigation;

        public INavigationService Navigation
        {
            get => _navigation;
            set
            {
                _navigation = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand NavigateHomeCommand { get; set; }

        public RelayCommand NavigateBoardCommand { get; set; }

        public MainViewModel(INavigationService navigation)
        {
            Navigation = navigation;
            NavigateHomeCommand = new RelayCommand(o => { Navigation.NavigateTo<HomeViewModel>(); }, o => true);
            NavigateBoardCommand = new RelayCommand(o => { Navigation.NavigateTo<BoardViewModel>(); }, o => true);

            Navigation.NavigateTo<HomeViewModel>();
        }
    }
}
