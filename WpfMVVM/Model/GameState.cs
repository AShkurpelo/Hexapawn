using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using WpfMVVM.Annotations;

namespace WpfMVVM.Model
{
    public class GameState : INotifyPropertyChanged
    {
        private Board _board;

        public Board Board
        {
            get => _board;
            set
            {
                _board = value;
                OnPropertyChanged(nameof(Board));
            }
        }

        public PlayerType CurrentPlayer { get; set; }

        public AIDifficultyLevel AILevel { get; set; }

        public Dictionary<PlayerType, HashSet<Cell>> PlayerCells { get; set; }

        public Move LastMove { get; set; }

        public GameState()
        {
            Board = new Board();
            CurrentPlayer = PlayerType.None;
            PlayerCells = null;
            LastMove = null;
            AILevel = AIDifficultyLevel.Hard;
        }

        public GameState Copy()
        {
            var copy = new GameState()
            {
                CurrentPlayer = this.CurrentPlayer,
                PlayerCells = new Dictionary<PlayerType, HashSet<Cell>> {{PlayerType.One, new HashSet<Cell>()}, {PlayerType.Two, new HashSet<Cell>()}},
                LastMove = new Move(this.LastMove.From, this.LastMove.To),
                Board = new Board(this.Board),
                AILevel = this.AILevel
            };
            foreach (var keyValuePair in PlayerCells)
            {
                foreach (var cell in keyValuePair.Value)
                {
                    copy.PlayerCells[keyValuePair.Key].Add(copy.Board.Cells[cell.Row * 3 + cell.Column]);
                }
            }
            return copy;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
