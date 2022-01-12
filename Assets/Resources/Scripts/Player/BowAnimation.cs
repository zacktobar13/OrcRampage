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

    public void PlayFireAnimation(PlayerAttack playerShoot, Projectile projectileSpawned)
    {
        spriteAnim.Play(fireAnimation);
    }

    private void OnDestroy()
    {
    }

    private void EnableBowSpriteRenderer(float time)
    {
        bowSpriteRenderer.enabled = true;
    }

    private void DisableBowSpriteRenderer(PlayerAttack playerShoot, Projectile projectileSpawned)
    {
        bowSpriteRenderer.enabled = false;
    }
}
