using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEffectBehavior : MonoBehaviour
{
    public AudioClip impactSound;

    void Start()
    {
        transform.Rotate(0f, 0f, Random.Range(0f, 360f));
        SoundManager.PlayOneShot(transform.position, impactSound, new SoundManagerArgs(false, "ProjectileImpact"));
    }

    void DestroySelf()
    {
        Destroy(gameObject);
    }
}
