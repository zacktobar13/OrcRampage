using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitEdge : MonoBehaviour
{
    public delegate void OnPlayerEnterPitEdge();
    public static event OnPlayerEnterPitEdge onPlayerEnterPitEdge;

    private void OnTriggerStay2D(Collider2D collision)
    {
        string colliderTag = collision.gameObject.tag;

        if ( colliderTag == "Player" )
        {
            if ( onPlayerEnterPitEdge != null )
            {
                onPlayerEnterPitEdge();
            }
        }
    }
}
