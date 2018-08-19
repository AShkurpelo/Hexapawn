using System;
using System.Collections.Generic;

namespace WpfMVVM.Model
{
    public class GameLogic
    {
        #region Fields: Private

        private readonly GameState _state;
        private readonly GameStateDeterminator _stateDeterminator;
        private readonly PlayerMoveLogic _moveLogic;
        private PlayerType _AIPlayer;

        #endregion

        #region Properties: Public

        public Board Board => _state.Board;

        public PlayerType AIPlayer => _AIPlayer;

        public AIDifficultyLevel AILevel
        {
            get => _state.AILevel;
            set => _state.AILevel = value;
        }

        #endregion

        #region Events: Public

        public event EventHandler<PlayerType> GameEndEvent;

        #endregion

        #region Constructors: Public

        public GameLogic()
        {
            _state = new GameState();
            _stateDeterminator = new GameStateDeterminator(_state);
            _moveLogic = new PlayerMoveLogic(_state);
            _moveLogic.OnMoveMakedEvent += OnMoveMaked;

            NewGame();
        }

        #endregion

        #region Methods: Private

        private void ResetGameBoard()
        {
            _state.Board.Cells[0].Player = PlayerType.Two;
            _state.Board.Cells[1].Player = PlayerType.Two;
            _state.Board.Cells[2].Player = PlayerType.Two;

            _state.Board.Cells[3].Player = PlayerType.None;
            _state.Board.Cells[4].Player = PlayerType.None;
            _state.Board.Cells[5].Player = PlayerType.None;

            _state.Board.Cells[6].Player = PlayerType.One;
            _state.Board.Cells[7].Player = PlayerType.One;
            _state.Board.Cells[8].Player = PlayerType.One;
            _state.Board.SelectedCell = null;
        }

        private void ResetPlayers()
        {
            _state.PlayerCells = new Dictionary<PlayerType, HashSet<Cell>>(2)
            {
                [PlayerType.One] =
                    new HashSet<Cell> {_state.Board.Cells[6], _state.Board.Cells[7], _state.Board.Cells[8]},
                [PlayerType.Two] =
                    new HashSet<Cell> {_state.Board.Cells[0], _state.Board.Cells[1], _state.Board.Cells[2]}
            };
            _state.CurrentPlayer = PlayerType.One;
            _AIPlayer = PlayerType.Two;
        }

        private void OnMoveMaked(object sender, EventArgs eventArgs)
        {
            if (_stateDeterminator.IsTerminalState())
            {
                EndGame();
            }
            else if (_state.CurrentPlayer == _AIPlayer)
            {
                _moveLogic.MakeAIMove();
            }
        }

        private void EndGame()
        {
            GameEndEvent?.Invoke(this, _stateDeterminator.Winner);
        }

        #endregion

        #region Methods: Public

        public void NewGame()
        {
            ResetGameBoard();
            ResetPlayers();
        }

        public void SelectCell(Cell cell)
        {
            _moveLogic.SelectCell(cell);
        }

        #endregion
    }
}
