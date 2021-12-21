using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CastSpriteShadow : MonoBehaviour
{
    private Sprite originalSprite;
    private Sprite shadowSprite;
    private SpriteRenderer shadowSpriteRenderer;
    private SpriteRenderer originalSpriteRenderer;
    public bool isDecoration;


    void Start()
    {
        originalSpriteRenderer = transform.Find("Sprite").GetComponent<SpriteRenderer>();
        originalSprite = originalSpriteRenderer.sprite;
        shadowSpriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        shadowSpriteRenderer.sprite = originalSprite;
        shadowSpriteRenderer.flipY = true;

        if (isDecoration)
        {
            SortingGroup sortingGroup = GetComponent<SortingGroup>();
            if (sortingGroup)
                sortingGroup.sortingOrder = 1;
            Collider2D[] colliders = GetComponents<Collider2D>();
            foreach (Collider2D collider in colliders)
            {
                collider.enabled = false;
            }
            DestructibleObject destructableObject = GetComponent<DestructibleObject>();
            if (destructableObject)
                destructableObject.enabled = false;

            shadowSpriteRenderer.material = StaticResources.decorationShadowMaterial;
            Color decorationColor = shadowSpriteRenderer.color;
            decorationColor.a = .8f;
            shadowSpriteRenderer.color = decorationColor;
        }
        else
        {
            originalSpriteRenderer.sortingLayerName = "Objects";
            shadowSpriteRenderer.sortingLayerName = "Shadows";
            shadowSpriteRenderer.color = Color.black;
            shadowSpriteRenderer.material = StaticResources.shadowMaterial;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        shadowSpriteRenderer.sprite = originalSpriteRenderer.sprite;
        shadowSpriteRenderer.flipX = originalSpriteRenderer.flipX;
    }
}
