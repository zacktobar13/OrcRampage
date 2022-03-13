using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    public float spawnRate;
    public GameObject[] enemiesToSpawn;
    public float[] spawnChances;
    List<Vector3> spawnPointList = new List<Vector3>();

    Transform player;

    int enemiesAlive = 0;

    public delegate void OnEnemyDeath(BaseEnemy enemy);
    public event OnEnemyDeath onEnemyDeath;
    

    void Start()
    {
        player = PlayerManagement.player.transform;
        InvokeRepeating("SpawnEnemy", 0f, spawnRate);
    }

    public void SpawnEnemy()
    {
        float offScreenXOffset = Screen.width * .1f;
        float offScreenYOffset = Screen.height * .1f;
        Vector3 leftScreenPos = new Vector3(-offScreenXOffset, Random.Range(0, Screen.height), 0);
        Vector3 topScreenPos = new Vector3(Random.Range(0, Screen.width), Screen.height + offScreenYOffset, 0);
        Vector3 rightScreenPos = new Vector3(Screen.width + offScreenXOffset, Random.Range(0, Screen.height), 0);
        Vector3 bottomScreenPos = new Vector3(Random.Range(0, Screen.width), -offScreenYOffset, 0);

        Vector3 leftMiddleWorldPos = Camera.main.ScreenToWorldPoint(leftScreenPos);
        Vector3 topMiddleWorldPos = Camera.main.ScreenToWorldPoint(topScreenPos);
        Vector3 rightMiddleWorldPos = Camera.main.ScreenToWorldPoint(rightScreenPos);
        Vector3 bottomMiddleWorldPos = Camera.main.ScreenToWorldPoint(bottomScreenPos);
        
        if (SpaceAvailable(leftMiddleWorldPos))
        {
            spawnPointList.Add(leftMiddleWorldPos);
        }
        if (SpaceAvailable(rightMiddleWorldPos))
        {
            spawnPointList.Add(rightMiddleWorldPos);
        }
        if (SpaceAvailable(topMiddleWorldPos))
        {
            spawnPointList.Add(topMiddleWorldPos);
        }
        if (SpaceAvailable(bottomMiddleWorldPos))
        {
            spawnPointList.Add(bottomMiddleWorldPos);
        }
       
        float randomRoll = Random.Range(0f, 100f);

        if (spawnPointList.Count > 0)
        {
            if (randomRoll <= spawnChances[0])
            {
                BaseEnemy enemy = Instantiate(enemiesToSpawn[0], spawnPointList[Random.Range(0, spawnPointList.Count)], transform.rotation).GetComponent<BaseEnemy>();
                BaseEnemy enemy2 = Instantiate(enemiesToSpawn[0], spawnPointList[Random.Range(0, spawnPointList.Count)], transform.rotation).GetComponent<BaseEnemy>(); 
            }
            else
            {
                BaseEnemy enemy = Instantiate(enemiesToSpawn[1], spawnPointList[Random.Range(0, spawnPointList.Count)], transform.rotation).GetComponent<BaseEnemy>();
                BaseEnemy enemy2 = Instantiate(enemiesToSpawn[1], spawnPointList[Random.Range(0, spawnPointList.Count)], transform.rotation).GetComponent<BaseEnemy>();
            }
        }

        spawnPointList.Clear();
        enemiesAlive += 2;
    }

    public bool SpaceAvailable(Vector3 position)
    {
        return !Physics2D.Raycast(position, Vector3.forward);
    }

    public void EnemyDeath(BaseEnemy enemy)
    {
        if (onEnemyDeath != null)
        {
            onEnemyDeath(enemy);
        }

        enemiesAlive--;
    }
}
