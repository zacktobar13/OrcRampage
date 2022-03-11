using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthGlobe : DroppedItem
{
    public int healAmount;
    public AudioClip healSoundEffect;
    PlayerHealth playerHealth;
    public SpriteRenderer[] spriteRenderers;
    public CircleCollider2D circleCollider;

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        HealInfo healInfo = new HealInfo(healAmount, healSoundEffect);
        collision.GetComponent<PlayerHealth>().ApplyHeal(healInfo);
        base.OnTriggerEnter2D(collision);
    }
}
