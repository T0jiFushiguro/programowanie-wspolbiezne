//____________________________________________________________________________________________________________________________________
//
//  Copyright (C) 2024, Mariusz Postol LODZ POLAND.
//
//  To be in touch join the community by pressing the `Watch` button and get started commenting using the discussion panel at
//
//  https://github.com/mpostol/TP/discussions/182
//
//_____________________________________________________________________________________________________________________________________

namespace TP.ConcurrentProgramming.BusinessLogic.Test
{
  [TestClass]
  public class BallUnitTest
  {
     [TestMethod]
    public async Task MoveTestMethod()
    {
        DataBallFixture dataBallFixture = new DataBallFixture();
        IPosition initialPosition = new Position(0, 0);
        Ball newInstance = new Ball(dataBallFixture, initialPosition);
    
        int numberOfCallBackCalled = 0;
        var tcs = new TaskCompletionSource<bool>();
    
        newInstance.NewPositionNotificationAsync += async (sender, position) =>
        {
            Assert.IsNotNull(sender);
            Assert.IsNotNull(position);
            numberOfCallBackCalled++;
            tcs.SetResult(true);
            await Task.CompletedTask;
        };
    
        dataBallFixture.Move();
    
        await tcs.Task;
    
        Assert.AreEqual(1, numberOfCallBackCalled);
    }

    #region testing instrumentation

    private class DataBallFixture : Data.IBall
    {
      public Data.IVector Velocity { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

      public event Func<object, Data.IVector, Task>? NewPositionNotificationAsync;
      public double Diameter { get; }
      public float Mass { get; }
      internal void Move()
      {
        NewPositionNotificationAsync?.Invoke(this, new VectorFixture(0.0, 0.0));
      }
    }

    private class VectorFixture : Data.IVector
    {
      internal VectorFixture(double X, double Y)
      {
        x = X; y = Y;
      }

      public double x { get; init; }
      public double y { get; init; }
    }

    #endregion testing instrumentation
  }
}