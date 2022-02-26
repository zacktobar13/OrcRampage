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
    
    // Run away after attack logic
    bool isRunningAway;
    Vector2 runAwayDirection;

    // Wander logic
    bool isCurrentlyWandering = false;
    float lastWanderTime;
    float wanderCooldown = 3f;
    float wanderStartTime;
    float maxWanderTime = 5f;

    Vector2 wanderPosition = Vector2.zero;

    public override void ChaseTarget()
    {
        
        if (IsAttackOnCooldown())
        {
            // Run away from target
            if (!isRunningAway)
            {
                // Initialize runAway direction and stick to it until you can attack again
                Vector2 directionToEnemy = (transform.position - target.transform.position).normalized;
                runAwayDirection = (Vector2)transform.position + (directionToEnemy);
                runAwayDirection = Utility.Rotate(runAwayDirection, Random.Range(-180, 180));
                runAwayDirection *= 100;
                isRunningAway = true;
            }

            spriteAnim.Play(chaseAnimation);
            MoveTowards(runAwayDirection, movementSpeed * Time.deltaTime);
        }
        else if (distanceToTarget >= attackRange)
        {
            // Chase Target
            isRunningAway = false;
            spriteAnim.Play(chaseAnimation);
            MoveTowards(target.transform.position, movementSpeed * Time.deltaTime);
        }
        else
        {
            // Idle
            isRunningAway = false;
            spriteAnim.Play(idleAnimation);
        }
    }

    public override void Attack()
    {
        if (ShouldAttack())
        {
            currentWeapon.Attack();
            lastAttackTime = Time.time;
        }
    }

    public override bool ShouldAttack()
    {
        return (inRangeToAttack && Time.time > lastAttackTime + attackCooldown);
    }

    public virtual bool IsAttackOnCooldown()
    {
        return Time.time < lastAttackTime + attackCooldown;
    }

    public override void Idle()
    {
        if (ShouldWander())
        {
            //Wander();
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

	public override void Anim_EnableDeadShadow()
	{
		base.Anim_EnableDeadShadow();
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
}
