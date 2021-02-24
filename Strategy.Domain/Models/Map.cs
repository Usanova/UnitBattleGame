using System;
using System.Collections.Generic;

namespace Strategy.Domain.Models
{
    /// <summary>
    /// Карта.
    /// </summary>
    public sealed class Map
    {
        private Matrix groundMatrix = new Matrix();

        private Matrix unitMatrix = new Matrix();

        /// <inheritdoc />
        public Map(IReadOnlyList<GameObject> ground, IReadOnlyList<GameObject> units)
        {
            if(ground != null)
                foreach (var gameObject in ground)
                    groundMatrix[gameObject.X,gameObject.Y] = gameObject;

            if (units != null)
                foreach (var gameObject in units)
                    unitMatrix[gameObject.X, gameObject.Y] = gameObject;
        }

        public void Move(Unit unit, int x, int y)
        {
            unitMatrix[unit.X, unit.Y] = null;

            unit.Move(x, y);
            unitMatrix[x, y] = unit;
        }

        public GameObject GetGameObjectByCoordinates(int x, int y)
        {
            if(unitMatrix[x, y] != null)
                return unitMatrix[x, y];
            if(groundMatrix[x,y] != null)
                return groundMatrix[x, y];

            return null;
        }

        public void SelectMovingArea(Unit unit) => ChangeMovingArea(unit, true);

        public void UnselectMovingArea(Unit unit) => ChangeMovingArea(unit, false);

        private void ChangeMovingArea(Unit unit, bool isMovingArea)
        {
            for (int i = Math.Max(0, unit.X - unit.MaximumTravelDistance);
                i <= Math.Min(19, unit.X + unit.MaximumTravelDistance); i++)
            {
                for (int j = Math.Max(0, unit.Y - unit.MaximumTravelDistance);
                j <= Math.Min(19, unit.Y + unit.MaximumTravelDistance); j++)
                {
                    if (i == unit.X && j == unit.Y)
                        continue;

                    groundMatrix[i, j].IsMovingArea = isMovingArea;
                }
            }
        }
    }



    internal class Matrix
    {
        private Dictionary<string, GameObject> matrix = new Dictionary<string, GameObject>();

        public GameObject this [int x, int y]
        {
            get
            {
                if (!matrix.ContainsKey(GetCoordinatesString(x, y)))
                    return null;

                return matrix[GetCoordinatesString(x, y)];
            }

            set
            {
                if (value == null)
                    matrix.Remove(GetCoordinatesString(x, y));
                matrix[GetCoordinatesString(x, y)] = value;
            }
        }

        private string GetCoordinatesString(int x, int y) => $"{x}:{y}";
    }
}