//____________________________________________________________________________________________________________________________________
//
//  Copyright (C) 2024, Mariusz Postol LODZ POLAND.
//
//  To be in touch join the community by pressing the `Watch` button and get started commenting using the discussion panel at
//
//  https://github.com/mpostol/TP/discussions/182
//
//_____________________________________________________________________________________________________________________________________

using System;
using System.Diagnostics;

namespace TP.ConcurrentProgramming.Data
{
  internal class DataImplementation : DataAbstractAPI
  {
    #region ctor

    public DataImplementation()
    {
        Move(null);
    }

        #endregion ctor

        #region DataAbstractAPI

        public override void Start(int numberOfBalls, double diameter, Action<IVector, IBall> upperLayerHandler)
        {
            if (Disposed)
                throw new ObjectDisposedException(nameof(DataImplementation));
            if (upperLayerHandler == null)
                throw new ArgumentNullException(nameof(upperLayerHandler));
            Random random = new Random();
            for (int i = 0; i < numberOfBalls; i++)
            {
                Vector startingPosition = new(random.Next(100, 400 - 100), random.Next(100, 400 - 100));
                Vector startingVelocity = new(random.Next(-50, 50), random.Next(-50, 50));
                float mass = random.Next(1, 10);
                Ball newBall = new(startingPosition, startingVelocity, diameter, mass);
                //upperLayerHandler.Metho
                upperLayerHandler(startingPosition, newBall);
                BallsList.Add(newBall);
            }
        }
        #endregion DataAbstractAPI

        #region IDisposable

    protected virtual void Dispose(bool disposing)
    {
      if (!Disposed)
      {
        if (disposing)
        {
          BallsList.Clear();
          cts.Cancel();
        }
        Disposed = true;
      }
      else
        throw new ObjectDisposedException(nameof(DataImplementation));
    }

    public override void Dispose()
    {
      // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
      Dispose(disposing: true);
      GC.SuppressFinalize(this);
    }

    #endregion IDisposable

    #region private

    private bool Disposed = false;

    private readonly Timer MoveTimer;

    private List<Ball> BallsList = [];

    private CancellationTokenSource cts = new CancellationTokenSource();

    private async Task Move(object? x)
    {
       while (!cts.Token.IsCancellationRequested)
       {
           await Task.Run(async () =>
           {
               var moveTasks = BallsList.Select(async item =>
               {
                   Vector vector = new Vector(item.Velocity.x / 100, item.Velocity.y / 100);
                   await item.Move(vector);
               });

               await Task.WhenAll(moveTasks);
           });

           await Task.Delay(TimeSpan.FromMilliseconds(16.6), cts.Token);
       }
    }

    #endregion private

    #region TestingInfrastructure

    [Conditional("DEBUG")]
    internal void CheckBallsList(Action<IEnumerable<IBall>> returnBallsList)
    {
      returnBallsList(BallsList);
    }

    [Conditional("DEBUG")]
    internal void CheckNumberOfBalls(Action<int> returnNumberOfBalls)
    {
      returnNumberOfBalls(BallsList.Count);
    }

    [Conditional("DEBUG")]
    internal void CheckObjectDisposed(Action<bool> returnInstanceDisposed)
    {
      returnInstanceDisposed(Disposed);
    }

    #endregion TestingInfrastructure
  }
}