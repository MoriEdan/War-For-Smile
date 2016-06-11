using System;
using UnityEngine;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using Assets.Scripts.Helpers;

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

    public static string GetDescription(this Enum type)
    {
        var attr = type.GetType()
            .GetField(type.ToString())
            .GetCustomAttributes(typeof(DescriptionAttribute), false)
            .SingleOrDefault() as DescriptionAttribute;

        return attr == null ? type.ToString() : attr.Description;
    }
}
