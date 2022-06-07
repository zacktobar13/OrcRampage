using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    public float spawnRate;
    public GameObject[] enemiesToSpawn;
    public float[] spawnChances;
    public int enemiesToSpawnBoss;

    [Header("Event Info")]
    public float eventRate;
    public int surroundEventEnemyCount;

    List<Vector3> spawnPointList = new List<Vector3>();

    [HideInInspector]
    public int enemiesAlive = 0;
    [HideInInspector]
    public int enemiesKilled = 0;

    Transform player;

    public delegate void OnEnemyDeath(BaseEnemy enemy, EnemySpawner enemySpawner);
    public event OnEnemyDeath onEnemyDeath;

    public delegate void OnBossSpawn();
    public event OnBossSpawn onBossSpawn;

    public int currentWave = 0;
    public int waveSize;
    public int enemiesRemaining;

    GameplayUI gameplayUI;

    void Start()
    {
        player = PlayerManagement.player.transform;
        InvokeRepeating("SpawnEvent", eventRate, eventRate);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            StartCoroutine(BeginWave(5f, 1f));
        }
    }

    public void UpdateGameplayUIReference(GameplayUI gameUI)
    {
        gameplayUI = gameUI;
    }

    void SpawnEnemy()
    {
        SpawnEnemy(null, null);
    }

    public IEnumerator BeginWave(float waveDuration, float enemiesPerSecond)
    {
        gameplayUI.EnableCurrentWaveText();
        currentWave++;
        gameplayUI.UpdateCurrentWaveText(currentWave);
        int enemiesSpawned = 0;
        waveSize = (int) (waveDuration * enemiesPerSecond);
        while (enemiesSpawned < waveSize)
        {
            yield return new WaitForSeconds(1f / enemiesPerSecond);
            SpawnEnemy();
            enemiesSpawned++;
        }
    }

    void SpawnEnemy(Vector3? forcedPosition = null, GameObject forcedEnemyType = null)
    {
        Vector3 spawnPosition;
        if (forcedPosition.HasValue && SpaceAvailable(forcedPosition.GetValueOrDefault()))
        {
            spawnPosition = forcedPosition.GetValueOrDefault();
        }
        else
        {
            spawnPosition = GetRandomSpawnPosition();
            if (spawnPosition == Vector3.zero)
                return;
        }

        GameObject enemyToSpawn = forcedEnemyType == null ? GetRandomEnemyToSpawn() : forcedEnemyType;
        Instantiate(enemyToSpawn, spawnPosition, transform.rotation);
        enemiesAlive++;
    }

    GameObject GetRandomEnemyToSpawn()
    {
        return Random.Range(0f, 100f) <= spawnChances[0] ? enemiesToSpawn[0] : enemiesToSpawn[1];
    }

    Vector3 GetRandomSpawnPosition()
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

        if (spawnPointList.Count == 0)
            return Vector3.zero;

        Vector3 spawnPosition = spawnPointList[Random.Range(0, spawnPointList.Count)];
        spawnPointList.Clear();
        return spawnPosition;
    }

    public bool SpaceAvailable(Vector3 position)
    {
        return !Physics2D.Raycast(position, Vector3.forward);
    }

    public void EnemyDeath(BaseEnemy enemy, EnemySpawner enemySpawner)
    {
        enemiesKilled++;

        if (onEnemyDeath != null)
        {
            onEnemyDeath(enemy, this);
        }


        if (ShouldSpawnBoss())
        {
            SpawnBoss();
        }
        enemiesAlive--;
    }

    void SpawnEvent()
    {
        System.Action[] events = { SpawnSurroundEvent };
        System.Action eventToTrigger = events[Random.Range(0, events.Length)];
        eventToTrigger.Invoke();
    }

    public bool ShouldSpawnBoss()
    {
        return enemiesKilled >= enemiesToSpawnBoss;
    }

    public void SpawnBoss()
    {
        Debug.Log("Spawn Boss");

        if (onBossSpawn != null)
        {
            onBossSpawn();
        }
    }

    void SpawnSurroundEvent()
    {
        Debug.Log("Spawning Surround Event");
        Vector2[] enemySpawnDirections = new Vector2[surroundEventEnemyCount];
        Vector2 originalDirection = new Vector2(1, 0).normalized;
        Vector2 rotatedDirection = originalDirection;

        for (int i = 0; i < surroundEventEnemyCount; i++)
        {
            enemySpawnDirections[i] = rotatedDirection;
            float rotationIncrement = (360f / surroundEventEnemyCount) * (i + 1);
            rotatedDirection = Utility.Rotate(originalDirection, rotationIncrement);
        }

        float magnitude = new Vector2(Screen.width, Screen.height).magnitude / 2;
        Vector2 cameraCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
        GameObject enemyToSpawn = GetRandomEnemyToSpawn();

        foreach (Vector2 direction in enemySpawnDirections)
        {
            Vector2 spawnPosition = Camera.main.ScreenToWorldPoint(cameraCenter + direction * magnitude);
            SpawnEnemy(spawnPosition, enemyToSpawn);
        }
    }
}
