using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public static class IntegerExtensions
{
    public static bool IsPowerOfTwo(this int value) {
        return (value > 0) && ((value & (value - 1)) == 0);
    }
}
