using System;

namespace IE.Presentation
{
    public struct PresenterDirection : IEquatable<PresenterDirection>
    {
        public int DeltaX;
        public int DeltaY;

        public PresenterDirection(int deltaX, int deltaY)
        {
            DeltaX = deltaX;
            DeltaY = deltaY;
        }

        public bool Equals(PresenterDirection other)
        {
            return DeltaX == other.DeltaX && DeltaY == other.DeltaY;
        }

        public override bool Equals(object obj)
        {
            return obj is PresenterDirection other && Equals(other);
        }
 
        public override int GetHashCode()
        {
            unchecked
            {
                return (DeltaX * 397) ^ DeltaY;
            }
        }

        public static bool operator ==(PresenterDirection left, PresenterDirection right) => left.Equals(right);
        public static bool operator !=(PresenterDirection left, PresenterDirection right) => !left.Equals(right);

        public override string ToString() => $"Direction({DeltaX}, {DeltaY})";
    }
}