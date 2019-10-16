using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectorService : MonoBehaviour
{
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
