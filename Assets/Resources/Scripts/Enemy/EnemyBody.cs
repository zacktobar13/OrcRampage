using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBody : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Sprite airSprite, groundSprite;
	private Vector2 randomDropDir;
    public float movementSpeed;
    Rigidbody2D rigidBody;
    private Coroutine moveCoroutine;

    public void Anim_SwitchToGroundSprite()
    {
        spriteRenderer.sprite = groundSprite;
        rigidBody = GetComponent<Rigidbody2D>();
    }
    public void Anim_SwitchToAirSprite()
    {
        spriteRenderer.sprite = airSprite;
    }

	private void Start()
	{
        moveCoroutine = StartCoroutine(MoveOnDrop());
    }

	IEnumerator MoveOnDrop()
    {
        randomDropDir = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        float targetTime = Time.time + 1f;
        while (Time.time <= targetTime)
        {
            yield return new WaitForFixedUpdate();
            rigidBody.MovePosition(rigidBody.position + randomDropDir * movementSpeed);
        }
    }

}
