using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public float spawnMovementSpeed;
    public SpriteRenderer spriteRenderer;
	Vector2 randomDirection = Vector2.zero;
    public Animator anim;
    float spinAnimSpeed = 0f;
    public int value;

    public Sprite[] sprites;
    int anim_index;
    float collectionTimer = .5f;
    float timeSpawned;

    void Start()
    {
        randomDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        timeSpawned = Time.time;
        anim.speed = Random.Range(.9f, 1.1f);
        spinAnimSpeed = Random.Range(.1f, .15f);
        InvokeRepeating("SpinAnimation", 0f, spinAnimSpeed);
    }

    void Update()
    {
        if (spawnMovementSpeed == 0)
            return;

        transform.Translate(randomDirection * spawnMovementSpeed * Time.deltaTime);
    }

	public void Anim_StopSpawnMovement()
    {
        spawnMovementSpeed = 0f;
    }

    bool hasBeenConsumed = false;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (hasBeenConsumed)
            return;

        if (collision.gameObject.tag.Equals("Player") && Time.time > timeSpawned + collectionTimer)
        {
            PlayerCurrencyManager playerCurrency = collision.gameObject.GetComponent<PlayerCurrencyManager>();
            playerCurrency.AddCurrency(value);
            hasBeenConsumed = true;
            Destroy(gameObject);
        }
    }

    void SpinAnimation()
    {
        spriteRenderer.sprite = sprites[anim_index % 4];
        anim_index += 1;
    }
}
