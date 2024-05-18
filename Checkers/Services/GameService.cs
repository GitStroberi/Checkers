using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using Checkers.MVVM.Model;
using Checkers.MVVM.ViewModel;

namespace Checkers.Services
{
    public class GameService : Core.ViewModel
    {
        public int RedPieces { get; set; } = 12;

        public int WhitePieces { get; set; } = 12;

        public bool PlayerTurn { get; set; }
        public bool PlayerMoved { get; set; }
        public bool PlayerJumped { get; set; }
        public bool GameOver { get; set; }
        public bool JumpsEnabled { get; set; }
        public PieceViewModel CurrentPiece { get; set; }

        void AddIfValid(int newRow, int newCol, List<Tuple<int, int>> validMoves, Board board)
        {
            if (newRow >= 0 && newRow <= 7 && newCol >= 0 && newCol <= 7 && board.GetPiece(newRow, newCol) == null)
            {
                validMoves.Add(new Tuple<int, int>(newRow, newCol));
            }
        }

        public List<Tuple<int, int>> GetValidMoves(Piece piece, Board board, ObservableCollection<PieceViewModel> pieces)
        {
            List<Tuple<int, int>> validMoves = new List<Tuple<int, int>>();
            if (!piece.IsKing)
            {
                if(piece.Color == PieceColor.Red)
                {
                    validMoves = GetValidMovesRed(piece, board);
                }
                else
                {
                    validMoves = GetValidMovesWhite(piece, board);
                }
            }
            else
            {
                List<Tuple<int, int>> validMovesWhite = GetValidMovesWhite(piece, board);
                List<Tuple<int, int>> validMovesRed = GetValidMovesRed(piece, board);
                validMoves.AddRange(validMovesWhite);
                validMoves.AddRange(validMovesRed);
            }
            return validMoves;
        }

        private List<Tuple<int, int>> GetValidMovesWhite(Piece piece, Board board)
        {
            List<Tuple<int, int>> validMoves = new List<Tuple<int, int>>();
            if(PlayerJumped && JumpsEnabled && piece.Column != CurrentPiece.Column && piece.Row != CurrentPiece.Row)
            {
                MessageBox.Show("You must use the same piece to jump again, or end your turn.");
                return validMoves;
            }

            int row = piece.Row;
            int col = piece.Column;

            if (row > 0)
            {
                if (!PlayerJumped)
                {
                    AddIfValid(row - 1, col - 1, validMoves, board);
                    AddIfValid(row - 1, col + 1, validMoves, board);
                }

                if (col > 1 && row > 1 && board.GetPiece(row - 1, col - 1) != null && board.GetPiece(row - 1, col - 1).Color != piece.Color && board.GetPiece(row - 2, col - 2) == null)
                {
                    AddIfValid(row - 2, col - 2, validMoves, board);
                }

                if (col < 6 && row > 1 && board.GetPiece(row - 1, col + 1) != null && board.GetPiece(row - 1, col + 1).Color != piece.Color && board.GetPiece(row - 2, col + 2) == null)
                {
                    AddIfValid(row - 2, col + 2, validMoves, board);
                }
            }

            return validMoves;
        }

        private List<Tuple<int, int>> GetValidMovesRed(Piece piece, Board board)
        {
            List<Tuple<int, int>> validMoves = new List<Tuple<int, int>>();
            if (PlayerJumped && JumpsEnabled && piece.Column != CurrentPiece.Column && piece.Row != CurrentPiece.Row)
            {
                MessageBox.Show("You must use the same piece to jump again, or end your turn.");
                return validMoves;
            }

            int row = piece.Row;
            int col = piece.Column;

            if (row < 7)
            {
                if (!PlayerJumped)
                {
                    AddIfValid(row + 1, col - 1, validMoves, board);
                    AddIfValid(row + 1, col + 1, validMoves, board);
                }

                if (col > 1 && row < 6 && board.GetPiece(row + 1, col - 1) != null && board.GetPiece(row + 1, col - 1).Color != piece.Color && board.GetPiece(row + 2, col - 2) == null)
                {
                    AddIfValid(row + 2, col - 2, validMoves, board);
                }

                if (col < 6 && row < 6 && board.GetPiece(row + 1, col + 1) != null && board.GetPiece(row + 1, col + 1).Color != piece.Color && board.GetPiece(row + 2, col + 2) == null)
                {
                    AddIfValid(row + 2, col + 2, validMoves, board);
                }
            }

            return validMoves;
        }

