using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PowerTools;

public class MeleeEnemyBehavior : BaseEnemy
{
    [Header("Melee Attributes")]
    public float attackCooldown;
    public float telegraphDuration;
    public float wanderMovementSpeed;
    public float damageColliderDistance;

    protected float lastAttack;
    protected bool isAttacking = false;
    protected Vector2 attackMoveEndPos;

    protected IEnumerator attack;
    protected IEnumerator attackMove;

    bool makingIdleNoise = false;
    bool isCurrentlyWandering = false;

    float lastWanderTime;
    float wanderCooldown = 3f;
    float wanderStartTime;
    float maxWanderTime = 5f;


    Vector2 wanderPosition = Vector2.zero;

    public override void ChaseTarget()
    {
        if (distanceToTarget >= 3)
        {
            spriteAnim.Play(chaseAnimation);
            MoveTowards(target.transform.position, movementSpeed * Time.deltaTime);
        }
        else
        {
            spriteAnim.Play(idleAnimation);
        }
    }

    public override void Attack()
    {
        if (attack == null)
        {
            attack = StartAttack();
            StartCoroutine(attack);
        }
    }

    public override bool ShouldAttack()
    {
        return isAttacking || (inRangeToAttack && Time.time > lastAttack + attackCooldown);
    }

    public virtual IEnumerator StartAttack ()
    {
        isAttacking = true;

        // Telegraph the attack
        spriteAnim.Play(telegraphAnimation);
        Vector2 directionToPlayer = (target.transform.position - transform.position).normalized;
        attackMoveEndPos = (Vector2)transform.position + directionToPlayer.normalized * 1.75f;
        yield return new WaitForSeconds(telegraphDuration);

        // Do the actual attack
        spriteAnim.Play(attackAnimation);
        attackMove = moveWhileAttacking();
        StartCoroutine(attackMove);
        GameObject damageCircle = Instantiate(damageCollider, damageSpawnPoint.transform.position, transform.rotation);
        DamageColliderBehavior circle = damageCircle.GetComponent<DamageColliderBehavior>();
        circle.creator = gameObject;
        circle.damageAmount = attackDamage;
        circle.damageDirection = directionToPlayer;
        circle.transform.position = (Vector2)damageSpawnPoint.position + directionToPlayer * damageColliderDistance;
        yield return new WaitForSeconds(0.5f);

        AttackEnd();
    }

    public void AttackEnd(bool goToIdle = true)
    {
        if (attackMove != null)
        {
            StopCoroutine(attackMove);
            attackMove = null;
        }
        isAttacking = false;
        lastAttack = Time.time;
        if (goToIdle)
            spriteAnim.Play(idleAnimation);
        attack = null;
    }

    public override void Idle()
    {
        if (ShouldWander())
        {
            Wander();
        }
        else
        {
            spriteAnim.Play(idleAnimation);
        }
    }

    public virtual bool ShouldWander()
    {

        // Have we been wandering for too long?
        if (isCurrentlyWandering && Time.time > wanderStartTime + maxWanderTime)
        {
            WanderEnd();
            return false;
        }

        // Have we reached our wander target?
        if ((Vector2)transform.position == wanderPosition)
        {
            WanderEnd();
            return false;
        }

        // Have we wandered too recently?
        if (lastWanderTime != 0 && Time.time < lastWanderTime + wanderCooldown)
        {
            return false;
        }

        return true;
    }

    public virtual void Wander()
    {
        // We haven't chosen a wander position yet
        if (wanderPosition == Vector2.zero)
        {
            WanderBegin();

            wanderPosition.x = transform.position.x + Random.Range(-10f, 10f);
            wanderPosition.y = transform.position.y + Random.Range(-10f, 10f);
        }

        spriteAnim.Play(wanderAnimation);
        MoveTowards(wanderPosition, wanderMovementSpeed * Time.deltaTime);
    }

    public void WanderEnd()
    {
        isCurrentlyWandering = false;
        wanderPosition = Vector2.zero;
        lastWanderTime = Time.time;
    }

    public void WanderBegin()
    {
        isCurrentlyWandering = true;
        wanderStartTime = Time.time;
    }

    public IEnumerator moveWhileAttacking()
    {
        while (true)
        {
            if ((Vector2)transform.position == attackMoveEndPos)
                break;
            MoveTowards(attackMoveEndPos, movementSpeed * Time.deltaTime * 3.5f);
            yield return new WaitForEndOfFrame();
        }
    }

    public override void ApplyDamage(DamageInfo damageInfo)
    {
        base.ApplyDamage(damageInfo);
        if (attack != null)
        {
            StopCoroutine(attack);
            AttackEnd(false);
        }
    }
}
