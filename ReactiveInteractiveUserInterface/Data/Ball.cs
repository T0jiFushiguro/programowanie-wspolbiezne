//____________________________________________________________________________________________________________________________________
//
//  Copyright (C) 2024, Mariusz Postol LODZ POLAND.
//
//  To be in touch join the community by pressing the `Watch` button and get started commenting using the discussion panel at
//
//  https://github.com/mpostol/TP/discussions/182
//
//_____________________________________________________________________________________________________________________________________

namespace TP.ConcurrentProgramming.Data
{
  internal class Ball : IBall
  {
    #region ctor

    internal Ball(Vector initialPosition, Vector initialVelocity, double initialDiameter, float initialMass)
    {
      Position = initialPosition;
      Velocity = initialVelocity;
      diameter = initialDiameter;
      mass = initialMass;
    }

    #endregion ctor

    #region IBall

    public event Func<object, IVector, Task>? NewPositionNotificationAsync;

    public IVector Velocity { get; set; }
    public double Diameter => diameter;

    public float Mass => mass;

    #endregion IBall

    #region private

    private Vector Position;

    private readonly double diameter;

    private readonly float mass;
   

   protected async Task RaiseNewPositionChangeNotificationAsync()
    {
      var handlers = NewPositionNotificationAsync;
      if (handlers != null)
      {
        var invocationList = handlers.GetInvocationList()
                                     .Cast<Func<object, IVector, Task>>();
        var tasks = invocationList.Select(handler => handler(this, Position));
        await Task.WhenAll(tasks);
      }
    }


    internal async Task Move(Vector delta)
    {
        Position = new Vector(Position.x + delta.x, Position.y + delta.y);
        await RaiseNewPositionChangeNotificationAsync();
    }

    #endregion private
  }
}