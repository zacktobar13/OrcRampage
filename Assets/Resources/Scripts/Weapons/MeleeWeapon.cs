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

        StartCoroutine(VisualEffects());
        PlayAttackSound();

        foreach (Collider2D collider in objectsHit)
        {
            BaseEnemy enemyHit;
            if (isOnPlayer && collider.TryGetComponent(out enemyHit))
            {
                DamageInfo damageInfo = generateDamageInfo(transform.parent.position, collider.transform.position);
                enemyHit.ApplyDamage(damageInfo);
                continue;
            }
            PlayerHealth playerHealth;
            if (!isOnPlayer && collider.TryGetComponent(out playerHealth))
            {
                DamageInfo damageInfo = generateDamageInfo(transform.parent.position, collider.transform.position);
                playerHealth.ApplyDamage(damageInfo);
                continue;
            }
            MapClutter mapClutter;
            if (collider.TryGetComponent(out mapClutter))
            {
                DamageInfo damageInfo = generateDamageInfo(transform.parent.position, collider.transform.position);
                mapClutter.ApplyDamage(damageInfo);
                continue;
            }
        }

        lastAttackTime = Time.time;
    }

    DamageInfo generateDamageInfo(Vector2 weaponPosition, Vector2 targetPosition)
    {
        Vector2 damageDirection = (targetPosition - weaponPosition).normalized;
        return new DamageInfo(attackPower, RollCrit(), damageDirection);
    }

    private void OnDrawGizmos()
    {
        if (isActiveAndEnabled && projectileSpawn)
            Gizmos.DrawSphere(projectileSpawn.transform.position, attackRadius);
    }
}
