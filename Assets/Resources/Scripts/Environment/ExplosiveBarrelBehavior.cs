using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBarrelBehavior : DestructibleObject
{

    public GameObject fire;
    public GameObject explosion;
    public GameObject debris;

    int onFireDamage;

    enum States
    {
        idle,
        onFire,
        explode,
        dead
    }

    States state;
    States previousState;
    bool stateNew = false;

    private new void Start()
    {
        //base.Start();
        onFireDamage = (int)(maxHealth * .1f);
    }

    void Update()
    {
        switch(state)
        {
            case States.idle:
            {
                if (health < maxHealth)
                {
                    ChangeState(States.onFire);
                }
                break;
            }
            case States.onFire:
            {
                if ( stateNew )
                {
                    // TODO Start fire sound effect.
                    stateNew = false;
                    fire.SetActive(true);
                    InvokeRepeating("FireDamage", .75f, .75f);
                }

                break;
            }
        }
    }

    void ChangeState(States targetState)
    {
        previousState = state;
        stateNew = true;
        state = targetState;
    }

    void FireDamage()
    {
        // TODO Hit sound effect.
        DamageInfo damageInfo = new DamageInfo(onFireDamage, false);
        ApplyDamage(damageInfo);
    }

    public override void Death()
    {
        Instantiate(debris, transform.position, transform.rotation);
        GameObject damageCircle = Instantiate(explosion, new Vector2(transform.position.x, transform.position.y + 3f), transform.rotation);
        ExplosionBehavior circle = damageCircle.GetComponent<ExplosionBehavior>();
        circle.creator = gameObject;
        circle.damageAmount = 100;
        base.Death();
    }
}
