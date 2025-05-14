using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using TP.ConcurrentProgramming.Data;

namespace TP.ConcurrentProgramming.BusinessLogic
{
    public record Vector(double x, double y) : IVector;
    internal class BallManager
    {

        private readonly IList<IBall> balls;
        private bool disposed = false;

        public BallManager(IList<IBall> balls) {
            this.balls = balls ?? throw new ArgumentNullException(nameof(balls));
            foreach (var ball in balls)
            {
                ball.NewPositionNotification += OnBallPositionChanged;
            }
        }

        private void OnBallPositionChanged(object sender, IPosition newPosition)
        {
            // Logika biznesowa reagująca na zmianę pozycji kulki
            //

            if (sender is IBall ballSender)
            {
                if (balls != null)
                {
                    BallCollision(balls, ballSender);
                }
            }
        }

        private void BallCollision(IList<IBall> balls, IBall ballSender)
        {

            Vector2 positionBallSender = new Vector2((float)ballSender.position.x, (float)ballSender.position.y);
            double diamterBallSender = ballSender.Diameter;

            

            foreach (var ball in balls)
            {
                Vector2 positionBall = new Vector2((float)ball.position.x, (float)ball.position.y);
                double diamterBall = ball.Diameter;

                //CheckCollision()

                ball.Velocity = new Vector(20, 20);
            }
        }

        private bool CheckCollision(Vector2 pos1, float radius1, Vector2 pos2, float radius2)
        {
            float distanceSquared = (pos1 - pos2).Length(); //LengthSquared
            float radiusSum = radius1 + radius2;
            return distanceSquared <= radiusSum * radiusSum;
        }




        public void Update()
        {
            if (disposed) throw new ObjectDisposedException(nameof(BallManager));

            foreach (var ball in balls)
            {
                //BoundaryCollision(ball);
            }

            //BallsCollisions();

            foreach (var ball in balls)
            {
                //ball.M
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                // Zwolnij zasoby zarządzane, jeśli istnieją
                if (balls != null)
                {
                    foreach (var ball in balls)
                    {
                        if (ball is IDisposable disposableBall)
                        {
                            disposableBall.Dispose();
                        }
                    }
                    balls.Clear();
                }
            }

            disposed = true;
        }
    }
}
