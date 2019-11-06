using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectorService : MonoBehaviour
{
    public static float ScalarMult2 (Vector3 v1, Vector3 v2)
    {
        return Mathf.Sqrt(v1.x * v2.x + v1.y * v2.y);
    }

    public static void RotateVector(ref Vector3 vector, float angle)
    {
        float newX = vector.x * Mathf.Cos(angle) - vector.y * Mathf.Sin(angle);
        float newY = vector.x * Mathf.Sin(angle) + vector.y * Mathf.Cos(angle);

        vector.x = newX;
        vector.y = newY;
    }

    public static void RotateVector(ref Vector2 vector, float angle)
    {
        float newX = vector.x * Mathf.Cos(angle) - vector.y * Mathf.Sin(angle);
        float newY = vector.x * Mathf.Sin(angle) + vector.y * Mathf.Cos(angle);

        vector.x = newX;
        vector.y = newY;
    }
}
