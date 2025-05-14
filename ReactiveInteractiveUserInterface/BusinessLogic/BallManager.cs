using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            // np. przekazywanie dalej, walidacje, agregacje itp.
            

            if (balls != null)
            {
                System.Diagnostics.Debug.WriteLine($"Aktualna wartość:");
                foreach (var ball in balls)
                {

                    ball.Velocity = new Vector(20, 20);
                }
            }
        
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
