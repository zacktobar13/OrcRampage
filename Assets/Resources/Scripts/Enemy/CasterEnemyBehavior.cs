using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CasterEnemyBehavior : BaseEnemy
{
    [Header("Caster Enemy")]
    public GameObject projectile;
    public float warningDuration;

    public override void Attack()
    {
        Vector2 targetPos = target.transform.position;
        GameObject spawnedProjectile = Instantiate(projectile, targetPos, Quaternion.identity);
        EnemyProjectile projectileInfo = spawnedProjectile.GetComponent<EnemyProjectile>();
        projectileInfo.SetProjectileInfo(CalculateAttackDamage(), attackRadius, warningDuration);
    }
}
