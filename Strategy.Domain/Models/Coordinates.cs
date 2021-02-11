namespace Strategy.Domain.Models
{
    /// <summary>
    /// Координаты на карте.
    /// </summary>
    public sealed class Coordinates
    {
        /// <inheritdoc />
        public Coordinates(int x, int y)
        {
            X = x;
            Y = y;
        }


        /// <summary>
        /// Координата X.
        /// </summary>
        public int X { get; }

        /// <summary>
        /// Координата Y.
        /// </summary>
        public int Y { get; }


        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj is Coordinates other && Equals(other);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                return (X * 397) ^ Y;
            }
        }

        /// <summary>
        /// Проверить на равенство с другим объектом.
        /// </summary>
        private bool Equals(Coordinates other)
        {
            return X == other.X && Y == other.Y;
        }
    }
}