using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Checkers.MVVM.Model;

namespace Checkers.MVVM.ViewModel
{
    public class PieceViewModel : Core.ViewModel
    {
        private Brush _color;
        public Brush Color
        {
            get { return _color; }
            set
            {
                _color = value;
                OnPropertyChanged("Color");
            }
        }

        private int _row;

        public int Row
        {
            get { return _row; }
            set
            {
                _row = value;
                OnPropertyChanged("Row");
            }
        }

        private int _column;

        public int Column
        {
            get { return _column; }
            set
            {
                _column = value;
                OnPropertyChanged("Column");
            }
        }

        private Visibility _king;

        public Visibility King
        {
            get { return _king; }
            set
            {
                _king = value;
                OnPropertyChanged("King");
            }
        }

        public PieceViewModel(Piece piece)
        {
            Row = piece.Row;
            Column = piece.Column;
            Color = piece.Color == PieceColor.Red ? Brushes.Red : Brushes.White;
            King = piece.IsKing ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}
