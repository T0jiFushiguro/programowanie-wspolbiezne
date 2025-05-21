//____________________________________________________________________________________________________________________________________
//
//  Copyright (C) 2023, Mariusz Postol LODZ POLAND.
//
//  To be in touch join the community by pressing the `Watch` button and get started commenting using the discussion panel at
//
//  https://github.com/mpostol/TP/discussions/182
//
//  by introducing yourself and telling us what you do with this community.
//_____________________________________________________________________________________________________________________________________

using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using TP.ConcurrentProgramming.BusinessLogic;
using LogicIBall = TP.ConcurrentProgramming.BusinessLogic.IBall;

namespace TP.ConcurrentProgramming.Presentation.Model
{
  internal class ModelBall : IBall
  {
    public ModelBall(double top, double left, LogicIBall underneathBall)
    {
      TopBackingField = top;
      LeftBackingField = left;
      underneathBall.NewPositionNotificationAsync += NewPositionNotification;
      //underneathBall.NewPositionNotification += NewPositionNotification;
    }

    #region IBall

    public double Top
    {
      get { return TopBackingField; }
      private set
      {
        if (TopBackingField == value)
          return;
        TopBackingField = value;
        RaisePropertyChanged();
      }
    }

    public double Left
    {
      get { return LeftBackingField; }
      private set
      {
        if (LeftBackingField == value)
          return;
        LeftBackingField = value;
        RaisePropertyChanged();
      }
    }

    public double Diameter { get; init; } = 0;

    #region INotifyPropertyChanged

    public event PropertyChangedEventHandler PropertyChanged;

    #endregion INotifyPropertyChanged

    #endregion IBall

    #region private

    private double TopBackingField;
    private double LeftBackingField;


    private async Task NewPositionNotification(object sender, IPosition e)
    {
        Top = e.y;
        Left = e.x;
        await Task.CompletedTask; // jeśli nie masz innych async operacji
    }


    private void RaisePropertyChanged([CallerMemberName] string propertyName = "")
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    #endregion private

    #region testing instrumentation

    [Conditional("DEBUG")]
    internal void SetLeft(double x)
    { Left = x; }

    [Conditional("DEBUG")]
    internal void SettTop(double x)
    { Top = x; }

        #endregion testing instrumentation

        /*using System.Collections.Generic;
using System;
using System.Threading.Tasks;

public class SpatialGrid
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

// Przykładowy interfejs dla piłki
public interface IBall
{
    double position { get; }
    double Diameter { get; }
    object previousPosition { get; }
}
*/
    }
}