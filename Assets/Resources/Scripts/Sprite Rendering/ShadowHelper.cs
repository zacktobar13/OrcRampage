using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowHelper : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sortingLayerName = "Shadows";
        spriteRenderer.color = Color.black;
    }
}
