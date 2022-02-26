using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public float spawnRate;
    public GameObject[] enemiesToSpawn;
    Transform player;
    
    void Start()
    {
        player = PlayerManagement.player.transform;
        InvokeRepeating("SpawnEnemy", 0f, spawnRate);
    }

    public void SpawnEnemy()
    {
        int randomRoll = (int)Random.Range(0, enemiesToSpawn.Length);
        Instantiate(enemiesToSpawn[randomRoll], new Vector3(player.position.x, player.position.y - 20f, 0f), transform.rotation);
    }
}
