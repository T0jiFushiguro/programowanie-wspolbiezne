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

    internal Ball(Vector initialPosition, Vector initialVelocity)
    {
      Position = initialPosition;
      Velocity = initialVelocity;
    }

    #endregion ctor

    #region IBall

    public event EventHandler<IVector>? NewPositionNotification;

    public IVector Velocity { get; set; }

    #endregion IBall

    #region private

    private Vector Position;

    private void RaiseNewPositionChangeNotification()
    {
      NewPositionNotification?.Invoke(this, Position);
    }

        internal void Move(Vector delta)
        {
            
            int diameter = 20; //temporary
            int borderWall = 5; //temporary
            int borderHeight = 400; //temporary
            int borderWidth = 400;

            if (Position.x + delta.x >= (0 /*+ (diameter / 2)*/) &&
                Position.x + delta.x <= (borderWidth - diameter - borderWall*2) && 
                Position.y + delta.y >= (0/* + (diameter / 2)*/) && 
                Position.y + delta.y <= (borderHeight - diameter - borderWall*2)){
                Position = new Vector(Position.x + delta.x, Position.y + delta.y);
                RaiseNewPositionChangeNotification(); 
            }
    }

    #endregion private
  }
}