        public void CapturePiece(ObservableCollection<PieceViewModel> pieces, Piece piece, Board board, int rowOffset, int colOffset)
        {
            int targetRow = piece.Row + rowOffset;
            int targetCol = piece.Column + colOffset;
            Piece targetPiece = board.GetPiece(targetRow, targetCol);
            if (targetPiece != null)
            {
                board.RemovePiece(targetRow, targetCol);
            }
            var targetPieceViewModel = pieces.FirstOrDefault(p => p.Row == targetRow && p.Column == targetCol);
            if (targetPieceViewModel != null)
            {
                pieces.Remove(targetPieceViewModel);
            }
        }

        public void UpdateBoard(Board board, CellViewModel cell, PieceViewModel currentPiece, ObservableCollection<PieceViewModel> pieces)
        {
            if(GameOver)
            {
                return;
            }
            int rowDiff = cell.Row - currentPiece.Row;
            int colDiff = cell.Column - currentPiece.Column;

            if (Math.Abs(rowDiff) == 2 && Math.Abs(colDiff) == 2)
            {
                CapturePiece(pieces, board.GetPiece(currentPiece.Row, currentPiece.Column), board, rowDiff / 2, colDiff / 2);
                if (currentPiece.Color == Brushes.Red)
                {
                    WhitePieces--;
                }
                else
                {
                    RedPieces--;
                }
                if (JumpsEnabled)
                {
                    PlayerJumped = true;
                    CurrentPiece = currentPiece;
                }
            }

            board.MovePiece(currentPiece.Row, currentPiece.Column, cell.Row, cell.Column);
            PlayerMoved = true;
            currentPiece.Row = cell.Row;
            currentPiece.Column = cell.Column;

            if((currentPiece.Color == Brushes.Red && cell.Row == 7) || (currentPiece.Color == Brushes.White && cell.Row == 0))
            {
                currentPiece.King = Visibility.Visible;
                board.GetPiece(cell.Row, cell.Column).IsKing = true;
            }

            if(RedPieces == 0)
            {
                MessageBox.Show("White wins!");
                Tuple<int, int, int> wins = HelperService.ReadWins();
                if(WhitePieces > wins.Item3)
                {
                    //set the record pieces remaining
                    HelperService.SetRecordPieces(WhitePieces);
                }
                HelperService.IncrementWhiteWins();

                GameOver = true;
            }
            else if(WhitePieces == 0)
            {
                MessageBox.Show("Red wins!");
                Tuple<int, int, int> wins = HelperService.ReadWins();
                if (RedPieces > wins.Item3)
                {
                    //set the record pieces remaining
                    HelperService.SetRecordPieces(RedPieces);
                }
                HelperService.IncrementRedWins();

                GameOver = true;
            }
        }

        public void ShowMoves(List<Tuple<int, int>> validMoves, ObservableCollection<CellViewModel> cells)
        {
            foreach (Tuple<int, int> move in validMoves)
            {
                int row = move.Item1;
                int col = move.Item2;

                int index = row * 8 + col;

                if(index >= 0 && index < cells.Count)
                {
                    cells[index].Background = Brushes.Green;
                }
            }
        }

        public void SwitchTurn()
        {
            PlayerMoved = false;
            PlayerJumped = false;
            PlayerTurn = !PlayerTurn;
            CurrentPiece = null;
        }
    }
}
