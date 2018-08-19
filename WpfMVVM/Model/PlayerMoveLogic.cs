using System;
using System.Collections.Generic;
using System.Linq;

namespace WpfMVVM.Model
{
    class PlayerMoveLogic
    {
        #region Fields: Private

        private readonly GameState _gameState;

        #endregion

        #region Properties: Private

        private PlayerType CurrentPlayer => _gameState.CurrentPlayer;

        #endregion

        #region Events: Public

        public event EventHandler OnMoveMakedEvent;

        #endregion

        #region Constructors: Public

        public PlayerMoveLogic()
        {
        }

        public PlayerMoveLogic(GameState gameState)
        {
            _gameState = gameState;
        }

        #endregion

        #region Methods: Private

        private bool CanSelectCell(Cell cell)
        {
            if (cell == null)
                return false;
            if (_gameState.Board.SelectedCell == null)
                return cell.Player == _gameState.CurrentPlayer;
            if (_gameState.Board.SelectedCell.Equals(cell))
                return true;
            return cell.Player == CurrentPlayer || CanMakeMove(cell);
        }

        private bool ValidateMoveDistance(Cell to)
        {
            var from = _gameState.Board.SelectedCell;
            var columnDelta = from.Column - to.Column;
            var rowDelta = from.Row - to.Row;
            if (CurrentPlayer == PlayerType.Two)
                rowDelta *= -1;
            return rowDelta > 0 && rowDelta < 2
                   && Math.Abs(columnDelta) < 2;
        }

        private bool CanMakeMove(Cell next)
        {
            if (!ValidateMoveDistance(next))
                return false;

            var current = _gameState.Board.SelectedCell;
            if (next.Column == current.Column)
            {
                return next.Player == PlayerType.None;
            }
            return next.Player != PlayerType.None && next.Player != CurrentPlayer;
        }

        private void OnMoveMaked(Cell newCell)
        {
            _gameState.LastMove = new Move(_gameState.Board.SelectedCell, newCell);
            _gameState.PlayerCells[_gameState.CurrentPlayer].Remove(_gameState.Board.SelectedCell);
            _gameState.PlayerCells[_gameState.CurrentPlayer].Add(newCell);
            _gameState.Board.SelectedCell = null;

            ChangePlayer();

            OnMoveMakedEvent?.Invoke(this, EventArgs.Empty);
        }

        private void ChangePlayer()
        {
            _gameState.CurrentPlayer = _gameState.CurrentPlayer == PlayerType.One ? PlayerType.Two : PlayerType.One;
        }

        private List<Move> GetValidMovesFromCell(GameState state, Cell cell)
        {
            var moves = new List<Move>();
            var rowDirection = cell.Player == PlayerType.One ? -1 : 1;

            var forwardCell = state.Board.Cells.FirstOrDefault(c => c.Column == cell.Column && c.Row == cell.Row + rowDirection);
            var forwardLeftCell = cell.Column > 0 ? state.Board.Cells.FirstOrDefault(c => c.Column == cell.Column - 1 && c.Row == cell.Row + rowDirection) : null;
            var forwardRigthCell = cell.Column < 2 ? state.Board.Cells.FirstOrDefault(c => c.Column == cell.Column + 1 && c.Row == cell.Row + rowDirection) : null;

            if (forwardCell != null && forwardCell.Player == PlayerType.None)
                moves.Add(new Move(cell, forwardCell));
            if (forwardLeftCell != null && forwardLeftCell.Player != PlayerType.None && forwardLeftCell.Player != cell.Player)
                moves.Add(new Move(cell, forwardLeftCell));
            if (forwardRigthCell != null && forwardRigthCell.Player != PlayerType.None && forwardRigthCell.Player != cell.Player)
                moves.Add(new Move(cell, forwardRigthCell));

            return moves;
        }

        private void MakeMove(Cell next)
        {
            if (next.Player != PlayerType.None)
                _gameState.PlayerCells[next.Player].Remove(next);
            _gameState.Board.SelectedCell.Player = PlayerType.None;
            next.Player = CurrentPlayer;
            OnMoveMaked(next);
        }

        private Move ToCurrentState(Move sourceMove)
        {
            var from = _gameState.Board.Cells[sourceMove.From.Column + sourceMove.From.Row * 3];
            var to = _gameState.Board.Cells[sourceMove.To.Column + sourceMove.To.Row * 3];
            return new Move(from, to);
        }

        #endregion

        #region Methods: Public

        public void MakeMove(Move move)
        {
            var currentMove = ToCurrentState(move);
            _gameState.Board.SelectedCell = _gameState.Board.Cells[currentMove.From.Column + currentMove.From.Row * 3];
            MakeMove(currentMove.To);
        }

        public void SelectCell(Cell cell)
        {
            if (cell == null)
                return;

            if (!CanSelectCell(cell))
                return;

            if (_gameState.Board.SelectedCell == null || !_gameState.Board.SelectedCell.Equals(cell) && cell.Player == CurrentPlayer)
                _gameState.Board.SelectedCell = cell;
            else if (_gameState.Board.SelectedCell.Equals(cell))
                _gameState.Board.SelectedCell = null;
            else
                MakeMove(cell);
        }

        public List<Move> GetValidPlayerMoves(GameState state, PlayerType player)
        {
            var moves = new List<Move>();
            foreach (var cell in state.PlayerCells[player])
               moves.AddRange(GetValidMovesFromCell(state, cell));
            return moves;
        }

        public void MakeAIMove()
        {
            var alfaBeta = new AlfaBetaAlgorithm(_gameState.Copy());
            var aiMove = alfaBeta.GetNextAIMove();
            MakeMove(aiMove);
        }

        #endregion
    }
}
