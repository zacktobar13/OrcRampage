using UnityEngine;

/// <summary>
/// Grabs a reference from the Level Info game object in the scene and rolls a random enemy from its array of
/// possible enemies.
/// </summary>

public class EnemySpawnPoint : MonoBehaviour
{
    int randomRoll;
    LevelInfo levelInfo;

    void Start()
    {
        levelInfo = GameObject.Find("Level Info").GetComponent<LevelInfo>();

        if (levelInfo != null)
        {
            randomRoll = (int)Random.Range(0, levelInfo.enemies.Length);
            WaveManager.aliveEnemies.AddLast(Instantiate(levelInfo.enemies[randomRoll], transform.position, transform.rotation));
        }
        else
        {
            Debug.LogError("Reference to Level Info on " + gameObject.name + " is null.");
        }
    }
}
