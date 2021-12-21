using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargerEnemyBehavior : MeleeEnemyBehavior
{
    // The percentage of distance that we'll charge in our targets direction
    // 0 == Don't charge at all
    // 1 == Charge all the way to target
    // 2 == Charge twice the distance between me and target
    const float chargeDistanceScalar = 5f;
    const float maxmimumChargeDuration = 1f;
    const float damageCooldown = 1f;

    const float chargeSpeed = 15f;

    private Vector2 chargeToPosition;
    private Vector2 directionToPlayer;
    private float chargeStartTime;
    private float lastTimeDamageDealt;

    public bool DoCharge()
    {
        gameObject.transform.position = Vector2.MoveTowards(gameObject.transform.position, chargeToPosition, movementSpeed * chargeSpeed * Time.deltaTime);

        bool hasReachedDestination = (Vector2)gameObject.transform.position == chargeToPosition;
        bool chargeTimeLimitReached = Time.time > maxmimumChargeDuration + chargeStartTime;
        return hasReachedDestination || chargeTimeLimitReached;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && Time.time > lastTimeDamageDealt + damageCooldown)
        {
            DamageInfo damageInfo = new DamageInfo(attackDamage, false, directionToPlayer);
            collision.gameObject.SendMessage("ApplyDamage", damageInfo, SendMessageOptions.DontRequireReceiver);
            lastTimeDamageDealt = Time.time;
        }
    }

    public override IEnumerator StartAttack()
    {
        isAttacking = true;

        // Telegraph the attack
        spriteAnim.Play(telegraphAnimation);
        Vector2 directionToPlayer = (target.transform.position - transform.position);
        chargeToPosition = (Vector2)gameObject.transform.position + directionToPlayer + directionToPlayer.normalized * chargeDistanceScalar;
        yield return new WaitForSeconds(telegraphDuration);

        // Do the actual attack
        spriteAnim.Play(attackAnimation);
        chargeStartTime = Time.time;
        yield return new WaitUntil(DoCharge);

        AttackEnd();
    }

}
