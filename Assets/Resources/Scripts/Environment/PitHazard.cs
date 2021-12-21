using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitHazard : MonoBehaviour
{
    public delegate void OnPlayerEnterPitHazard(Collider2D hazard, Collider2D outerEdge);
    public static event OnPlayerEnterPitHazard onPlayerEnterPitHazard;

    public delegate void OnPlayerExitPitHazard();
    public static event OnPlayerExitPitHazard onPlayerExitPitHazard;

    private Collider2D hazardCollider;
    private Collider2D outerEdgeCollider;

    private void Start()
    {
        hazardCollider = GetComponent<Collider2D>();
        outerEdgeCollider = transform.parent.GetComponent<Collider2D>();
        Debug.Assert(outerEdgeCollider != null);
        Debug.Assert(hazardCollider != null);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        string colliderTag = collision.gameObject.tag;

        if (colliderTag == "Player")
        {
            if (onPlayerEnterPitHazard != null)
            {
                onPlayerEnterPitHazard(hazardCollider, outerEdgeCollider);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        string colliderTag = collision.gameObject.tag;

        if (colliderTag == "Player")
        {
            if (onPlayerEnterPitHazard != null)
            {
                onPlayerExitPitHazard();
            }
        }
    }
}
