using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : DroppedItem
{
    //PlayerCurrencyManager playerCurrency;
    public SpriteRenderer spriteRenderer;
    float spinAnimSpeed = 0f;
    public int value;

    public Sprite[] sprites;
    int anim_index;

    protected new void Start()
    {
        base.Start();
        InvokeRepeating("SpinAnimation", 0f, spinAnimSpeed);
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        //playerCurrency.AddCurrency(value);
    }

    void SpinAnimation()
    {
        spriteRenderer.sprite = sprites[anim_index % 4];
        anim_index += 1;
    }
}
