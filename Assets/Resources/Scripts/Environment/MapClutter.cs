using UnityEngine;
using System.Collections;

public class MapClutter : MonoBehaviour
{
    public float health;
    public int maxHealth;
    public GameObject[] remains;
    AudioSource audioSource;
    SpriteRenderer spriteRenderer;
    BoxCollider2D boxCollider2D;
    public AudioClip destroySound;
    public float volumeScalar;
    GameObject hitEffect;

    bool alreadyDroppedRemains = false;

    void Awake()
    {
        health = maxHealth;
        audioSource = GetComponent<AudioSource>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        hitEffect = StaticResources.hitEffect1;
    }

    public void ApplyDamage(DamageInfo damageInfo)
    {
        if (!enabled)
            return;

        if (damageInfo.damageAmount == 0)
            return;

        StartCoroutine(Death());
    }

    public void DropRemains()
    {
        if (alreadyDroppedRemains)
            return;

        alreadyDroppedRemains = true;
        foreach (GameObject remain in remains)
        {
            Instantiate(remain, transform.position, transform.rotation);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        StartCoroutine(Death());
    }

    IEnumerator Death()
    {
        if ( audioSource != null )
        {
            SoundManager.PlayOneShot(audioSource, destroySound, new SoundManagerArgs(destroySound.name));
        }

        Instantiate(hitEffect, transform.position, transform.rotation);

        DropRemains();

        if ( spriteRenderer != null )
            spriteRenderer.enabled = false;

        if ( boxCollider2D != null )
            boxCollider2D.enabled = false;

        yield return new WaitForSeconds(4f);
        Destroy(gameObject);
    }
}
