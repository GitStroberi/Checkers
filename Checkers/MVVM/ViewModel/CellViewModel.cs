using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Checkers.MVVM.ViewModel
{
    public class CellViewModel : Core.ViewModel
    {
        private Brush _background;

        public Brush Background
        {
            get { return _background; }
            set
            {
                _background = value;
                OnPropertyChanged("Background");
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
    }
}
