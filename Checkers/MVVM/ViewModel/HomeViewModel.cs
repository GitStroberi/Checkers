using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Checkers.Core;
using Checkers.Services;

namespace Checkers.MVVM.ViewModel
{
    public class HomeViewModel : Core.ViewModel
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

        public RelayCommand NavigateBoardCommand { get; set; }

        public RelayCommand LoadGameCommand { get; set; }

        public RelayCommand StatisticsCommand { get; set; }
        public HomeViewModel(INavigationService navigation)
        {
            Navigation = navigation;
            NavigateBoardCommand = new RelayCommand(o => { Navigation.NavigateTo<BoardViewModel>(); }, o => true);
            LoadGameCommand = new RelayCommand(o =>
            {
                Navigation.NavigateTo<BoardViewModel>();
                ((BoardViewModel)Navigation.CurrentView).LoadGameCommand.Execute(null);
            }, o => true);
            StatisticsCommand = new RelayCommand(o =>
            {
                //get the BoardViewModel with the NavigationGetViewModel method
                BoardViewModel boardViewModel = Navigation.GetViewModel<BoardViewModel>();
                if (boardViewModel != null)
                {
                    boardViewModel.StatisticsCommand.Execute(null);
                }
            }, o => true);
        }
    }
}
