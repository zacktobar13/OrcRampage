using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthGlobe : MonoBehaviour
{
    public float healPercentage;
    public float magnestismDistance;
    public float magnetismSpeed;
    AudioSource audioSource;
    public AudioClip healSoundEffect;
    GameObject nearestPlayer = null;
    PlayerHealth playerHealth;
    float distanceToPlayer;
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
        if (playerHealth.health >= playerHealth.maxHealth)
            return;

        HealInfo healInfo = new HealInfo((int)(playerHealth.maxHealth * healPercentage));
        collision.GetComponent<PlayerHealth>().ApplyHeal(healInfo);

        audioSource.PlayOneShot(healSoundEffect);
        circleCollider.enabled = false;

        foreach(SpriteRenderer sr in spriteRenderers)
        {
            sr.enabled = false;
        }

        StartCoroutine("DestroyObject");
    }

    public void FixedUpdate()
    {
        // Return out of movement code if player is at full health.
        if (playerHealth.health == playerHealth.maxHealth)
            return;

        if (nearestPlayer == null)
        {
            nearestPlayer = PlayerManagement.player;

            if (nearestPlayer == null)
            {
                Debug.LogWarning(gameObject.name + " could not find a player!");
                return;
            }
        }
        else
        {
            distanceToPlayer = Vector2.Distance(nearestPlayer.transform.position, transform.position);
        }

        // Move towards player if they are in magnetism range.
        if (distanceToPlayer < magnestismDistance )
        {
            transform.position = Vector2.MoveTowards(transform.position, nearestPlayer.transform.position, magnetismSpeed);
        }
    }

    IEnumerator DestroyObject()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}
