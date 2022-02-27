using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedItem : MonoBehaviour
{
    public float spawnMovementSpeed;
    Vector2 randomDirection = Vector2.zero;
    protected Animator anim;
    protected ItemMagnetism magnetism;

    float collectionTimer = .5f;
    float timeSpawned;
    bool hasBeenConsumed;

    protected virtual void Start()
    {
        anim = GetComponent<Animator>();
        randomDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        timeSpawned = Time.time;
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
        if (hasBeenConsumed || Time.time < timeSpawned + collectionTimer)
            return;

        if (collision.gameObject.tag.Equals("Player"))
        {
            hasBeenConsumed = true;
            Destroy(gameObject);
        }
    }
}
