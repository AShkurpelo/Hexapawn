using System.ComponentModel;
using System.Runtime.CompilerServices;
using WpfMVVM.Annotations;

namespace WpfMVVM.Model
{
    public class Cell : INotifyPropertyChanged
    {
        private PlayerType _player;
        private readonly int _column;
        private readonly int _row;

        public PlayerType Player
        {
            get => _player;
            set
            {
                _player = value;
                OnPropertyChanged(nameof(Player));
            }
        }

        public int Column => _column;

        public int Row => _row;

        public Cell(int row, int column, PlayerType player)
        {
            _row = row;
            _column = column;
            _player = player;
        }

        public Cell(Cell source)
        {
            _row = source.Row;
            _column = source.Column;
            _player = source.Player;
        }

        public override bool Equals(object obj)
        {
            if (obj is Cell cell)
            {
                return cell.Column == Column
                       && cell.Row == Row;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Row.GetHashCode() ^ Column.GetHashCode();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
