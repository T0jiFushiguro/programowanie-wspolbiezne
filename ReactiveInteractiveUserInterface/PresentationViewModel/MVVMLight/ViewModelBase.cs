//__________________________________________________________________________________________
//
//  Copyright 2024 Mariusz Postol LODZ POLAND.
//
//  To be in touch join the community by pressing the `Watch` button and to get started
//  comment using the discussion panel at
//  https://github.com/mpostol/TP/discussions/182
//__________________________________________________________________________________________

using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TP.ConcurrentProgramming.Presentation.ViewModel.MVVMLight
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged

<<<<<<< Updated upstream
<<<<<<< Updated upstream
<<<<<<< Updated upstream
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion INotifyPropertyChanged

        #region API

        /// <summary>
        /// Raises the PropertyChanged event if needed.
        /// </summary>
        /// <param name="propertyName">(optional) The name of the property that changed.
        /// The <see cref="CallerMemberName"/> allows you to obtain the method or property name of the caller to the method.
        /// </param>
        protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Metoda do ustawiania właściwości i powiadamiania o zmianach
        protected bool Set<T>(ref T field, T value, [CallerMemberName] string propertyName = "")
        {
            if (!EqualityComparer<T>.Default.Equals(field, value))
            {
                field = value;
                RaisePropertyChanged(propertyName);
                return true;
            }
            return false;
        }
        #endregion API
    }
=======
=======
>>>>>>> Stashed changes
=======
>>>>>>> Stashed changes
        // Metoda do ustawiania właściwości i powiadamiania o zmianach
     protected bool Set<T>(ref T field, T value, [CallerMemberName] string propertyName = "")
     {
         if (!EqualityComparer<T>.Default.Equals(field, value))
         {
             field = value;
             RaisePropertyChanged(propertyName);
             return true;
         }
         return false;
     }
     #endregion API
  }
>>>>>>> Stashed changes
}