using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using WpfMVVM.Annotations;

namespace WpfMVVM.Model
{
    public class Board : INotifyPropertyChanged
    {
        public static readonly int GridSize = 3;

        private List<Cell> _cells;
        private Cell _selectedCell;

        public Cell SelectedCell
        {
            get => _selectedCell;
            set
            {
                _selectedCell = value;
                OnPropertyChanged(nameof(SelectedCell));
            }
        }

        public List<Cell> Cells
        {
            get => _cells;
            set
            {
                _cells = value;
                OnPropertyChanged(nameof(Cells));
            }
        }

        public Board()
        {
            Cells = new List<Cell>(GridSize*GridSize);
            for (int row = 0; row < GridSize; ++row)
            for (int line = 0; line < GridSize; ++line)
                Cells.Add(new Cell(row, line, PlayerType.None));
        }

        public Board(Board source)
        {
            if (source == null)
            {
                Cells = new List<Cell>();
                return;
            }
            Cells = new List<Cell>(GridSize * GridSize);
            for (int i = 0; i < source.Cells.Count; ++i)
                Cells.Add(new Cell(source.Cells[i]));
            SelectedCell = source.SelectedCell == null ? null : new Cell(source.SelectedCell);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
