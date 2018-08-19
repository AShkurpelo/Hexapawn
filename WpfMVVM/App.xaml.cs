using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using WpfMVVM.Model;
using WpfMVVM.ViewModel;

namespace WpfMVVM
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var gameLogic = new GameLogic();
            var gameViewModel = new GameViewModel(gameLogic);
            var win = new MainWindow {DataContext = gameViewModel}; 

            win.Show();
        }
    }
}
