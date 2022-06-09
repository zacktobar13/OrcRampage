using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    public float spawnRate;
    public GameObject[] enemiesToSpawn;
    public GameObject bossToSpawn;
    public float[] spawnChances;
    public int waveToSpawnBoss;
    public float waveCooldownTimer;

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

    public delegate void OnBossSpawn(GameObject boss);
    public event OnBossSpawn onBossSpawn;

    public int currentWave = 0;
    public int waveSize;
    public int enemiesRemaining;

    GameplayUI gameplayUI;

    Coroutine waveCountdownCo;

    bool hasSpawnedBoss = false;

    void Start()
    {
        player = PlayerManagement.player.transform;
        InvokeRepeating("SpawnEvent", eventRate, eventRate);
        waveCountdownCo = StartCoroutine(WaveCountdownCo());
    }

    public void BeginWave()
    {
        if (hasSpawnedBoss)
            return;

        if (ShouldSpawnBoss())
        {
            SpawnBoss();
        }
        else
        {
            StartCoroutine(BeginWaveCo(5f, 1f));
        }
    }

    IEnumerator BeginWaveCo(float waveDuration, float enemiesPerSecond)
    {
        if (waveCountdownCo != null)
        {
            StopCoroutine(waveCountdownCo);
        }
        gameplayUI.EnableCurrentWaveText();
        currentWave++;
        gameplayUI.UpdateCurrentWaveText(currentWave);
        int enemiesSpawned = 0;
        waveSize = (int) (waveDuration * enemiesPerSecond);
        enemiesRemaining += waveSize;
        while (enemiesSpawned < waveSize)
        {
            yield return new WaitForSeconds(1f / enemiesPerSecond);
            SpawnEnemy();
            enemiesSpawned++;
        }
    }

    IEnumerator WaveCountdownCo()
    {
        for (int i = 0; i < waveCooldownTimer; i++)
        {
            Debug.Log("Spawning Wave in... " + (waveCooldownTimer - i));
            yield return new WaitForSeconds(1);
        }

        BeginWave();
    }

    public void UpdateGameplayUIReference(GameplayUI gameUI)
    {
        gameplayUI = gameUI;
    }

    GameObject SpawnEnemy()
    {
        return SpawnEnemy(null, null);
    }


    GameObject SpawnEnemy(Vector3? forcedPosition = null, GameObject forcedEnemyType = null)
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
                return null;
        }

        GameObject enemyToSpawn = forcedEnemyType == null ? GetRandomEnemyToSpawn() : forcedEnemyType;
        Instantiate(enemyToSpawn, spawnPosition, transform.rotation);
        enemiesAlive++;
        return enemyToSpawn;
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
        enemiesAlive--;
        enemiesRemaining--;

        if (onEnemyDeath != null)
        {
            onEnemyDeath(enemy, this);
        }

        if (ShouldBeginWaveCountDown())
        {
            waveCountdownCo = StartCoroutine(WaveCountdownCo());
        }
    }

    void SpawnEvent()
    {
        System.Action[] events = { SpawnSurroundEvent };
        System.Action eventToTrigger = events[Random.Range(0, events.Length)];
        eventToTrigger.Invoke();
    }

    public bool ShouldSpawnBoss()
    {
        return currentWave == waveToSpawnBoss - 1;
    }

    public bool ShouldBeginWaveCountDown()
    {
        return enemiesRemaining == 0 && !hasSpawnedBoss;
    }

    public void SpawnBoss()
    {
        if (waveCountdownCo != null)
        {
            StopCoroutine(waveCountdownCo);
        }

        Vector3 spawnPosition = GetRandomSpawnPosition();
        if (spawnPosition == Vector3.zero)
            Debug.LogError("Failed to find spawn location for boss");

        Debug.Log("Spawning Boss");
        GameObject boss = Instantiate(bossToSpawn, spawnPosition, Quaternion.identity);

        hasSpawnedBoss = true;

        if (onBossSpawn != null)
        {
            onBossSpawn(boss);
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
