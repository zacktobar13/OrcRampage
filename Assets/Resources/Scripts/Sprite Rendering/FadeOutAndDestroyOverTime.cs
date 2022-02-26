using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOutAndDestroyOverTime : MonoBehaviour
{
    public float timeUntilFadeOut;
    public float fadeOutDuration;

    public SpriteRenderer shadowSprite;
    SpriteRenderer mainSprite;

    bool startFade = false;
    float spriteAlpha = 1f;
    float shadowAlpha = 1f;
    float shadowAlphaStart = 1f;
    float fadeTimer = 0;

    void OnEnable()
    {

        StartCoroutine(StartFade());
        if (shadowSprite)
        {
            shadowAlpha = shadowSprite.material.GetColor("_Color").a;
            shadowAlphaStart = shadowAlpha;
        }
    }

    void FixedUpdate()
    {
        if (startFade)
        {
           
            if (shadowSprite)
            {
                shadowSprite.material.SetColor("_Color", new Vector4(0f, 0f, 0f, shadowAlpha));
                shadowAlpha = Mathf.Lerp(shadowAlphaStart, 0, fadeTimer / fadeOutDuration);
            }

            if (mainSprite)
            {
                Color spriteColor = new Color(mainSprite.material.color.r, mainSprite.material.color.g, mainSprite.material.color.b, spriteAlpha);
                mainSprite.material.color = spriteColor;
                spriteAlpha = Mathf.Lerp(1, 0, fadeTimer / fadeOutDuration);
            }

            fadeTimer += Time.fixedDeltaTime;

            if (spriteAlpha <= .001f || shadowAlpha <= .001f)
            {
                if (transform.parent)
                {
                    Destroy(transform.parent.gameObject);
                }
                else
                {
                    Destroy(gameObject);
                }
            }
        } 
    }

    IEnumerator StartFade()
    {
        yield return new WaitForSeconds(timeUntilFadeOut);

        Transform sprite = transform.Find("Sprite");

        if (sprite)
        {
            mainSprite = sprite.GetComponent<SpriteRenderer>();
            spriteAlpha = mainSprite.material.color.a;
        }

        startFade = true;
        fadeTimer = 0;
    }
}
