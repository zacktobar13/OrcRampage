using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionOnKillAffix : BaseAffix
{
    GameObject explosion;
    EnemySpawner enemySpawner;

    float chanceForExplosion;

    void Start()
    {
        explosion = StaticResources.explosionSmall;
        Debug.Assert(explosion != null);
        enemySpawner = GameObject.Find("Game Management").GetComponent<EnemySpawner>();
        enemySpawner.onEnemyDeath += OnEnemyDeath;
    }

    public void OnEnemyDeath(BaseEnemy enemy)
    {
        bool willExplode = Random.Range(0f, 100f) <= chanceForExplosion;
        if (!willExplode)
            return;

        GameObject explosionSpawned = Instantiate(explosion, enemy.transform.position, Quaternion.identity);
        ExplosionBehavior explosionBehavior = explosion.GetComponent<ExplosionBehavior>();
        explosionBehavior.damageAmount = playerStats.CalculateDamage(false, 2);
    }

    public override void AddAffixCount(int increment)
    {
        base.AddAffixCount(increment);
        chanceForExplosion += baseProcChance;
    }

    public override void IntializeFromScriptableObject(AffixObject data)
    {
        base.IntializeFromScriptableObject(data);
        chanceForExplosion = baseProcChance;
    }

    private void OnDestroy()
    {
        enemySpawner.onEnemyDeath -= OnEnemyDeath;
    }
}
