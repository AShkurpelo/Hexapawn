using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfMVVM.Model
{
    class GameStateDeterminator
    {
        #region Fields: Private

        private GameState _game;
        private PlayerType _winner;

        #endregion

        #region Properties: Public

        public PlayerType Winner => _winner;

        #endregion

        #region Constructors: Public

        public GameStateDeterminator(GameState game)
        {
            _game = game;
            _winner = PlayerType.None;
        }

        #endregion

        #region Methods: Private

        private bool PlayerRichBoardEnd()
        {
            var endRow = _game.LastMove.To.Player == PlayerType.One ? 0 : 2;
            if (_game.LastMove.To.Row == endRow)
            {
                _winner = _game.LastMove.To.Player;
                return true;
            }
            return false;
        }

        private bool PlayerBitsOponentCells()
        {
            var playersWithCells = _game.PlayerCells
                .Where(keyValue => keyValue.Value.Count != 0)
                .Select(keyValue => keyValue.Key);
            if (playersWithCells.Count() == 1)
            {
                _winner = playersWithCells.First();
                return true;
            }
            return false;
        }

        private bool PlayerCantMakeMove()
        {
            var moveLogic = new PlayerMoveLogic(_game);
            var moves = moveLogic.GetValidPlayerMoves(_game, _game.CurrentPlayer);
            if (moves.Count == 0)
            {
                _winner = _game.LastMove.To.Player;
                return true;
            }
            return false;
        }

        #endregion

        #region Methods: Public

        public bool IsTerminalState()
        {
            return PlayerRichBoardEnd()
                   || PlayerBitsOponentCells()
                   || PlayerCantMakeMove();
        }

        #endregion
    }
}
