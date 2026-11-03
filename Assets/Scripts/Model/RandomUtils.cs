using System;

namespace IE.Model
{
    public static class RandomUtils
    {
        private static Random random = new Random();

        public static int GetRandomInt(int min, int max)
        {
            return random.Next(min, max);
        }

        public static GridDirection GetRandomDirection()
        {
            int directionIndex = GetRandomInt(0, 4);
            
            switch (directionIndex)
            {
                case 0:
                    return GridDirection.Up;
                case 1:
                    return GridDirection.Down;
                case 2:
                    return GridDirection.Left;
                case 3:
                    return GridDirection.Right;
                default:
                    throw new InvalidOperationException("Invalid random direction index.");
            }
        }
    }
}