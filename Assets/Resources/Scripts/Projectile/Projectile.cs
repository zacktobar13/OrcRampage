using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour
{
    Animator anim;
    public int projectileDamage;
    public bool shotByPlayer;
    public float rotationOffset;
    [HideInInspector] public bool isCriticalHit;
    [HideInInspector] public float knockbackAmount;
    public GameObject hitEffect;
    public Transform frontOfProjectile;
    public Rigidbody2D rigidBody;
    private GameObject spriteGameObject;
    public GameObject shadow;

    public float movementSpeed;
    public int numberOfPierces;
    Vector2 movementDirection;
    SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteGameObject = transform.Find("Sprite").gameObject;
        spriteRenderer = spriteGameObject.GetComponent<SpriteRenderer>();

        if (anim)
        {
            anim = GetComponent<Animator>();
            anim.speed = Random.Range(.8f, 1.2f);
        }

        transform.right = Utility.Rotate((Vector2)transform.right, rotationOffset);
        movementDirection = transform.right;
        movementSpeed *= Random.Range(.98f, 1.01f);
        Destroy(gameObject, 5);
    }

    public void SetProjectileRotation(float rotation)
    {
        GameObject newShadow = Instantiate(shadow, new Vector3(transform.position.x, transform.position.y - 2.5f, 0f), Quaternion.identity);
        transform.rotation = Quaternion.Euler(0, 0, rotation);
        newShadow.transform.parent = gameObject.transform;
        newShadow.transform.rotation = transform.rotation;
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
        if (spriteRenderer.isVisible)
        {
            GameObject spawnedHitEffect = Instantiate(hitEffect, collision.bounds.ClosestPoint(frontOfProjectile.position), collision.transform.rotation);
            spawnedHitEffect.transform.localScale = transform.localScale;
        }

        DamageInfo damageInfo = new DamageInfo(projectileDamage, movementDirection.normalized, 1f, 1f, isCriticalHit, false);
        collision.gameObject.SendMessage("ApplyDamage", damageInfo, SendMessageOptions.DontRequireReceiver);
        
        bool hitMapClutter = collision.gameObject.layer == LayerMask.NameToLayer("Map Clutter");
        if (numberOfPierces <= 0 && !hitMapClutter) 
            Destroy(gameObject);

        numberOfPierces--;
    }
}
