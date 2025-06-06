﻿//____________________________________________________________________________________________________________________________________
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
       logger = new ReactiveDiagnosticsLogger("logVelocity.txt");
       BallsList = new List<Ball>();
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
            upperLayerHandler(startingPosition, newBall);
            BallsList.Add(newBall);
        }

        StartMove(null);
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
          
          try
          {
              if (moveTasks != null) {
                Task[] tasksArray = moveTasks.ToArray();
                if (tasksArray != null && tasksArray.Length > 0)
                {
                    Task.WaitAll(tasksArray);
                }
              }
          }
          catch (AggregateException ae)
          {
              ae.Handle(e => e is OperationCanceledException);
          }
          catch (OperationCanceledException)
          {
              
          }

          cts.Dispose();
          logger.Dispose();
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

    private IEnumerable<Task> moveTasks;

    private List<Ball> BallsList;

    private CancellationTokenSource cts;

    private ReactiveDiagnosticsLogger logger;

    private async Task StartMove(object? x)
    {
        moveTasks = BallsList.Select((item, index) =>
        {
            return Task.Run(async () =>
            {

                var stopwatch = Stopwatch.StartNew();
                double lastElapsedTime = 0;
                double currentElapsedTime = 0;
                double deltaTime = 0;

                int numberBall = index;

                while (!cts.Token.IsCancellationRequested)
                {

                    currentElapsedTime = stopwatch.Elapsed.TotalSeconds;
                    deltaTime = currentElapsedTime - lastElapsedTime;

                    Vector vector = new Vector(item.Velocity.x  * deltaTime, item.Velocity.y * deltaTime);
                    item.Move(vector);

                    logger.Log($"Ball {numberBall}: Velocity = ({item.Velocity.x}, {item.Velocity.y})");

                    lastElapsedTime = currentElapsedTime;


                    try
                    {
                        //Male opoznienie zeby cpy nie bylo 100%
                        await Task.Delay(TimeSpan.FromMilliseconds(1), cts.Token); //16.6
                    }
                    catch (TaskCanceledException)
                    {
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