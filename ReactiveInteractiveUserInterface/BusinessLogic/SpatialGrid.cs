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

        // Oblicza indeks komórki dla danej pozycji
        private (int, int) GetCellIndex(float x, float y)
        {
            return ((int)Math.Floor(x / cellSize), (int)Math.Floor(y / cellSize));
        }

        // Dodaje piłkę do siatki
        public void AddBall(IBall ball)
        {
            var (x, y) = (ball.position.x, ball.position.y);
            var cellIndex = GetCellIndex((float)x, (float)y);

            lock (cells) // Synchronizacja dostępu do słownika
            {
                if (!cells.ContainsKey(cellIndex))
                {
                    cells[cellIndex] = new List<IBall>();
                }
                cells[cellIndex].Add(ball);
            }
        }

        // Pobiera listę piłek w sąsiednich komórkach
        public List<IBall> GetNearbyBalls(IBall ball)
        {
            var (x, y) = (ball.position.x, ball.position.y);
            var cellIndex = GetCellIndex((float)x, (float)y);

            List<IBall> nearbyBalls = new List<IBall>();

            // Iteracja po sąsiednich komórkach
            for (int dx = -1; dx <= 1; dx++)
            {
                for (int dy = -1; dy <= 1; dy++)
                {
                    var neighborCellIndex = (cellIndex.Item1 + dx, cellIndex.Item2 + dy);

                    lock (cells) // Synchronizacja dostępu do słownika
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

        // Czyści siatkę (usuwa wszystkie piłki z komórek)
        public void Clear()
        {
            lock (cells)
            {
                cells.Clear();
            }
        }

        // Metoda pomocnicza do aktualizacji pozycji piłki w siatce (jeśli pozycja się zmieniła)
        public void UpdateBallPosition(IBall ball)
        {
            // Oblicz stare i nowe indeksy komórek
            var oldCellIndex = GetCellIndex((float)ball.previousPosition.x, (float)ball.previousPosition.y);
            var newCellIndex = GetCellIndex((float)ball.position.x, (float)ball.position.y);

            // Jeśli piłka nie zmieniła komórki, nic nie rób
            if (oldCellIndex == newCellIndex) return;

            // Usuń piłkę ze starej komórki
            lock (cells)
            {
                if (cells.TryGetValue(oldCellIndex, out var balls))
                {
                    balls.Remove(ball);
                }
            }

            // Dodaj piłkę do nowej komórki
            AddBall(ball);
        }
    }
}
