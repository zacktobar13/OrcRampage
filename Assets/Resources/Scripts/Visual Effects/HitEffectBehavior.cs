using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEffectBehavior : MonoBehaviour
{
    public AudioClip impactSound;
    public AudioSource audioSource;

    void Start()
    {
        transform.Rotate(0f, 0f, Random.Range(0f, 360f));
        audioSource.PlayOneShot(impactSound);
    }

    void DestroySelf()
    {
        Destroy(gameObject);
    }
}
