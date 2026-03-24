using System;
using System.Collections.Generic;

namespace TypeSerialization._internal;

internal static class IEnumerableExt
{
    public static int FirstIndex<T>(this IEnumerable<T> items, Func<T, bool> selector)
    {
        var i = 0;
        foreach (var item in items)
        {
            if (selector(item))
                return i;

            i++;
        }
        return -1;
    }
}
