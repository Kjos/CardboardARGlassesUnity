
using System;

public static class Extensions
{

    public static T[] SubArray<T>(this T[] source, int start, int end)
    {
        int count = end - start;
        T[] result = new T[count];
        Array.Copy(source, start, result, 0, count);
        return result;
    }
}
