using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMagnetism : MonoBehaviour
{
    public float baseMagnetismDistance = 10f;
    public float magnetismSpeed = .03f;

    GameObject nearestPlayer = null;
    float distanceToPlayer;
    PlayerStats playerStats;

    private void Start()
    {
        playerStats = PlayerManagement.player.GetComponent<PlayerStats>();
    }

    void FixedUpdate()
    {
        nearestPlayer = PlayerManagement.player;
        if (nearestPlayer == null)
        {
            Debug.LogWarning(gameObject.name + " could not find a player!");
            return;
        }
        else
        {
            distanceToPlayer = Vector2.Distance(nearestPlayer.transform.position, transform.position);
        }

        // Move towards player if they are in magnetism range.
        if (distanceToPlayer < playerStats.CalculateMagnetismDistance(baseMagnetismDistance))
        {
            transform.position = Vector2.MoveTowards(transform.position, nearestPlayer.transform.position, magnetismSpeed);
        }
    }

    public void Anim_EnableMagnetism()
    {
        this.enabled = true;
    }
}
