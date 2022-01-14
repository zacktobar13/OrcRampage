using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : Weapon
{
    [Header("Melee Weapon")]
    public float attackRadius;

    public override bool HasAmmo()
    {
        return true;
    }

    public override void Attack()
    {
        if (!CanAttack())
            return;

        Collider2D[] objectsHit = Physics2D.OverlapCircleAll(projectileSpawn.position, attackRadius);
        
        foreach(Collider2D collider in objectsHit)
        {
            BaseEnemy enemyHit;
            if (collider.TryGetComponent(out enemyHit))
            {
                Vector2 damageDirection = (collider.transform.position - transform.parent.position).normalized;
                DamageInfo damageInfo = new DamageInfo(attackPower, RollCrit(), damageDirection);
                enemyHit.ApplyDamage(damageInfo);
                continue;
            }
            MapClutter mapClutter;
            if (collider.TryGetComponent(out mapClutter))
            {
                Vector2 damageDirection = (collider.transform.position - transform.parent.position).normalized;
                DamageInfo damageInfo = new DamageInfo(attackPower, RollCrit(), damageDirection);
                mapClutter.ApplyDamage(damageInfo);
                continue;
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (isActiveAndEnabled && projectileSpawn)
            Gizmos.DrawSphere(projectileSpawn.transform.position, attackRadius);
    }
}
