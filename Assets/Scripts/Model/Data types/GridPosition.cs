using System;

namespace IE.Model
{
    public struct GridPosition : IEquatable<GridPosition>
    {
        public int X;
        public int Y;

        public GridPosition(int x, int y)
        {
            X = x;
            Y = y;
        }

        public GridPosition Add(int deltaX, int deltaY)
        {
            return new GridPosition(X + deltaX, Y + deltaY);
        }
        
        public float DistanceTo(GridPosition other)
        {
            int deltaX = other.X - X;
            int deltaY = other.Y - Y;
            return (float)Math.Sqrt(deltaX * deltaX + deltaY * deltaY);
        }

        public bool Equals(GridPosition other)
        {
            return X == other.X && Y == other.Y;
        }

        public override bool Equals(object obj)
        {
            return obj is GridPosition other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (X * 397) ^ Y;
            }
        }

        public static bool operator ==(GridPosition left, GridPosition right) => left.Equals(right);
        public static bool operator !=(GridPosition left, GridPosition right) => !left.Equals(right);

        public override string ToString() => $"({X}, {Y})";
    }
}