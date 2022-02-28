using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utility : MonoBehaviour {

    /// <summary>
    /// Returns the amount A has to rotate to face B in degrees.
    /// </summary>
    public static float RotationAmount( Vector2 origin, Vector2 target )
    {
        return Mathf.Atan2 ( target.y - origin.y, target.x - origin.x ) * Mathf.Rad2Deg;
    }

    public static bool InFirstQuadrant ( float angle )
    {
        return angle <= 90 && angle > 0;
    }

    public static bool InSecondQuadrant ( float angle )
    {
        return angle > 90 && angle <= 180;
    }

    public static bool InThirdQuadrant ( float angle )
    {
        return angle <= -90 && angle >= -180;
    }

    public static bool InFourthQuadrant ( float angle )
    {
        return angle >= -90 && angle < 0;
    }

    public static Vector2 Rotate(Vector2 v, float delta)
    {
        delta *= Mathf.Deg2Rad;
        return new Vector2(
            v.x * Mathf.Cos(delta) - v.y * Mathf.Sin(delta),
            v.x * Mathf.Sin(delta) + v.y * Mathf.Cos(delta)
        );
    }

    public static Vector3 Rotate(Vector3 v, float delta)
    {
        return Quaternion.Euler(0, delta, 0) * v;
    }

    public static int Mod(int x, int n)
    {
        return (x % n + n) % n;
    }

    public static T[] RemoveAt<T>(T[] source, int index)
    {
        T[] dest = new T[source.Length - 1];
        if (index > 0)
            System.Array.Copy(source, 0, dest, 0, index);

        if (index < source.Length - 1)
            System.Array.Copy(source, index + 1, dest, index, source.Length - index - 1);

        return dest;
    }
    public static void Shuffle<T>(T[] array)
    {
        for (int i = 0; i < array.Length; i++)
        {
            int newIndex = Random.Range(0, array.Length);
            T temp = array[i];
            array[i] = array[newIndex];
            array[newIndex] = temp;
        }
    }

    public static float EaseOutQuad( float start, float end, float value )
    {
        end -= start;
        return -end * value * ( value - 2 ) + start;
    }

    public static float EaseOutSine( float start, float end, float value )
    {
        end -= start;
        return end * Mathf.Sin( value * ( Mathf.PI * 0.5f ) ) + start;
    }
}
