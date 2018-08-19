using System;
using System.Windows;

namespace WpfMVVM.ViewModel
{
    class MessageBoxService
    {
        public bool StartNewAfterGameEndMessage(string winnerString)
        {
            var message = String.Concat(winnerString, "\r\n\r\n", Properties.Resources.StartNewGameQuestion);
            return MessageBox.Show(message, "Game over", MessageBoxButton.YesNo) == MessageBoxResult.Yes;
        }
    }
}
