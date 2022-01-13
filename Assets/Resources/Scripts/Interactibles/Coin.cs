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

    public Sprite[] sprites;
    int anim_index;

    void Start()
    {
        randomDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        anim.speed = Random.Range(.9f, 1.1f);
        spinAnimSpeed = Random.Range(.1f, .15f);
        InvokeRepeating("SpinAnimation", 0f, spinAnimSpeed);
    }

    void Update()
    {
        transform.Translate(randomDirection * spawnMovementSpeed * Time.deltaTime);
    }

	public void Anim_StopSpawnMovement()
    {
        spawnMovementSpeed = 0f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            PlayerCurrencyManager.localCurrency += 1;
            Debug.Log("New Local Currency Amount: " + PlayerCurrencyManager.localCurrency);
            Destroy(gameObject);
        }
    }

    void SpinAnimation()
    {
        spriteRenderer.sprite = sprites[anim_index % 4];
        anim_index += 1;
    }
}
