using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthGlobe : MonoBehaviour
{
    public int healAmount;
    AudioSource audioSource;
    public AudioClip healSoundEffect;
    PlayerHealth playerHealth;
    public SpriteRenderer[] spriteRenderers;
    public CircleCollider2D circleCollider;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        playerHealth = PlayerManagement.player.GetComponent<PlayerHealth>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        string colliderTag = collision.gameObject.tag;

        if (colliderTag != "Player")
            return;

        // Don't heal player if they are at full health.
        if (playerHealth.IsAtMaxHealth())
            return;

        HealInfo healInfo = new HealInfo(healAmount);
        collision.GetComponent<PlayerHealth>().ApplyHeal(healInfo);

        SoundManager.PlayOneShot(audioSource, healSoundEffect);

        circleCollider.enabled = false;

        foreach(SpriteRenderer sr in spriteRenderers)
        {
            sr.enabled = false;
        }

        StartCoroutine("DestroyObject");
    }

    IEnumerator DestroyObject()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}
