using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BomberEnemyBehavior : MeleeEnemyBehavior
{
    public GameObject explosion;

    /*public override IEnumerator StartAttack()
    {
        isAttacking = true;
        ApplyDamage(new DamageInfo(maxHealth, false));
        return null;
    }*/

    public override void Death()
    {
        Instantiate(explosion, transform.position, transform.rotation);
    }
}
