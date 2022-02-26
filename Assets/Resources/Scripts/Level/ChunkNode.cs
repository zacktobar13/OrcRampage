using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkNode : MonoBehaviour
{
    public GameObject[] availableChunksOverride;
    public GameObject[] availableChunks;
    ChunkManager chunkManager;
 
    void Start()
    {
        chunkManager = GameObject.Find("Chunk Layout").GetComponent<ChunkManager>();
        if (availableChunks.Length > 0)
        {
            availableChunks = availableChunksOverride.Length > 0 ? availableChunksOverride : chunkManager.defaultAvailableChunks;
            ChooseNodeToSpawn();
        }
    }

    void ChooseNodeToSpawn()
    {
        Instantiate(availableChunks[Random.Range(0, availableChunks.Length)], transform.position, Quaternion.identity);
    }
}
