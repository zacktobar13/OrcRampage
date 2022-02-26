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
    float fadeTimer = 0;

    void Start()
    {
        StartCoroutine(StartFade());
    }

    void FixedUpdate()
    {
        if (startFade)
        {

            if (shadowSprite)
            {
                Color shadowColor = new Color(shadowSprite.material.color.r, shadowSprite.material.color.g, shadowSprite.material.color.b, shadowAlpha);
                shadowSprite.material.color = shadowColor;
                shadowAlpha = Mathf.Lerp(1, 0, fadeTimer / fadeOutDuration);
                fadeTimer += Time.fixedDeltaTime;
            }

            if (mainSprite)
            {
                Color spriteColor = new Color(mainSprite.material.color.r, mainSprite.material.color.g, mainSprite.material.color.b, spriteAlpha);
                mainSprite.material.color = spriteColor;
                spriteAlpha = Mathf.Lerp(1, 0, fadeTimer / fadeOutDuration);
                fadeTimer += Time.fixedDeltaTime;
            }

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

        shadowSprite = GetComponent<SpriteRenderer>();

        if (shadowSprite)
            shadowAlpha = shadowSprite.material.color.a;

        startFade = true;
        fadeTimer = 0;
    }
}
