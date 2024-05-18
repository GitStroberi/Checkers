using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Checkers.Core;
using Checkers.MVVM.Model;
using Checkers.Services;

namespace Checkers.MVVM.ViewModel
{
    public class BoardViewModel : Core.ViewModel
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

        public ObservableCollection<CellViewModel> Cells { get; set; }

        public ObservableCollection<PieceViewModel> Pieces { get; set; }

        private Board board;
        private PieceViewModel selectedPieceVM;
        private GameService gameService;
        

        public bool JumpsEnabled
        {
            set
            {
                gameService.JumpsEnabled = value;
            }
            get
            {
                return gameService.JumpsEnabled;
            }
        }

        public string CurrentTurnString { get { return gameService.PlayerTurn ? "Turn: Red" : "Turn: White"; } }

        public RelayCommand NavigateHomeCommand { get; set; }
        public RelayCommand PieceClickCommand { get; set; }
        public RelayCommand CellClickCommand { get; set; }
        public RelayCommand SwitchTurnCommand { get; set; }

        public RelayCommand NewGameCommand { get; set; }
        public RelayCommand SaveGameCommand { get; set; }
        public RelayCommand LoadGameCommand { get; set; }

        public RelayCommand StatisticsCommand { get; set; }
        public RelayCommand AboutCommand { get; set; }
        
        public BoardViewModel()
        {
            Cells = HelperService.InitializeCells();
            board = new Board();
            gameService = new GameService();
            Pieces = HelperService.InitializePieces(board);

            NavigateHomeCommand = new RelayCommand(o => { Navigation.NavigateTo<HomeViewModel>(); }, o => true);
            PieceClickCommand = new RelayCommand(ExecuteClickPiece);
            CellClickCommand = new RelayCommand(ExecuteClickCell);

            NewGameCommand = new RelayCommand(ExecuteNewGame);
            SaveGameCommand = new RelayCommand(ExecuteSaveGame);
            LoadGameCommand = new RelayCommand(ExecuteLoadGame);
            AboutCommand = new RelayCommand(ExecuteAboutCommand);
            StatisticsCommand = new RelayCommand(ExecuteStatisticsCommand);
        }

        public void ClickPiece(PieceViewModel piece)
        {
            if((!gameService.PlayerTurn && piece.Color == Brushes.Red) || (gameService.PlayerTurn && piece.Color == Brushes.White))
            {
                MessageBox.Show("Not your turn");
                return;
            }
            if(gameService.PlayerMoved && !gameService.PlayerJumped)
            {
                MessageBox.Show("You already moved");
                HelperService.ResetCells(Cells);
                return;
            }
            HelperService.ResetCells(Cells);
            selectedPieceVM = piece;
            var selectedPiece = board.GetPiece(piece.Row, piece.Column);
            List<Tuple<int, int>> validMoves = gameService.GetValidMoves(selectedPiece, board, Pieces);
            gameService.ShowMoves(validMoves, Cells);
        }

        private void ExecuteClickPiece(object obj)
        {
            ClickPiece(obj as PieceViewModel);
        }

        public void ClickCell(CellViewModel cell)
        {
            if(cell.Background != Brushes.Green)
            {
                return;
            }
            gameService.UpdateBoard(board, cell, selectedPieceVM, Pieces);
            HelperService.ResetCells(Cells);
        }

        private void ExecuteClickCell(object obj)
        {
            ClickCell(obj as CellViewModel);
        }

        public int SwitchTurn()
        {
            if (!gameService.PlayerMoved)
            {
                MessageBox.Show("You must move");
                return -1;
            }
            gameService.SwitchTurn();
            OnPropertyChanged(nameof(CurrentTurnString));
            return gameService.PlayerTurn ? 1 : 0;
        }

        public void ExecuteNewGame(object obj)
        {
            Cells = HelperService.InitializeCells();
            board = new Board();
            gameService = new GameService();
            Pieces = HelperService.InitializePieces(board);
            OnPropertyChanged(nameof(Cells));
            OnPropertyChanged(nameof(Pieces));
            OnPropertyChanged(nameof(CurrentTurnString));
            OnPropertyChanged(nameof(JumpsEnabled));

            NavigateHomeCommand = new RelayCommand(o => { Navigation.NavigateTo<HomeViewModel>(); }, o => true);
            PieceClickCommand = new RelayCommand(ExecuteClickPiece);
            CellClickCommand = new RelayCommand(ExecuteClickCell);

            NewGameCommand = new RelayCommand(ExecuteNewGame);
            SaveGameCommand = new RelayCommand(ExecuteSaveGame);
            LoadGameCommand = new RelayCommand(ExecuteLoadGame);
            AboutCommand = new RelayCommand(ExecuteAboutCommand);
            StatisticsCommand = new RelayCommand(ExecuteStatisticsCommand);
        }

        public void ExecuteSaveGame(object obj)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.Filter = "JSON files (*.json)|*.json";
            if (dlg.ShowDialog() == true)
            {
                SaveData saveData = new SaveData
                {
                    Board = board,
                    PlayerTurn = gameService.PlayerTurn,
                    PlayerMoved = gameService.PlayerMoved,
                    PlayerJumped = gameService.PlayerJumped,
                    JumpsEnabled = gameService.JumpsEnabled,
                    GameOver = gameService.GameOver,
                    RedPieces = gameService.RedPieces,
                    WhitePieces = gameService.WhitePieces
                };
                HelperService.Save(dlg.FileName, saveData);
            }
        }

        public void ExecuteLoadGame(object obj)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.Filter = "JSON files (*.json)|*.json";
            if (dlg.ShowDialog() == true)
            {
                SaveData saveData = HelperService.Load(dlg.FileName);
                board = saveData.Board;
                gameService.PlayerTurn = saveData.PlayerTurn;
                gameService.PlayerMoved = saveData.PlayerMoved;
                gameService.PlayerJumped = saveData.PlayerJumped;
                gameService.JumpsEnabled = saveData.JumpsEnabled;
                gameService.GameOver = saveData.GameOver;
                gameService.RedPieces = saveData.RedPieces;
                gameService.WhitePieces = saveData.WhitePieces;

                // Update Pieces
                Pieces.Clear();
                foreach (var piece in HelperService.InitializePieces(board))
                {
                    Pieces.Add(piece);
                }

                // Update Cells
                Cells.Clear();
                foreach (var cell in HelperService.InitializeCells())
                {
                    Cells.Add(cell);
                }

                // Notify UI that properties have changed
                OnPropertyChanged(nameof(Pieces));
                OnPropertyChanged(nameof(Cells));
                OnPropertyChanged(nameof(JumpsEnabled));
                OnPropertyChanged(nameof(CurrentTurnString));
            }
        }

        public void ExecuteStatisticsCommand(object obj)
        {
            //read the wins from the file
            Tuple<int, int, int> wins = HelperService.ReadWins();
            string statisticsMessage = "Red wins: " + wins.Item1 + "\n" +
                                       "White wins: " + wins.Item2 + "\n" +
                                       "Record pieces remaining: " + wins.Item3;

            MessageBox.Show(statisticsMessage, "Statistics", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public void ExecuteAboutCommand(object obj)
        {
            string aboutMessage = "Author: Andrei Filip\n" +
                                 "Group: 10LF321\n" +
                                 "Email: andrei-o.filip@student.unitbv.ro\n" +
                                 "Description: Checkers is a classic board game played by two players on an 8x8 board. " +
                                 "The game involves strategy and tactical thinking as players try to capture each other's pieces " +
                                 "while avoiding being captured themselves.";

            MessageBox.Show(aboutMessage, "About Checkers Game", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
