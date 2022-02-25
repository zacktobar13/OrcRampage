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

    public static int GetPlayerFacingDirection(float angle)
    {
        if ( angle < 105 && angle >= 75 ) // Facing up
        {
            return 0;
        }
        else if ( angle >= 105 && angle < 180 ) // Facing up left
        {
            return 5;
        }
        else if (angle > -180 && angle <= -105) // Facing down left
        {
            return 4;
        }
        else if (angle > -105 && angle < -75) // Facing down
        {
            return 3;
        }
        else if (angle >= -75 && angle < 0) // Facing down right
        {
            return 2;
        }
        else if(angle >= 0 && angle < 75 ) // Facing up right
        {
            return 1;
        }

        Debug.LogError("Player facing direction angle out of range: " + angle.ToString());
        return -1;
    }

    public static int Mod(int x, int n)
    {
        return (x % n + n) % n;
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
