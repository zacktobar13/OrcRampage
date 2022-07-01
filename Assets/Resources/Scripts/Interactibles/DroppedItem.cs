using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedItem : MonoBehaviour
{
    public float spawnMovementSpeed;
    public AudioClip consumeSound;
    Vector2 randomDirection = Vector2.zero;
    protected Animator anim;
    protected ItemMagnetism magnetism;
    AudioSource audioSource;
    bool hasBeenConsumed;

    protected virtual void Start()
    {
        audioSource = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        randomDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        anim.speed = Random.Range(.9f, 1.1f);
        magnetism = GetComponent<ItemMagnetism>();
    }

    protected virtual void Update()
    {
        if (spawnMovementSpeed == 0)
            return;

        transform.Translate(randomDirection * spawnMovementSpeed * Time.deltaTime);
    }

    public virtual void Anim_StopSpawnMovement()
    {
        spawnMovementSpeed = 0f;
    }

    public virtual void Anim_EnableMagnetism()
    {
        magnetism.enabled = true;
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasBeenConsumed || !magnetism.enabled)
            return;

        if (collision.gameObject.tag.Equals("Player"))
        {
            Consume();
            Destroy(gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        OnTriggerEnter2D(collision);
    }

    protected virtual void Consume()
    {
        hasBeenConsumed = true;

        if (!audioSource || !audioSource.isActiveAndEnabled || !consumeSound)
            return;
        Debug.Log("About to play sound " + consumeSound.name);
        SoundManager.PlayOneShot(audioSource, consumeSound, new SoundManagerArgs(true, consumeSound.name));
        return;
    }
}
