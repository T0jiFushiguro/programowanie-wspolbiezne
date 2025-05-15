//____________________________________________________________________________________________________________________________________
//
//  Copyright (C) 2024, Mariusz Postol LODZ POLAND.
//
//  To be in touch join the community by pressing the `Watch` button and get started commenting using the discussion panel at
//
//  https://github.com/mpostol/TP/discussions/182
//
//_____________________________________________________________________________________________________________________________________

namespace TP.ConcurrentProgramming.Data.Test
{
  [TestClass]
  public class BallUnitTest
  {
    [TestMethod]
    public void ConstructorTestMethod()
    {
      Vector testinVector = new Vector(0.0, 0.0);
      Ball newInstance = new(testinVector, testinVector, 20, 5);
    }

    [TestMethod]
    public async Task MoveTestMethod()
    {
        Vector initialPosition = new(10.0, 10.0);
        Ball newInstance = new(initialPosition, new Vector(0.0, 0.0), 20, 5);
    
        IVector? currentPosition = null;
        int numberOfCallBackCalled = 0;
    
        var tcs = new TaskCompletionSource<bool>();
    
        newInstance.NewPositionNotificationAsync += async (sender, position) =>
        {
            Assert.IsNotNull(sender);
            currentPosition = position;
            numberOfCallBackCalled++;
            tcs.SetResult(true);
            await Task.CompletedTask;
        };
    
        await newInstance.Move(new Vector(0.0, 0.0));
    
        // Czekamy na wywołanie eventu
        await tcs.Task;
    
        Assert.AreEqual(1, numberOfCallBackCalled);
        Assert.IsNotNull(currentPosition);
        Assert.AreEqual(initialPosition.x, currentPosition!.x, 1e-6);
        Assert.AreEqual(initialPosition.y, currentPosition.y, 1e-6);
    }

  }
}