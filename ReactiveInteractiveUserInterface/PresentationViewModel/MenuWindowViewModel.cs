using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TP.ConcurrentProgramming.Presentation.Model;
using TP.ConcurrentProgramming.Presentation.ViewModel;
using TP.ConcurrentProgramming.Presentation.ViewModel.MVVMLight;

namespace TP.ConcurrentProgramming.PresentationViewModel
{
    public class MenuWindowViewModel : ViewModelBase
    {


        public Action<int, double>? StartMainWindowRequested { get; set; }

        #region Constructor

        public MenuWindowViewModel()
        {
            ConfirmCommand = new RelayCommand(OnConfirm);
        }

        #endregion
        #region Properties

        private int _ballsCount;
        public int BallsCount
        {
            get => _ballsCount;
            set => Set(ref _ballsCount, value);
        }

        private double _ballSize;
        public double BallSize
        {
            get => _ballSize;
            set => Set(ref _ballSize, value);
        }

        private double _fieldHeight;
        public double FieldHeight
        {
            get => _fieldHeight;
            set => Set(ref _fieldHeight, value);
        }

        private double _fieldWidth;
        public double FieldWidth
        {
            get => _fieldWidth;
            set => Set(ref _fieldWidth, value);
        }

        #endregion

        #region Command

        public ICommand ConfirmCommand { get; }

        private void OnConfirm()
        {
            StartMainWindowRequested?.Invoke(BallsCount, BallSize);
        }

        #endregion

    }
}
