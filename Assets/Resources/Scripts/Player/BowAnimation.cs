using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PowerTools;

public class BowAnimation : MonoBehaviour
{
    public AnimationClip idleAnimation;
    public AnimationClip drawAnimation;
    public AnimationClip fireAnimation;

    SpriteAnim spriteAnim;
    SpriteRenderer bowSpriteRenderer;
    void Awake()
    {
        PlayerShoot.onBowDraw += PlayDrawAnimation;
        PlayerShoot.onBowDraw += EnableBowSpriteRenderer;
        PlayerShoot.onShoot += DisableBowSpriteRenderer;
    }

    private void OnEnable()
    {
        bowSpriteRenderer = GetComponent<SpriteRenderer>();
        spriteAnim = GetComponent<SpriteAnim>();      
    }

    private void OnDisable()
    {
        bowSpriteRenderer.enabled = false;
    }

    private void Start()
    {
        PlayIdleAnimation();
    }

    public void PlayIdleAnimation()
    {
        spriteAnim.Play(idleAnimation);
    }

    public void PlayDrawAnimation(float time)
    {
        spriteAnim.Play(drawAnimation);
    }

    public void PlayFireAnimation(PlayerShoot playerShoot, Projectile projectileSpawned)
    {
        spriteAnim.Play(fireAnimation);
    }

    private void OnDestroy()
    {
        PlayerShoot.onBowDraw -= PlayDrawAnimation;
        PlayerShoot.onBowDraw -= EnableBowSpriteRenderer;
        PlayerShoot.onShoot -= DisableBowSpriteRenderer;
    }

    private void EnableBowSpriteRenderer(float time)
    {
        bowSpriteRenderer.enabled = true;
    }

    private void DisableBowSpriteRenderer(PlayerShoot playerShoot, Projectile projectileSpawned)
    {
        bowSpriteRenderer.enabled = false;
    }
}
