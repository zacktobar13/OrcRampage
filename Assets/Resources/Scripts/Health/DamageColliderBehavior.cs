using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageColliderBehavior : MonoBehaviour
{
    public GameObject creator;
    public Vector2 damageDirection = Vector2.zero;
    public int damageAmount;

    void Awake()
    {
        StartCoroutine("DestroyAfterTime", .3f);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject != creator && collision.isTrigger)
        {
            damageDirection = collision.transform.position - transform.position;
            DamageInfo damageInfo = new DamageInfo(damageAmount, false, damageDirection.normalized);
            collision.gameObject.SendMessage("ApplyDamage", damageInfo, SendMessageOptions.DontRequireReceiver);
        }
    }

    IEnumerator DestroyAfterTime(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Destroy(gameObject);
    }
}
