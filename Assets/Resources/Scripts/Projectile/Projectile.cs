﻿using UnityEngine;
using System.Collections;
using EZCameraShake;
public class Projectile : MonoBehaviour
{
    public int projectileDamage;
    public bool shotByPlayer;
    public float spread;
    [HideInInspector] public bool isCriticalHit;
    [HideInInspector] public float knockbackAmount;
    public GameObject hitEffect;
    public Transform frontOfProjectile;
    public Rigidbody2D rigidBody;

    public float movementSpeed;
    public bool isPiercing;
    Vector2 movementDirection;

    public void SetProjectileRotation(float rotation)
    {
        transform.rotation = Quaternion.Euler(0, 0, rotation);

        //transform.Rotate(0, 0, Random.Range(-spread, spread));
    }

    void Start()
    {
        movementDirection = transform.right;
        Destroy(gameObject, 5);
    }

    void FixedUpdate ()
    {
        rigidBody.MovePosition(rigidBody.position + movementDirection * movementSpeed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // No friendly fire
        if (shotByPlayer && collision.CompareTag("Player"))
        {
            return;
        }
        if (!shotByPlayer && collision.CompareTag("Enemy"))
        {
            return;
        }

        // Don't hit an objects world colliders (this gameobject doesnt have ApplyDamage)
        if (collision.gameObject.layer == LayerMask.NameToLayer("World Colliders"))
        {
            return;
        }

        // Spawn hit effect
        Instantiate(hitEffect, collision.bounds.ClosestPoint(frontOfProjectile.position), collision.transform.rotation);

        DamageInfo damageInfo = new DamageInfo(projectileDamage, movementDirection.normalized, 1f, 1f, isCriticalHit);
        collision.gameObject.SendMessage("ApplyDamage", damageInfo, SendMessageOptions.DontRequireReceiver);
        
        bool hitMapClutter = collision.gameObject.layer == LayerMask.NameToLayer("Map Clutter");
        if (!isPiercing && !hitMapClutter) 
            Destroy(gameObject);
    }
}
