using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP.ConcurrentProgramming.BusinessLogic
{
    internal class SpatialGrid
    {
        private readonly int cellSize;
        private readonly Dictionary<(int, int), List<IBall>> cells = new Dictionary<(int, int), List<IBall>>();

        public SpatialGrid(int cellSize)
        {
            this.cellSize = cellSize;
        }

        // Oblicza indeks komorki dla danej pozycji
        private (int, int) GetCellIndex(float x, float y)
        {
            return ((int)Math.Floor(x / cellSize), (int)Math.Floor(y / cellSize));
        }


        public void AddBall(IBall ball)
        {
            var (x, y) = (ball.position.x, ball.position.y);
            var cellIndex = GetCellIndex((float)x, (float)y);

            lock (cells)
            {
                if (!cells.ContainsKey(cellIndex))
                {
                    cells[cellIndex] = new List<IBall>();
                }
                cells[cellIndex].Add(ball);
            }
        }

        public List<IBall> GetNearbyBalls(IBall ball)
        {
            var (x, y) = (ball.position.x, ball.position.y);
            var cellIndex = GetCellIndex((float)x, (float)y);

            List<IBall> nearbyBalls = new List<IBall>();

            for (int dx = -1; dx <= 1; dx++)
            {
                for (int dy = -1; dy <= 1; dy++)
                {
                    var neighborCellIndex = (cellIndex.Item1 + dx, cellIndex.Item2 + dy);

                    lock (cells)
                    {
                        if (cells.TryGetValue(neighborCellIndex, out var balls))
                        {
                            nearbyBalls.AddRange(balls);
                        }
                    }
                }
            }

            return nearbyBalls;
        }

        public void Clear()
        {
            lock (cells)
            {
                cells.Clear();
            }
        }

        public void UpdateBallPosition(IBall ball)
        {
            var oldCellIndex = GetCellIndex((float)ball.previousPosition.x, (float)ball.previousPosition.y);
            var newCellIndex = GetCellIndex((float)ball.position.x, (float)ball.position.y);

            if (oldCellIndex == newCellIndex) return;

            // Usun pilke ze starej komorki
            lock (cells)
            {
                if (cells.TryGetValue(oldCellIndex, out var balls))
                {
                    balls.Remove(ball);
                }
            }

            AddBall(ball);
        }
    }
}
