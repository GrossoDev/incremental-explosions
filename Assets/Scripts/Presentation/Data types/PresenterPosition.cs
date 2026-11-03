using System;

namespace IE.Presentation
{
    public struct PresenterPosition : IEquatable<PresenterPosition>
    {
        public int X;
        public int Y;
  
        public PresenterPosition(int x, int y)
        {
            X = x;
            Y = y;
        }

        public bool Equals(PresenterPosition other)
        {
            return X == other.X && Y == other.Y;
        }

        public override bool Equals(object obj)
        {
            return obj is PresenterPosition other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (X * 397) ^ Y;
            }
        }

        public static bool operator ==(PresenterPosition left, PresenterPosition right) => left.Equals(right);
        public static bool operator !=(PresenterPosition left, PresenterPosition right) => !left.Equals(right);

        public override string ToString() => $"({X}, {Y})";
    }
}