using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkNode : MonoBehaviour
{
    public GameObject[] availableChunksOverride;
    ChunkManager chunkManager;
    public GameObject[] availableChunks;
 
    void Start()
    {
        chunkManager = GameObject.Find("Chunk Layout").GetComponent<ChunkManager>();
        availableChunks = availableChunksOverride.Length > 0 ? availableChunksOverride : chunkManager.defaultAvailableChunks;
        ChooseNodeToSpawn();
    }

    void ChooseNodeToSpawn()
    {
        Instantiate(availableChunks[Random.Range(0, availableChunks.Length)], transform.position, Quaternion.identity);
    }
}
