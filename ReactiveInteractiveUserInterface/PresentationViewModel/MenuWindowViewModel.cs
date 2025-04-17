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
    public class MenuWindowViewModel : ViewModelBase, IDisposable
    {


        //public MenuWindowViewModel() : this(null)
        //{
        //    
        //}

        internal MenuWindowViewModel(ModelAbstractApi modelLayerAPI, MainWindowViewModel mainWindowVM)
        {
            ConfirmCommand = new RelayCommand(OnConfirm);
        }

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
            //MainWindowViewModel viewModel = (MainWindowViewModel)DataContext;
            //var mainWindowViewModel = (MainWindowViewModel)DataContext;
            //
            //mainWindowViewModel.Start(BallsCount, BallSize);
        }

        #endregion



        #region IDisposable

        protected virtual void Dispose(bool disposing)
        {
            if (!Disposed)
            {
                if (disposing)
                {
                    Observer.Dispose();
                    ModelLayer.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                Disposed = true;
            }
        }

        public void Dispose()
        {
            if (Disposed)
                throw new ObjectDisposedException(nameof(MainWindowViewModel));
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion IDisposable

        #region private

        private IDisposable Observer = null;
        private ModelAbstractApi ModelLayer;
        private bool Disposed = false;



        #endregion private

    }
}
