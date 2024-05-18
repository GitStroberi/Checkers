using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Checkers.Core;

namespace Checkers.MVVM.Model
{
    public class Board : ObservableObject
    {
        private Piece[,] _pieces;

        public Piece[,] Pieces
        {
            get { return _pieces; }
            set { _pieces = value; OnPropertyChanged(); }
        }

        public Board()
        {
            _pieces = new Piece[8, 8];
            InitializeBoard();
        }

        private void InitializeBoard()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if ((i + j) % 2 != 0)
                    {
                        if (i < 3)
                        {
                            _pieces[i, j] = new Piece(PieceColor.Red, i, j);
                        }
                        else if (i > 4)
                        {
                            _pieces[i, j] = new Piece(PieceColor.White, i, j);
                        }
                    }
                }
            }
        }

        //testing initialization by printing the board
        public void PrintBoard()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (_pieces[i, j] != null)
                    {
                        Console.Write(_pieces[i, j].Color == PieceColor.Red ? "R" : "W");
                    }
                    else
                    {
                        Console.Write(" ");
                    }
                }
                Console.WriteLine();
            }
        }

        public Piece GetPiece(int row, int column)
        {
            return _pieces[row, column];
        }

        public void RemovePiece(int row, int column)
        {
            _pieces[row, column] = null;
            OnPropertyChanged("Pieces");
        }

        public void MovePiece(int oldRow, int oldColumn, int newRow, int newColumn)
        {
            _pieces[newRow, newColumn] = _pieces[oldRow, oldColumn];
            _pieces[newRow, newColumn].Row = newRow;
            _pieces[newRow, newColumn].Column = newColumn;
            _pieces[oldRow, oldColumn] = null;
            OnPropertyChanged("Pieces");
        }
    }
}
