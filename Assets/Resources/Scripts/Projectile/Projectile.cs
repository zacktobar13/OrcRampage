using UnityEngine;
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
    Vector2 movementDirection;

    public void SetProjectileRotation(float rotation)
    {
        transform.rotation = Quaternion.Euler(0, 0, rotation);
        transform.Rotate(0, 0, Random.Range(-spread, spread));
    }

    void Start()
    {
        movementDirection = transform.right;
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

        // Spawn hit effect
        Instantiate(hitEffect, collision.bounds.ClosestPoint(frontOfProjectile.position), collision.transform.rotation);

        DamageInfo damageInfo = new DamageInfo(projectileDamage, movementDirection.normalized, 1f, 1f, isCriticalHit);
        collision.gameObject.SendMessage("ApplyDamage", damageInfo, SendMessageOptions.DontRequireReceiver);

        if (collision.gameObject.layer != LayerMask.NameToLayer("Map Clutter")) 
            Destroy(gameObject);
    }
}
