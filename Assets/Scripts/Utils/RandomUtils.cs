using System;
using System.Collections.Generic;

public static class RandomUtils
{
    private static Random random = new Random();

    public static int GetRandomInt(int minInclusive, int maxExclusive)
    {
        return random.Next(minInclusive, maxExclusive);
    }

    public static T GetRandomItem<T>(IList<T> values)
    {
        int index = GetRandomInt(0, values.Count);
        
        return values[index];
    }
}