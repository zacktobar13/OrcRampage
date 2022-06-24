using UnityEngine;

public class Coin : DroppedItem
{
    PlayerCurrencyManager playerCurrency;
    public SpriteRenderer spriteRenderer;
    public float spinAnimSpeed = 0f;
    public int value;

    public Sprite[] sprites;
    int anim_index;

    protected new void Start()
    {
        base.Start();
        anim_index = Random.Range(0, sprites.Length);
        spinAnimSpeed *= Random.Range(.8f, 1.2f);
        playerCurrency = PlayerManagement.player.GetComponent<PlayerCurrencyManager>();
        InvokeRepeating("SpinAnimation", 0f, spinAnimSpeed);
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


}
