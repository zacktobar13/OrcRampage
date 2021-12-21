using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{

    public Transform[] enemySpawnLocations;
    public GameObject[] enemies;
    public static LinkedList<GameObject> aliveEnemies = new LinkedList<GameObject>();

    public int waveNumber;
    int numberOfEnemiesToSpawn = 1;

    int numberOfEnemiesAlive;
    public int numberOfWavesToSpawn;
    public float waveCooldown = 0f;
    public float waveTimer = 0f;
    public float nextWaveTime = 0f;
    int wavesRemaining;
    bool waveCountdownStarted = false;

    public delegate void OnNewWave(int number);
    public static event OnNewWave onNewWave;

    public delegate void OnWaveCompleted(bool completed);
    public static event OnWaveCompleted onWaveCompleted;

    public delegate void OnEnemyDied(int number);
    public static event OnEnemyDied onEnemyDied;

    public delegate void OnExitSpawned();
    public static event OnExitSpawned onExitSpawned;

    public delegate void OnBossExitSpawned();
    public static event OnBossExitSpawned onBossExitSpawned;

    private void Awake()
    {
        aliveEnemies.Clear();
        onNewWave = null;
        onWaveCompleted = null;
        onEnemyDied = null;
        onExitSpawned = null;
        onBossExitSpawned = null;
    }

    private void Start()
    { 
        wavesRemaining = numberOfWavesToSpawn - waveNumber;
    }

    void Update()
    {

        if (aliveEnemies.Count > 0)
            return;

        if (wavesRemaining > 0)
        {
            waveTimer = nextWaveTime - Time.time;
            if (!waveCountdownStarted)
            {
                StartCoroutine("StartWaveCountdown");
                if (onWaveCompleted != null)
                    onWaveCompleted(true);
                waveCountdownStarted = true;
            }
        }
        else
        {
            // We're done spawning waves and all enemies are dead. Portal out
            //if ( onLastEnemyKilled != null )
            //{
            //    onLastEnemyKilled();
            //}
        }
    }

    public bool ShouldSpawnWave()
    {
        return aliveEnemies.Count == 0;
    }

    // TODO:
    //      If we get a frame hitch, we have a couple of options here:
    //          1) Frame defer the instantiations
    //          2) Pool up the zombies and instead of instantiate just activate + set transform
    public void SpawnNewWave()
    {
        for(int i = 0; i < numberOfEnemiesToSpawn; i++)
        {
            aliveEnemies.AddLast(Instantiate(enemies[Random.Range(0, enemies.Length)], enemySpawnLocations[Random.Range(0, enemySpawnLocations.Length)].position, Quaternion.identity));
        }

        waveCountdownStarted = false;
        if (onWaveCompleted != null)
            onWaveCompleted(false);
        waveNumber += 1;
        if (onNewWave != null)
            onNewWave(waveNumber);
        if (onEnemyDied != null)
            onEnemyDied(aliveEnemies.Count);

        numberOfEnemiesToSpawn += waveNumber * 2;
        wavesRemaining = numberOfWavesToSpawn - waveNumber;
    }

    IEnumerator StartWaveCountdown()
    {
        nextWaveTime = Time.time + waveCooldown;
        waveTimer = waveCooldown;
        yield return new WaitForSeconds(waveCooldown);
        SpawnNewWave();
    }

    public static void EnemyDied(GameObject deadEnemy)
    {
        aliveEnemies.Remove(deadEnemy);
        if (onEnemyDied != null)
            onEnemyDied(aliveEnemies.Count);

        // All enemies dead. Check if exit should be a boss or normal exit.
        if (aliveEnemies.Count == 0)
        {
            LevelInfo.levelsCompleted++;

            if (LevelInfo.levelsCompleted < LevelInfo.levelsUntilBoss)
            {
                if (onExitSpawned != null)
                    onExitSpawned();
            }
            else
            {
                if (onBossExitSpawned != null)
                    onBossExitSpawned();
            }
        }
    }
}
