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

    internal Ball(Vector initialPosition, Vector initialVelocity, double initialDiameter)
    {
      Position = initialPosition;
      Velocity = initialVelocity;
      diameter = initialDiameter;
    }

    #endregion ctor

    #region IBall

    public event EventHandler<IVector>? NewPositionNotification;

    public IVector Velocity { get; set; }
    public double Diameter => diameter;

    #endregion IBall

    #region private

    private Vector Position;

    private readonly double diameter;

    private void RaiseNewPositionChangeNotification()
    {
      NewPositionNotification?.Invoke(this, Position);
    }

    internal void Move(Vector delta)
    {
        int borderWall = 5; //temporary
        int borderHeight = 400; //temporary
        int borderWidth = 400;

        if (Position.x + delta.x >= (0) &&
            Position.x + delta.x <= (borderWidth - diameter - borderWall*2) && 
            Position.y + delta.y >= (0) && 
            Position.y + delta.y <= (borderHeight - diameter - borderWall*2)){
            Position = new Vector(Position.x + delta.x, Position.y + delta.y);
            RaiseNewPositionChangeNotification(); 
        }
    }

    #endregion private
  }
}