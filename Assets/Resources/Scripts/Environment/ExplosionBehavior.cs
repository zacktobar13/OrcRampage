using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PowerTools;
using EZCameraShake;
public class ExplosionBehavior : MonoBehaviour
{
    public int damageAmount;
    public bool canHurtPlayer;
    public float explosionRadius = 8f;
    
    AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.pitch = Random.Range(.5f, 2f);
        SpriteAnim spriteAnim = GetComponent<SpriteAnim>();
        float animationLength = spriteAnim.GetCurrentAnimation().length;
        float animationSpeed = spriteAnim.GetSpeed();
        Destroy(gameObject, animationLength / animationSpeed);

        Explode();
    }

    public void Explode()
    {
        Collider2D[] collisions = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        foreach (Collider2D collision in collisions)
        {
            DealDamage(collision);
        }
    }

    void DealDamage(Collider2D collision)
    {
        if (!canHurtPlayer && collision.tag == "Player")
            return;

        if (collision.isTrigger)
        {
            Vector2 damageDirection = collision.transform.position - transform.position;

            DamageInfo damageInfo = new DamageInfo(damageAmount, false, damageDirection);
            collision.gameObject.SendMessage("ApplyDamage", damageInfo, SendMessageOptions.DontRequireReceiver);
            damageDirection = Vector2.zero;
        }
    }
}
