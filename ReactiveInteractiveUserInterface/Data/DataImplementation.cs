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
       cts = new CancellationTokenSource();
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
            Vector startingPosition = new(random.Next(50, 550), random.Next(50, 550));
            Vector startingVelocity = new(random.Next(-50, 50), random.Next(-50, 50));
            float mass = random.Next(1, 10);
            Ball newBall = new(startingPosition, startingVelocity, diameter, mass);
            //upperLayerHandler.Metho
            upperLayerHandler(startingPosition, newBall);
            BallsList.Add(newBall);
        }

        Task.Run(() => StartMove(null));
    }
    #endregion DataAbstractAPI

    #region IDisposable

    protected virtual void Dispose(bool disposing)
    {
      if (!Disposed)
      {
        if (disposing)
        {
          cts.Cancel();
          cts.Dispose();
          BallsList.Clear();
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

    private CancellationTokenSource cts;

    //Problem taki ze Task sie caly czas tworzy a ma byc raz zrobiony
    //Zobaczyc kod Task
    //Watek musi dluzej dzialac
    private async Task StartMove(object? x)
    {
        var moveTasks = BallsList.Select(item =>
        {
            return Task.Run(async () =>
            {
                while (!cts.Token.IsCancellationRequested)
                {
                    Vector vector = new Vector(item.Velocity.x / 100, item.Velocity.y / 100);
                    item.Move(vector);

                    try
                    {
                        await Task.Delay(TimeSpan.FromMilliseconds(16.6), cts.Token);
                    }
                    catch (TaskCanceledException)
                    {
                        // Oczekiwane anulowanie, wyjdź z pętli
                        break;
                    }
                }

            }, cts.Token);
        });
        
        await Task.WhenAll(moveTasks);
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