using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using TP.ConcurrentProgramming.Presentation.Model;
using TP.ConcurrentProgramming.Presentation.ViewModel;
using TP.ConcurrentProgramming.PresentationViewModel;

namespace TP.ConcurrentProgramming.PresentationViewModelTest
{
    [TestClass]
    public class MenuWindowViewModelUnitTest
    {

        [TestMethod]
        public void Default_Values_Are_Correct()
        {
            var viewModel = new MenuWindowViewModel();

            Assert.AreEqual(0, viewModel.BallsCount);
            Assert.AreEqual(0.0, viewModel.BallSize);
        }

        [TestMethod]
        public void Setting_BallsCount_Raises_PropertyChanged()
        {
            var viewModel = new MenuWindowViewModel();
            bool propertyChangedRaised = false;

            viewModel.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(viewModel.BallsCount))
                    propertyChangedRaised = true;
            };

            viewModel.BallsCount = 10;

            Assert.IsTrue(propertyChangedRaised);
            Assert.AreEqual(10, viewModel.BallsCount);
        }

        [TestMethod]
        public void Setting_BallSize_Raises_PropertyChanged()
        {
            var viewModel = new MenuWindowViewModel();
            bool propertyChangedRaised = false;

            viewModel.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(viewModel.BallSize))
                    propertyChangedRaised = true;
            };

            viewModel.BallSize = 42.5;

            Assert.IsTrue(propertyChangedRaised);
            Assert.AreEqual(42.5, viewModel.BallSize);
        }

        [TestMethod]
        public void ConfirmCommand_Invokes_StartMainWindowRequested()
        {
            var viewModel = new MenuWindowViewModel
            {
                BallsCount = 5,
                BallSize = 15.0
            };

            int? receivedCount = null;
            double? receivedSize = null;

            viewModel.StartMainWindowRequested = (count, size) =>
            {
                receivedCount = count;
                receivedSize = size;
            };

            Assert.IsTrue(viewModel.ConfirmCommand.CanExecute(null));
            viewModel.ConfirmCommand.Execute(null);

            Assert.AreEqual(5, receivedCount);
            Assert.AreEqual(15.0, receivedSize);
        }

    }
}
