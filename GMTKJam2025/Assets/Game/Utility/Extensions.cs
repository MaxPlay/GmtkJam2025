using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Extensions
{
    public static bool IsValidIndex<T>(this IEnumerable<T> enumerable, int index)
    {
        return index >= 0 && index < enumerable.Count();
    }
}
