using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMagnetism : MonoBehaviour
{
    public float magnestismDistance = 15f;
    public float magnetismSpeed = .03f;

    GameObject nearestPlayer = null;
    float distanceToPlayer;

    void FixedUpdate()
    {
        if (nearestPlayer == null)
        {
            nearestPlayer = PlayerManagement.player;

            if (nearestPlayer == null)
            {
                Debug.LogWarning(gameObject.name + " could not find a player!");
                return;
            }
        }
        else
        {
            distanceToPlayer = Vector2.Distance(nearestPlayer.transform.position, transform.position);
        }

        // Move towards player if they are in magnetism range.
        if (distanceToPlayer < magnestismDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, nearestPlayer.transform.position, magnetismSpeed);
        }
    }

    public void Anim_EnableMagnetism()
    {
        this.enabled = true;
    }
}
