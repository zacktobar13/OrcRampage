using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class DroppedItem : MonoBehaviour
{
    public float spawnMovementSpeed;
    public AudioClip consumeSound;
    Vector2 randomDirection = Vector2.zero;
    protected Animator anim;
    protected ItemMagnetism magnetism;
    bool hasBeenConsumed;
    protected ObjectPool<GameObject> myPool;
    float tempMovementSpeed;

    protected virtual void OnEnable()
    {
        tempMovementSpeed = spawnMovementSpeed;
        anim = GetComponent<Animator>();
        randomDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        anim.speed = Random.Range(.9f, 1.1f);
        magnetism = GetComponent<ItemMagnetism>();
    }

    public void SetMyPool(ObjectPool<GameObject> pool)
    {
        myPool = pool;
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
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        OnTriggerEnter2D(collision);
    }

    protected virtual void Consume()
    {
        hasBeenConsumed = true;

        if (myPool != null)
            myPool.Release(gameObject);
        else
            Destroy(gameObject); // TODO This is for affix drops which currently aren't pooled.

        if (!consumeSound)
            return;

        SoundManager.PlayOneShot(transform.position, consumeSound, new SoundManagerArgs(false, consumeSound.name));
        return;
    }

    protected virtual void OnDisable()
    {
        spawnMovementSpeed = tempMovementSpeed;
        magnetism.enabled = false;
        hasBeenConsumed = false;
    }
}
