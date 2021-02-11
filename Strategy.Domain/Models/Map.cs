using System;
using System.Collections.Generic;

namespace Strategy.Domain.Models
{
    /// <summary>
    /// Карта.
    /// </summary>
    public sealed class Map
    {
        private Matrix gameMatrix = new Matrix();

        /// <inheritdoc />
        public Map(IReadOnlyList<GameObject> ground, IReadOnlyList<GameObject> units)
        {
            if(ground != null)
                foreach (var gameObject in ground)
                    if(gameObject.Type == GameObjectType.Water)
                        gameMatrix[gameObject.X,gameObject.Y] = gameObject;

            if (units != null)
                foreach (var gameObject in units)
                    gameMatrix[gameObject.X, gameObject.Y] = gameObject;
        }

        public void Move(Unit unit, int x, int y)
        {
            gameMatrix[unit.X, unit.Y] = null;

            unit.Move(x, y);
            gameMatrix[x, y] = unit;

        }

        public GameObjectType GetGameObjectTypeByCoordinates(int x, int y)
        {
            if(gameMatrix[x, y] == null)
            {
                return GameObjectType.Grass;
            }

            return gameMatrix[x, y].Type;
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