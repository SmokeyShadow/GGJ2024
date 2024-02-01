using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utility
{
    #region STATIC METHODS
    public static bool AlmostEqual(Vector3 v1, Vector3 v2, float precision)
    {
        bool equal = true;

        if (Mathf.Abs(v1.x - v2.x) > precision)
            equal = false;
        if (Mathf.Abs(v1.y - v2.y) > precision)
            equal = false;
        if (Mathf.Abs(v1.z - v2.z) > precision)
            equal = false;

        return equal;
    }
    #endregion
}
