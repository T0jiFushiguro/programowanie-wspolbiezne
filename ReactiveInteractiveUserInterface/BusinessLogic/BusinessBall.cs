//____________________________________________________________________________________________________________________________________
//
//  Copyright (C) 2024, Mariusz Postol LODZ POLAND.
//
//  To be in touch join the community by pressing the `Watch` button and get started commenting using the discussion panel at
//
//  https://github.com/mpostol/TP/discussions/182
//
//_____________________________________________________________________________________________________________________________________




using TP.ConcurrentProgramming.Data;

namespace TP.ConcurrentProgramming.BusinessLogic
{
  internal class Ball : IBall
  {
    public Ball(Data.IBall ball, IPosition valPosition)
    {
      dataBall = ball;
      diameter = ball.Diameter;
      mass = ball.Mass;
      position = valPosition;
      previousPosition = valPosition;
      ball.NewPositionNotificationAsync += RaisePositionChangeEventAsync;
    }

    #region IBall

    //public event EventHandler<IPosition>? NewPositionNotification;
    public event Func<object, IPosition, Task>? NewPositionNotificationAsync;
    public double Diameter => diameter;

    public float mass { get; private set; }

    public IPosition position { get; private set; }
    public IPosition previousPosition { get; private set; }

    public IVector Velocity
    {
        get => dataBall.Velocity;
        set
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            dataBall.Velocity = value;
        }
    }

    #endregion IBall

    #region private

    private async Task RaisePositionChangeEventAsync(object sender, IVector e)
    {
      previousPosition = position;
      position = new Position(e.x, e.y);

      // Wywołaj event asynchroniczny w warstwie biznesowej i poczekaj na jego obsługę
      var handlers = NewPositionNotificationAsync;
      if (handlers != null)
      {
        var invocationList = handlers.GetInvocationList().Cast<Func<object, IPosition, Task>>();
        var tasks = invocationList.Select(h => h(this, position));
        await Task.WhenAll(tasks);
      }
    }

    //private void RaisePositionChangeEvent(object? sender, Data.IVector e)
    //{
    //  position = new Position(e.x, e.y);
    //  NewPositionNotification?.Invoke(this, new Position(e.x, e.y));
    //}

    private readonly Data.IBall dataBall;

    private readonly double diameter;

    #endregion private
  }
}