using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEffectBehavior : MonoBehaviour
{
    void Start()
    {
        transform.Rotate(0f, 0f, Random.Range(0f, 360f));
    }

    void DestroySelf()
    {
        Destroy(gameObject);
    }
}
