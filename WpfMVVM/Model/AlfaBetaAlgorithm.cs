using System;

namespace WpfMVVM.Model
{
    class AlfaBetaAlgorithm
    {
        private readonly GameState _state;
        private readonly PlayerMoveLogic _moveLogic;
        private readonly int _baseDepth;

        public AlfaBetaAlgorithm(GameState state)
        {
            _state = state;
            _moveLogic = new PlayerMoveLogic();
            _baseDepth = (int) _state.AILevel;
        }

        public Move GetNextAIMove()
        {
            var moves = _moveLogic.GetValidPlayerMoves(_state, _state.CurrentPlayer);
            var bestScore = int.MaxValue;
            Move bestMove = null;
            var alpha = int.MinValue;
            var beta = int.MaxValue;
            foreach (var move in moves)
            {
                var score = GetMoveScore(_state.Copy(), move, false, _baseDepth, alpha, beta);
                if (score < bestScore)
                {
                    bestMove = move;
                    bestScore = score;
                }
            }
            return bestMove;
        }

        private int GetMoveScore(GameState state, Move move, bool isMin, int depth, int alpha, int beta)
        {
            if (depth == 0)
                return 0;
            var moveLogic = new PlayerMoveLogic(state);
            moveLogic.MakeMove(move);

            var stateDeterminator = new GameStateDeterminator(state);
            if (stateDeterminator.IsTerminalState())
            {
                return GetWinnerScore(stateDeterminator.Winner);
            }

            var moves = moveLogic.GetValidPlayerMoves(state, state.CurrentPlayer);
            var bestScore = isMin ? int.MaxValue : int.MinValue;
            foreach (var curMove in moves)
            {
                var score = GetMoveScore(state.Copy(), curMove, !isMin, depth - 1, alpha, beta);
                if (isMin && score < bestScore || !isMin && score > bestScore)
                {
                    bestScore = score;
                }

                if (isMin)
                {
                    beta = Math.Min(beta, score);
                }
                else
                {
                    alpha = Math.Max(alpha, score);
                }
                if (alpha > beta)
                    break;
            }
            return bestScore;
        }

        private int GetWinnerScore(PlayerType winner)
        {
            return winner == PlayerType.One ? 1 : -1;
        }
    }
}
