using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.Pool;

public class EnemySpawner : MonoBehaviour
{
    [Header("Wave Parameters")]
    public float enemiesPerSecond;
    public float waveDuration;
    public int waveToSpawnBoss;
    public float waveCooldownTimer;
    public GameObject bossToSpawn;
    public GameObject[] enemiesToSpawn;
    public float[] enemyDistribution;

    [HideInInspector]
    public int enemiesAlive = 0;
    [HideInInspector]
    public int enemiesKilled = 0;
    [HideInInspector]
    public int currentWave = 0;
    [HideInInspector]
    public int enemiesRemaining;

    [Space(20)]

    [Header("Event Info")]
    public float eventRate;
    public int surroundEventEnemyCount;

    public delegate void OnEnemyDeath(BaseEnemy enemy, EnemySpawner enemySpawner);
    public event OnEnemyDeath onEnemyDeath;

    public delegate void OnBossSpawn(GameObject boss);
    public event OnBossSpawn onBossSpawn;

    List<Vector3> spawnPointList = new List<Vector3>();
    GameplayUI gameplayUI;
    int waveSize;
    Coroutine waveCountdownCo;
    bool hasSpawnedBoss = false;
    PoolManager poolManager;


    void Start()
    {
        poolManager = GetComponent<PoolManager>();
        InvokeRepeating("SpawnEvent", eventRate, eventRate);
        waveCountdownCo = StartCoroutine(WaveCountdownCo());
        CheckEnemyDistribution();
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
            StartCoroutine(BeginWaveCo(waveDuration, enemiesPerSecond));
        }
    }

    IEnumerator BeginWaveCo(float waveDuration, float enemiesPerSecond)
    {
        gameplayUI.ToggleWaveInfoText(false);
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
            SpawnEnemy();
            enemiesSpawned++;
            yield return new WaitForSeconds(1f / enemiesPerSecond);
        }
    }

    IEnumerator WaveCountdownCo()
    {
        gameplayUI.ToggleWaveInfoText(true);
        string counterString;

        if (ShouldSpawnBoss())
        {
            counterString = "An elite enemy\napproaches in ";
        }
        else
        {
            counterString = "Wave " + (currentWave + 1).ToString() + "\n begins in ";
        }

        for (int i = 0; i < waveCooldownTimer; i++)
        {
            
            gameplayUI.UpdateWaveInfoText(counterString + (waveCooldownTimer - i).ToString());
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

    // Check that our enemy and distribution arrays are of the same size and that the 
    // distribution array sums to 100.
    void CheckEnemyDistribution()
    {
        Debug.Assert(enemyDistribution.Length == enemiesToSpawn.Length, "Enemy to spawn array and distribution array should be the same size.");
        float sum = 0;
        int index = 0;

        while (index < enemyDistribution.Length)
        {
            sum += enemyDistribution[index];
            index++;
        }

        Debug.Assert(sum == 100f, "Distribution of enemies doesn't sum to 100.");
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
        ObjectPool<GameObject> enemyPool = poolManager.GetObjectPool(enemyToSpawn);
        GameObject enemy = enemyPool.Get();
        BaseEnemy enemyInfo = enemy.GetComponent<BaseEnemy>();
        enemyInfo.SetEnemySpawner(this, enemyPool);
        // If we're not already pooled + parented, find our parent
        if (enemy.transform.parent == null) 
        {
            string poolName = string.Concat(enemyToSpawn.name, " Pool"); 
            enemy.transform.parent = GameObject.Find(poolName).transform;
        }
        enemy.transform.position = spawnPosition;
        enemiesAlive++;
        return enemyToSpawn;
    }

    GameObject GetRandomEnemyToSpawn()
    {
        float roll = Random.Range(0f, 100f);
        int index = 0;
        float sum = 0f;
   
        while (index < enemyDistribution.Length)
        {
            sum += enemyDistribution[index];
            
            if (roll < sum)
            {
                return enemiesToSpawn[index];
            }
            else
            {
                index++;
                continue;
            }
        }

        Debug.LogError("Enemy not found");
        return null;
       // return Random.Range(0f, 100f) <= enemyDistribution[0] ? enemiesToSpawn[0] : enemiesToSpawn[1];
    }

    Vector2 GetRandomSpawnPosition()
    {
        float offScreenXOffset = Screen.width * .1f;
        float offScreenYOffset = Screen.height * .1f;
        Vector2 leftScreenPos = new Vector2(-offScreenXOffset, Random.Range(0, Screen.height));
        Vector2 topScreenPos = new Vector2(Random.Range(0, Screen.width), Screen.height + offScreenYOffset);
        Vector2 rightScreenPos = new Vector2(Screen.width + offScreenXOffset, Random.Range(0, Screen.height));
        Vector2 bottomScreenPos = new Vector2(Random.Range(0, Screen.width), -offScreenYOffset);

        Vector2 leftMiddleWorldPos = Camera.main.ScreenToWorldPoint(leftScreenPos);
        Vector2 topMiddleWorldPos = Camera.main.ScreenToWorldPoint(topScreenPos);
        Vector2 rightMiddleWorldPos = Camera.main.ScreenToWorldPoint(rightScreenPos);
        Vector2 bottomMiddleWorldPos = Camera.main.ScreenToWorldPoint(bottomScreenPos);

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
            return Vector2.zero;

        Vector2 spawnPosition = spawnPointList[Random.Range(0, spawnPointList.Count)];
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
        gameplayUI.ToggleWaveInfoText(false);

        if (waveCountdownCo != null)
        {
            StopCoroutine(waveCountdownCo);
        }

        Vector3 spawnPosition = GetRandomSpawnPosition();
        if (spawnPosition == Vector3.zero)
            Debug.LogError("Failed to find spawn location for boss");

        Debug.Log("Spawning Boss");

        GameObject boss = Instantiate(bossToSpawn, spawnPosition, Quaternion.identity);
        BaseEnemy bossInfo = boss.GetComponent<BaseEnemy>();
        bossInfo.SetEnemySpawner(this, null);

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
