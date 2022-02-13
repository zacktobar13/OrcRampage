using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetRandomSprite : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Sprite[] sprites;

    void Start() {
        spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];
    }
}
