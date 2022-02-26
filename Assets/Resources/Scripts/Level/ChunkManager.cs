using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkManager : MonoBehaviour
{
    public GameObject[] defaultAvailableChunks;
    Transform player;

    void Start()
    {
        player = PlayerManagement.player.transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
