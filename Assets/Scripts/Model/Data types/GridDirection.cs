using System;

namespace IE.Model
{
    public struct GridDirection : IEquatable<GridDirection>
    {
        public int DeltaX;
        public int DeltaY;

        public GridDirection(int deltaX, int deltaY)
        {
            DeltaX = deltaX;
            DeltaY = deltaY;
        }

        public bool IsOppositeTo(GridDirection other)
        {
            return DeltaX == -other.DeltaX && DeltaY == -other.DeltaY;
        }

        public GridDirection Opposite()
        {
            return new GridDirection(-DeltaX, -DeltaY);
        }

        public bool Equals(GridDirection other)
        {
            return DeltaX == other.DeltaX && DeltaY == other.DeltaY;
        }

        public override bool Equals(object obj)
        {
            return obj is GridDirection other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (DeltaX * 397) ^ DeltaY;
            }
        }

        public static bool operator ==(GridDirection left, GridDirection right) => left.Equals(right);
        public static bool operator !=(GridDirection left, GridDirection right) => !left.Equals(right);

        public static GridDirection Up => new GridDirection(0, 1);
        public static GridDirection Down => new GridDirection(0, -1);
        public static GridDirection Left => new GridDirection(-1, 0);
        public static GridDirection Right => new GridDirection(1, 0);

        public override string ToString() => $"Direction({DeltaX}, {DeltaY})";
    }
}