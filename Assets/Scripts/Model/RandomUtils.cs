using System;
using System.Collections.Generic;

namespace IE.Model
{
    public static class RandomUtils
    {
        private static Random random = new Random();

        public static int GetRandomInt(int min, int max)
        {
            return random.Next(min, max);
        }

        public static T GetRandomItem<T>(IList<T> values)
        {
            int index = GetRandomInt(0, values.Count);
            
            return values[index];
        }
    }
}