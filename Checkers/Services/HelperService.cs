using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using Checkers.MVVM.Model;
using Checkers.MVVM.ViewModel;
using Newtonsoft.Json;

namespace Checkers.Services
{
    public class HelperService
    {
        public static ObservableCollection<PieceViewModel> InitializePieces(Board board)
        {
            ObservableCollection<PieceViewModel> pieces = new ObservableCollection<PieceViewModel>();
            
            for(int i = 0; i < board.Pieces.GetLength(0); i++)
            {
                for(int j = 0; j < board.Pieces.GetLength(1); j++)
                {
                    if (board.Pieces[i, j] != null)
                    {
                        pieces.Add(new PieceViewModel(board.Pieces[i, j]));
                    }
                }
            }
            return pieces;
        }

        public static ObservableCollection<CellViewModel> InitializeCells()
        {
            ObservableCollection<CellViewModel> Cells = new ObservableCollection<CellViewModel>();

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Brush background = (i + j) % 2 == 0 ? new SolidColorBrush(Color.FromRgb(255, 206, 158)) : new SolidColorBrush(Color.FromRgb(209, 139, 71));
                    Cells.Add(new CellViewModel { Row = i, Column = j, Background = background });
                }
            }
            return Cells;
        }

        public static void ResetCells(ObservableCollection<CellViewModel> cells)
        {
            foreach (var cell in cells)
            {
                cell.Background = (cell.Row + cell.Column) % 2 == 0 ? new SolidColorBrush(Color.FromRgb(255, 206, 158)) : new SolidColorBrush(Color.FromRgb(209, 139, 71));
            }
        }

        public static void Save(string filename, SaveData saveData)
        {
            string json = JsonConvert.SerializeObject(saveData);
            System.IO.File.WriteAllText(filename, json);
        }

        public static SaveData Load(string filename)
        {
            string json = System.IO.File.ReadAllText(filename);
            return JsonConvert.DeserializeObject<SaveData>(json);
        }

        //save win statistics like redwins, whitewins and recordpieces to a json file named statistics.json
        public static void SaveWins(int redWins, int whiteWins, int recordPieces)
        {
            WinsData winsData = new WinsData
            {
                RedWins = redWins,
                WhiteWins = whiteWins,
                RecordPieces = recordPieces
            };

            string json = JsonConvert.SerializeObject(winsData);
            System.IO.File.WriteAllText("statistics.json", json);
        }

        //read win statistics from statistics.json
        public static Tuple<int, int, int> ReadWins()
        {
            string filename = "statistics.json";

            if (!System.IO.File.Exists(filename))
            {
                WinsData defaultWinsData = new WinsData
                {
                    RedWins = 0,
                    WhiteWins = 0,
                    RecordPieces = 0
                };

                string defaultJson = JsonConvert.SerializeObject(defaultWinsData);
                System.IO.File.WriteAllText(filename, defaultJson);
            }

            string json = System.IO.File.ReadAllText(filename);
            WinsData winsData = JsonConvert.DeserializeObject<WinsData>(json);
            return new Tuple<int, int, int>(winsData.RedWins, winsData.WhiteWins, winsData.RecordPieces);
        }

        public static int IncrementRedWins()
        {
            Tuple<int, int, int> wins = ReadWins();
            SaveWins(wins.Item1+1, wins.Item2, wins.Item3);
            return wins.Item1;
        }
        public static int IncrementWhiteWins()
        {
            Tuple<int, int, int> wins = ReadWins();
            SaveWins(wins.Item1, wins.Item2+1, wins.Item3);
            return wins.Item2;
        }

        public static int SetRecordPieces(int recordPieces)
        {
            Tuple<int, int, int> wins = ReadWins();
            SaveWins(wins.Item1, wins.Item2, recordPieces);
            return recordPieces;
        }
    }
}
