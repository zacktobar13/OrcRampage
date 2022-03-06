using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public float spawnRate;
    public GameObject[] enemiesToSpawn;
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

        Vector3[] spawnLocations = { leftMiddleWorldPos, topMiddleWorldPos, rightMiddleWorldPos, bottomMiddleWorldPos };

        int randomRoll = (int)Random.Range(0, enemiesToSpawn.Length);
        BaseEnemy enemy = Instantiate(enemiesToSpawn[randomRoll], spawnLocations[Random.Range(0, spawnLocations.Length)], transform.rotation).GetComponent<BaseEnemy>();
        enemy.SetRarity(RollEnemyRarity());
        enemiesAlive++;
    }

    public void EnemyDeath(BaseEnemy enemy)
    {
        if (onEnemyDeath != null)
        {
            onEnemyDeath(enemy);
        }

        enemiesAlive--;
    }

    private RarityUtil.Rarity RollEnemyRarity()
    {
        float roll = Random.Range(0f, 100f);

        if (roll <= 95f)
            return RarityUtil.Rarity.COMMON;
        else if (roll <= 99.3f)
            return RarityUtil.Rarity.UNCOMMON;
        else if (roll <= 99.6f)
            return RarityUtil.Rarity.MAGIC;
        else if (roll <= 99.8f)
            return RarityUtil.Rarity.EPIC;
        else if (roll <= 99.95f)
            return RarityUtil.Rarity.LEGENDARY;
        else
            return RarityUtil.Rarity.ANCIENT;
    }
}
