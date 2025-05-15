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
        private readonly object _ballslock = new object();
        private readonly IList<IBall> balls;
        private bool disposed = false;
        private int borderHeight = 400;
        private int borderWidth = 400;

        public BallManager(IList<IBall> balls) {
            this.balls = balls ?? throw new ArgumentNullException(nameof(balls));

            
            foreach (var ball in balls)
            {
                //ball.NewPositionNotification += OnBallPositionChanged;
                //ball.NewPositionNotification += OnBallPositionChanged;
                ball.NewPositionNotificationAsync += OnBallPositionChanged;
            }
        }

        private async Task OnBallPositionChanged(object sender, IPosition newPosition)
        {
            // Logika biznesowa reagująca na zmianę pozycji kulki

            if (sender is IBall ballSender)
            {
                //lock (_ballslock)
                //{
                    if (balls != null)
                    {
                        BallCollision(balls, ballSender);
                        BorderCollision(ballSender);

                    }
                //}
            }

            await Task.CompletedTask;
        }

        private void BorderCollision(IBall ballSender)
        {
            Vector2 positionBall = new Vector2((float)ballSender.position.x, (float)ballSender.position.y);
            Vector2 velocityBall = new Vector2((float)ballSender.Velocity.x, (float)ballSender.Velocity.y);
            float diameterBall = (float)ballSender.Diameter;

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
            IVector velocityBallSender = ballSender.Velocity;
            double diamterBallSender = ballSender.Diameter;


            foreach (var ball in balls)
            {
                if (ball == ballSender) continue;

                Vector2 positionBall = new Vector2((float)ball.position.x, (float)ball.position.y);
                IVector velocityBall = ball.Velocity;
                double diamterBall = ball.Diameter;

                
                Vector2 positionDelta = positionBall - positionBallSender;
                float ballDistance = positionDelta.Length();
                float radiusSum = (float)diamterBall / 2f + (float)diamterBallSender / 2f;

                //wykrycie kolizji
                if (ballDistance <= radiusSum && ballDistance > 0) {
                    float depthInBall = radiusSum - ballDistance;

                    Vector2 collisionNormal = positionDelta / ballDistance;

                    Vector2 relativeVelocity = new Vector2((float)velocityBall.x - (float)velocityBallSender.x, (float)velocityBall.y - (float)velocityBallSender.y);
                    float velocityAlongNormal = Vector2.Dot(relativeVelocity, collisionNormal);

                    // Jeśli kulki oddalają się, nie wykonujemy odbicia
                    if (velocityAlongNormal <= 0)
                    {
                        var (newVelocityBall, newVelocityBallSender) = CalculateElasticCollision(velocityBall, ball.mass, velocityBallSender, ballSender.mass, collisionNormal);

                        var firstLock = ball.GetHashCode() < ballSender.GetHashCode() ? ball : ballSender;
                        var secondLock = firstLock == ball ? ballSender : ball;

                        lock (firstLock)
                        {
                            lock (secondLock)
                            {
                                ball.Velocity = newVelocityBall;
                                ballSender.Velocity = newVelocityBallSender;
                            }
                        }
                    } else if (depthInBall > 0f)
                    {
                        float percent = 0.8f;
                        float slop = 0.01f;

                        Vector2 correctionVelocity = collisionNormal * MathF.Max(depthInBall - slop, 0) / 2f * percent / 16.6f;
                        var firstLock = ball.GetHashCode() < ballSender.GetHashCode() ? ball : ballSender;
                        var secondLock = firstLock == ball ? ballSender : ball;

                        lock (firstLock)
                        {
                            lock (secondLock)
                            {
                                ball.Velocity = new Vector(velocityBall.x + correctionVelocity.X, velocityBall.y + correctionVelocity.Y);
                                ballSender.Velocity = new Vector(velocityBallSender.x - correctionVelocity.X, velocityBallSender.y - correctionVelocity.Y);
                            }
                        }
                    }
                }
                 
            }
        }

        private (Vector newVelocity1, Vector newVelocity2) CalculateElasticCollision(
            IVector velocity1, double mass1,
            IVector velocity2, double mass2,
            Vector2 collisionNormal)
        {
            Vector2 n = Vector2.Normalize(collisionNormal);
        

            Vector2 v1 = new Vector2((float)velocity1.x, (float)velocity1.y);
            Vector2 v2 = new Vector2((float)velocity2.x, (float)velocity2.y);
        
            float v1n = Vector2.Dot(v1, n);
            float v2n = Vector2.Dot(v2, n);
        
            // Obliczamy nowe składowe prędkości wzdłuż normalnej po zderzeniu
            float v1nAfter = (float)((v1n * (mass1 - mass2) + 2 * mass2 * v2n) / (mass1 + mass2));
            float v2nAfter = (float)((v2n * (mass2 - mass1) + 2 * mass1 * v1n) / (mass1 + mass2));
        
            // Składowe prędkości prostopadłe do normalnej pozostają bez zmian
            Vector2 v1t = v1 - v1n * n;
            Vector2 v2t = v2 - v2n * n;
        
            // Nowe prędkości to suma składowych normalnych po zderzeniu i niezmienionych stycznych
            Vector2 v1After = v1t + v1nAfter * n;
            Vector2 v2After = v2t + v2nAfter * n;
        
            return (
                new Vector(v1After.X, v1After.Y),
                new Vector(v2After.X, v2After.Y)
            );
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
                // Usuwamy subskrypcje eventów i zwalniamy zasoby zarządzane
                if (balls != null)
                {
                    foreach (var ball in balls)
                    {
                        // Odsubskrybowanie eventu
                        ball.NewPositionNotificationAsync -= OnBallPositionChanged;

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
