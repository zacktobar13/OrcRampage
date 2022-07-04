using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class FadeOutAndDestroyOverTime : MonoBehaviour
{
    const float SHADOW_ALPHA_START = 89f/255f;
    public float timeUntilFadeOut;
    public float fadeOutDuration;

    public SpriteRenderer shadowSprite;
    SpriteRenderer mainSprite;

    bool startFade = false;
    float spriteAlpha = 1f;
    float shadowAlpha = 1f;
    float shadowAlphaStart = 1f;
    float fadeTimer = 0;
    ObjectPool<GameObject> myPool;

    void OnEnable()
    {
        //string poolName = gameObject.name.Replace("(Clone)", "");
        myPool = GameObject.Find("Game Management").GetComponent<PoolManager>().GetObjectPool(gameObject);
        StartCoroutine(StartFade());

        spriteAlpha = 1f;
        shadowAlpha = 1f;
        shadowAlphaStart = 1f;
        fadeTimer = 0;
        startFade = false;

        if (shadowSprite)
        {
            shadowAlpha = shadowSprite.material.GetColor("_Color").a;
            shadowAlphaStart = SHADOW_ALPHA_START;
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
                myPool.Release(gameObject);
            }
        } 
    }

    void OnDisable()
    {
        Color spriteColor = new Color(mainSprite.material.color.r, mainSprite.material.color.g, mainSprite.material.color.b, 1f);
        mainSprite.material.color = spriteColor;
        shadowSprite.material.SetColor("_Color", new Vector4(0f, 0f, 0f, SHADOW_ALPHA_START));
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
