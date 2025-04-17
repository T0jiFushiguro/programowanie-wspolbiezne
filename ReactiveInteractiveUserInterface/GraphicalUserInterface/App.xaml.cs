//__________________________________________________________________________________________
//
//  Copyright 2024 Mariusz Postol LODZ POLAND.
//
//  To be in touch join the community by pressing the `Watch` button and to get started
//  comment using the discussion panel at
//  https://github.com/mpostol/TP/discussions/182
//__________________________________________________________________________________________

using System;
using System.Windows;
using TP.ConcurrentProgramming.Presentation.Model;
using TP.ConcurrentProgramming.Presentation.ViewModel;
using TP.ConcurrentProgramming.PresentationViewModel;

namespace TP.ConcurrentProgramming.PresentationView
{
  /// <summary>
  /// Interaction logic for App.xaml
  /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var menuWindow = new MenuWindow();
            var modelView = new MenuWindowViewModel();
            menuWindow.DataContext = modelView;

            modelView.StartMainWindowRequested = (ballsCount, ballSize) =>
            {
                var mainWindow = new MainWindow();
                MainWindowViewModel mainWindowViewModel = new MainWindowViewModel();
                mainWindow.DataContext = mainWindowViewModel;
                mainWindowViewModel.Start(ballsCount, ballSize);
                mainWindow.Show();

                menuWindow.Close();
            };

            menuWindow.Show();

        }
    }
}