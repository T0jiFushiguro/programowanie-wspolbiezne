//__________________________________________________________________________________________
//
//  Copyright 2024 Mariusz Postol LODZ POLAND.
//
//  To be in touch join the community by pressing the `Watch` button and to get started
//  comment using the discussion panel at
//  https://github.com/mpostol/TP/discussions/182
//__________________________________________________________________________________________

using System.Windows;
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

            var MenuWindow = new MenuWindow();
            MenuWindow.DataContext = new MenuWindowViewModel();
            MenuWindow.Show();

            Current.MainWindow = MenuWindow;
        }
    }
}