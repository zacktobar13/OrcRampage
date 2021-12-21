using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowDrawAnimation : MonoBehaviour
{
    Animator animator;
    SpriteRenderer spriteRenderer;

    void OnEnable()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        animator.StopPlayback();

        PlayerShoot.onBowDraw += PlayDrawAnimation;
        PlayerShoot.onShoot += StopDrawAnimation;
    }

    private void OnDisable()
    {
        animator.SetBool("drawArrow", false);
        spriteRenderer.enabled = false;
    }

    private void PlayDrawAnimation(float time)
    {
        spriteRenderer.enabled = true;
        animator.SetBool("drawArrow", true);
    }

    private void StopDrawAnimation(PlayerShoot playerShoot, Projectile projectileSpawned)
    {
        animator.SetBool("drawArrow", false);
        spriteRenderer.enabled = false;
    }

    private void OnDestroy()
    {
        PlayerShoot.onBowDraw -= PlayDrawAnimation;
        PlayerShoot.onShoot -= StopDrawAnimation;
    }
}
