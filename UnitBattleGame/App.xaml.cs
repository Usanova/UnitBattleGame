using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using UnitBattleGame.ViewModels;

namespace UnitBattleGame
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public DisplayRootRegistry displayRootRegistry = new DisplayRootRegistry();

        public App()
        {
            displayRootRegistry.RegisterWindowType<GameDeskViewModel, MainWindow>();
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var gameDeskViewModel = new GameDeskViewModel();

            await displayRootRegistry.ShowModalPresentation(gameDeskViewModel);

            Shutdown();
        }
    }
}
