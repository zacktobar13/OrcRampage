using UnityEngine;
using System.Collections;

public class Coin : DroppedItem
{
    PlayerCurrencyManager playerCurrency;
    public SpriteRenderer spriteRenderer;
    public float spinAnimSpeed = 0f;
    public int value;
    float secondsUntilDestroy = 15f;
    public Sprite[] sprites;
    int anim_index;
    Coroutine timer;

    private void Start()
    {
        spinAnimSpeed *= Random.Range(.8f, 1.2f);
    }

    protected new void OnEnable()
    {
        base.OnEnable();
        anim_index = Random.Range(0, sprites.Length);
        playerCurrency = PlayerManagement.player.GetComponent<PlayerCurrencyManager>();
        InvokeRepeating("SpinAnimation", 0f, spinAnimSpeed);
        timer = StartCoroutine(DestroyAfterTime());
    }

	protected override void Consume()
	{
        base.Consume();
        playerCurrency.AddLocalCurrency(value);
	}
    void SpinAnimation()
    {
        spriteRenderer.sprite = sprites[anim_index % 4];
        anim_index += 1;
    }

    IEnumerator DestroyAfterTime()
    {
        yield return new WaitForSeconds(secondsUntilDestroy);
        myPool.Release(gameObject);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        StopCoroutine(timer);
        secondsUntilDestroy = 15f;
    }
}
