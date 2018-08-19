using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using WpfMVVM.Converter;
using WpfMVVM.Model;

namespace WpfMVVM.ViewModel
{
    public class GameViewModel
    {
        #region Fields: Private

        private readonly GameLogic _gameLogic;
        private readonly MessageBoxService _messageBoxService;

        #endregion

        #region Properties: Public

        public Board Board => _gameLogic.Board;

        #endregion

        #region Comands: Private

        private RelayCommand _selectCellCommand;
        private RelayCommand _exitCommand;
        private RelayCommand _newGameCommand;
        private RelayCommand _choseDifficultyLevelCommand;

        #endregion

        #region Comands: Public

        public ICommand SelectCellCommand => _selectCellCommand ?? (_selectCellCommand = new RelayCommand(
                                                 cell => _gameLogic.SelectCell(cell as Cell)));

        public ICommand ExitCommand => _exitCommand ?? (_exitCommand = new RelayCommand(
                                           _ => CloseGame()));

        public ICommand NewGameCommand => _newGameCommand ?? (_newGameCommand = new RelayCommand(
                                              _ => _gameLogic.NewGame()));

        public ICommand ChoseDifficultyLevelCommand =>
            _choseDifficultyLevelCommand ?? (_choseDifficultyLevelCommand = new RelayCommand(
                objValue =>
                {
                    if (!int.TryParse(objValue.ToString(), out var intValue)) return;
                    _gameLogic.AILevel = new IntToAIDifficultyLevelConverter().Convert(intValue);
                    _gameLogic.NewGame();
                }));

        #endregion

        #region Events: Public

        public event EventHandler<PlayerType> GameEndEvent
        {
            add => _gameLogic.GameEndEvent += value;
            remove => _gameLogic.GameEndEvent -= value;
        }

        #endregion

        #region Constructors: Public

        public GameViewModel(GameLogic gameLogic)
        {
            _gameLogic = gameLogic;
            _messageBoxService = new MessageBoxService();

            _gameLogic.GameEndEvent += OnGameEnd;
        }

        #endregion

        #region Methods: Private

        private void OnGameEnd(object sender, PlayerType winner)
        {
            if (_messageBoxService.StartNewAfterGameEndMessage(GetWinnerString(winner)))
            {
                _gameLogic.NewGame();
            }
            else
            {
                CloseGame();
            }
        }

        private string GetWinnerString(PlayerType winner)
        {
            return winner == _gameLogic.AIPlayer
                ? Properties.Resources.AIWinMessage
                : Properties.Resources.HumanWinMessage;
        }

        private void CloseGame()
        {
            Application.Current.Shutdown();
        }

        #endregion
    }
}