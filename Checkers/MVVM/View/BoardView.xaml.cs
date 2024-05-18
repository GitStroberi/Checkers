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
using Checkers.MVVM.Model;
using Checkers.MVVM.ViewModel;

namespace Checkers.MVVM.View
{
    /// <summary>
    /// Interaction logic for BoardView.xaml
    /// </summary>
    public partial class BoardView : UserControl
    {
        public BoardView()
        {
            InitializeComponent();
        }

        private void PieceClick(object sender, MouseButtonEventArgs e)
        {
            Ellipse clickedPiece = sender as Ellipse;
            if (clickedPiece != null)
            {
                PieceViewModel piece = clickedPiece.DataContext as PieceViewModel;
                if (piece != null)
                {
                    ((BoardViewModel)DataContext).PieceClickCommand.Execute(piece);
                }
            }
        }

        private void CellClick(object sender, MouseButtonEventArgs e)
        {
            Border clickedCell = sender as Border;
            if (clickedCell != null)
            {
                CellViewModel cell = clickedCell.DataContext as CellViewModel;
                if (cell != null)
                {
                    ((BoardViewModel)DataContext).CellClickCommand.Execute(cell);
                }
            }
        }

        private void SwitchButtonClick(object sender, RoutedEventArgs e)
        {
            ((BoardViewModel)DataContext).SwitchTurn();
        }

        private void NewGameButtonClick(object sender, RoutedEventArgs e)
        {
            ((BoardViewModel)DataContext).NewGameCommand.Execute(null);
        }

        private void SaveGameButtonClick(object sender, RoutedEventArgs e)
        {
            ((BoardViewModel)DataContext).SaveGameCommand.Execute(null);
        }

        private void LoadGameButtonClick(object sender, RoutedEventArgs e)
        {
            ((BoardViewModel)DataContext).LoadGameCommand.Execute(null);
        }

        private void StatisticsButtonClick(object sender, RoutedEventArgs e)
        {
            ((BoardViewModel)DataContext).StatisticsCommand.Execute(null);
        }

        private void AboutButtonClick(object sender, RoutedEventArgs e)
        {
            ((BoardViewModel)DataContext).AboutCommand.Execute(null);
        }
    }
}
