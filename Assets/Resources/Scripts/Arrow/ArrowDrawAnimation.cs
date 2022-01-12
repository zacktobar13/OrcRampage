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

        //PlayerAttack.onBowDraw += PlayDrawAnimation;
        //PlayerAttack.onAttack += StopDrawAnimation;
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

    private void StopDrawAnimation(PlayerAttack playerShoot, Projectile projectileSpawned)
    {
        animator.SetBool("drawArrow", false);
        spriteRenderer.enabled = false;
    }

    private void OnDestroy()
    {
        //PlayerAttack.onBowDraw -= PlayDrawAnimation;
        //PlayerAttack.onAttack -= StopDrawAnimation;
    }
}
