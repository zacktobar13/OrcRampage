using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpewerEnemyBehavior : MeleeEnemyBehavior
{
    public GameObject projectile;
    public Transform projectileSpawn;

    public override IEnumerator StartAttack()
    {
        isAttacking = true;

        // Telegraph the attack
        spriteAnim.Play(telegraphAnimation);
        Vector2 directionToPlayer = (target.transform.position - transform.position).normalized;
        float rotationAmount = Utility.RotationAmount(transform.position, target.transform.position);
        attackMoveEndPos = (Vector2)transform.position + directionToPlayer.normalized * 1.75f;
        yield return new WaitForSeconds(telegraphDuration);

        // Do the actual attack
        spriteAnim.Play(attackAnimation);
        attackMove = moveWhileAttacking();
        StartCoroutine(attackMove);
        GameObject myProjectile = Instantiate(projectile, projectileSpawn.position, projectileSpawn.rotation);
        SpewerProjectile projectileBehavior = myProjectile.GetComponent<SpewerProjectile>();
        projectileBehavior.movementDirection = directionToPlayer;
        projectileBehavior.projectileSpriteGameObject.transform.rotation = Quaternion.Euler(0f, 0f, rotationAmount);
        projectileBehavior.shadowGameObject.transform.rotation = Quaternion.Euler(0f, 0f, rotationAmount);
        yield return new WaitForSeconds(0.5f);

        AttackEnd();
    }
}
