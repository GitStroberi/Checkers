using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Checkers.Core;

namespace Checkers.MVVM.Model
{
    public enum PieceColor
    {
        Red,
        White
    }

    public class Piece : ObservableObject
    {
        public PieceColor Color { get; set; }

        public bool IsKing { get; set; }
        
        public int Row { get; set; }

        public int Column { get; set; }

        public Piece(PieceColor color, int row, int column, bool isKing = false)
        {
            Color = color;
            Row = row;
            Column = column;
            IsKing = isKing;
        }
    }
}
