using UnityEngine;
using System.Collections;

public class Coin : DroppedItem
{
    PlayerCurrencyManager playerCurrency;
    public SpriteRenderer spriteRenderer;
    public float spinAnimSpeed = 0f;
    public int value;
    public Sprite[] sprites;
    int anim_index;

    private void Start()
    {
        spinAnimSpeed *= Random.Range(.8f, 1.2f);
        InvokeRepeating("SpinAnimation", 0f, spinAnimSpeed);
    }

    protected new void OnEnable()
    {
        base.OnEnable();
        anim_index = Random.Range(0, sprites.Length);
        playerCurrency = PlayerManagement.player.GetComponent<PlayerCurrencyManager>();
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

    protected override void OnDisable()
    {
        base.OnDisable();
    }
}
