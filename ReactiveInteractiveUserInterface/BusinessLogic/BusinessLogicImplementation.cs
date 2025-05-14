//____________________________________________________________________________________________________________________________________
//
//  Copyright (C) 2024, Mariusz Postol LODZ POLAND.
//
//  To be in touch join the community by pressing the `Watch` button and get started commenting using the discussion panel at
//
//  https://github.com/mpostol/TP/discussions/182
//
//_____________________________________________________________________________________________________________________________________

using System.Diagnostics;
using UnderneathLayerAPI = TP.ConcurrentProgramming.Data.DataAbstractAPI;

namespace TP.ConcurrentProgramming.BusinessLogic
{
  internal class BusinessLogicImplementation : BusinessLogicAbstractAPI
  {
    #region ctor

    public BusinessLogicImplementation() : this(null)
    { }

    internal BusinessLogicImplementation(UnderneathLayerAPI? underneathLayer)
    {
      layerBellow = underneathLayer == null ? UnderneathLayerAPI.GetDataLayer() : underneathLayer;
    }

    #endregion ctor

    #region BusinessLogicAbstractAPI

    public override void Dispose()
    {
      if (Disposed)
        throw new ObjectDisposedException(nameof(BusinessLogicImplementation));
      layerBellow.Dispose();
      Disposed = true;
    }

    public override async Task Start(int numberOfBalls, double diameter, Action<IPosition, IBall> upperLayerHandler)
    {
      if (Disposed)
        throw new ObjectDisposedException(nameof(BusinessLogicImplementation));
      if (upperLayerHandler == null)
        throw new ArgumentNullException(nameof(upperLayerHandler));

      var tcs = new TaskCompletionSource<bool>();

      layerBellow.Start(numberOfBalls, diameter, (startingPosition, databall) => {
          var ball = new Ball(databall, new Position(startingPosition.x, startingPosition.x));
          balls.Add(ball);
          upperLayerHandler(new Position(startingPosition.x, startingPosition.x), ball);

          if (balls.Count == numberOfBalls)
              tcs.SetResult(true);
      });

      await tcs.Task;

      ballManager = new BallManager(balls);
    }

    #endregion BusinessLogicAbstractAPI

    #region private

    private bool Disposed = false;

    private readonly UnderneathLayerAPI layerBellow;

    private BallManager ballManager;

    private List<IBall> balls = [];

    #endregion private

    #region TestingInfrastructure

    [Conditional("DEBUG")]
    internal void CheckObjectDisposed(Action<bool> returnInstanceDisposed)
    {
      returnInstanceDisposed(Disposed);
    }

    #endregion TestingInfrastructure
  }
}