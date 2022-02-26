using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    public Vector2 distanceFromPlayer;
    public float deactivateThresholdX;
    public float deactivateThresholdY;
    Transform player;

    void Start()
    {
        player = PlayerManagement.player.transform;
    }

    private void Update()
    {
        distanceFromPlayer.x = player.position.x - transform.position.x;
        distanceFromPlayer.y = player.position.y - transform.position.y;

        if (Mathf.Abs(distanceFromPlayer.x) > deactivateThresholdX)
        {
            gameObject.SetActive(false);
        }

        if (Mathf.Abs(distanceFromPlayer.y) > deactivateThresholdY)
        {
            gameObject.SetActive(false);
        }
    }

}
