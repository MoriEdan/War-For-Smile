using UnityEngine;
using System.Collections;

public static class Extensions
{
    public static float ToAngle(this Vector3 vector)
    {
        return (float)Mathf.Atan2(vector.y, vector.x) * Mathf.Rad2Deg;
    }

    public static Vector3 FromPolar(float angle, float magnitude)
    {
        return magnitude * new Vector2((float)Mathf.Cos(angle), (float)Mathf.Sin(angle));
    }
}
