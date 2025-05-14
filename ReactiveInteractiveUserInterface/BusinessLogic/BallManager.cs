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
        private int borderHeight = 400;
        private int borderWidth = 400;

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

                    BorderCollision(ballSender);
                }
            }
        }

        private void BorderCollision(IBall ballSender)
        {
            Vector2 positionBall = new Vector2((float)ballSender.position.x, (float)ballSender.position.y);
            Vector2 velocityBall = new Vector2((float)ballSender.Velocity.x, (float)ballSender.Velocity.y);
            float diameterBall = (float)ballSender.Diameter;

            //Vector2 newPosition = positionBall + velocityBall;

            if (positionBall.X <= (0) || positionBall.X + diameterBall + 10 >= borderWidth)
            {
                velocityBall.X = -velocityBall.X;
            }

            if (positionBall.Y <= (0) || positionBall.Y + diameterBall + 10 >= borderHeight)
            {
                velocityBall.Y = -velocityBall.Y;   
            }

            ballSender.Velocity = new Vector(velocityBall.X, velocityBall.Y);
        }

        private void BallCollision(IList<IBall> balls, IBall ballSender)
        {

            Vector2 positionBallSender = new Vector2((float)ballSender.position.x, (float)ballSender.position.y);
            double diamterBallSender = ballSender.Diameter;

            

            foreach (var ball in balls)
            {
                Vector2 positionBall = new Vector2((float)ball.position.x, (float)ball.position.y);
                double diamterBall = ball.Diameter;

                if (ball != ballSender)
                {
                    Vector2 positionDelta = positionBall - positionBallSender;
                    float ballDistance = positionDelta.Length();
                    float radiusSum = (float)diamterBall / 2f + (float)diamterBallSender / 2f;

                    //wykrycie kolizji
                    if (ballDistance <= radiusSum && ballDistance > 0) {
                        float depthInBall = radiusSum - ballDistance;

                        Vector2 collisionNormal = positionDelta / ballDistance;

                        Vector2 relativeVelocity = new Vector2((float)ball.Velocity.x - (float)ballSender.Velocity.x, (float)ball.Velocity.y - (float)ballSender.Velocity.y);
                        float velocityAlongNormal = Vector2.Dot(relativeVelocity, collisionNormal);

                        // Jeśli kulki oddalają się, nie wykonujemy odbicia
                        if (velocityAlongNormal <= 0)
                        {
                            ball.Velocity = CalculateVelocity(ball.Velocity, collisionNormal);
                            ballSender.Velocity = CalculateVelocity(ballSender.Velocity, -collisionNormal);
                        }

                        if (depthInBall > 0f)
                        {
                            //System.Diagnostics.Debug.WriteLine($"Debug: kulka utknela kolizja  {collisionNormal} ");
                            float percent = 0.8f;
                            float slop = 0.01f;

                            Vector2 correctionVelocity = collisionNormal * MathF.Max(depthInBall - slop, 0) / 2f * percent / 16.6f;
                            //Vector2 correctionVelocity = collisionNormal * (depthInBall / 16.6f);

                            ball.Velocity = new Vector(ball.Velocity.x + correctionVelocity.X, ball.Velocity.y + correctionVelocity.Y);
                            ballSender.Velocity = new Vector(ballSender.Velocity.x - correctionVelocity.X, ballSender.Velocity.y - correctionVelocity.Y);
                        }
                    }
                }
            }
        }

        private Vector CalculateVelocity(IVector velocity, Vector2 collisionNormal)
        {
            // Odbicie wektora prędkości względem normalnej: v' = v - 2*(v·n)*n
            double dotVector = velocity.x * collisionNormal.X + velocity.y * collisionNormal.Y;
            return new Vector(
                velocity.x - 2 * dotVector * collisionNormal.X,
                velocity.y - 2 * dotVector * collisionNormal.Y
            );
        }


        /*

                            if (CheckCollision(positionBall, (float)diamterBall / 2f, positionBallSender, (float)diamterBallSender / 2f))
                    {
                        
                    }

        private bool CheckCollision(Vector2 pos1, float radius1, Vector2 pos2, float radius2)
        {
            float distanceSquared = (pos1 - pos2).Length(); //LengthSquared
            float radiusSum = radius1 + radius2;
            return distanceSquared <= radiusSum; //radiusSum * radiusSum
        }*/




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
