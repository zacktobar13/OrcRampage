using UnityEngine;

public class EditorIcon : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    void OnEnable()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = false;
    }
}
