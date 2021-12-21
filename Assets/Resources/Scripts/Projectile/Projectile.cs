using UnityEngine;
using System.Collections;
using EZCameraShake;
public class Projectile : MonoBehaviour
{

    public int projectileDamage;
    [HideInInspector] public bool isCriticalHit;
    [HideInInspector] public bool isHeadshot;
    [HideInInspector] public float spread;
    [HideInInspector] public float knockbackAmount;
    public GameObject hitEffect;
    public Transform frontOfProjectile;
    public Rigidbody2D rigidBody;
    public GameObject shadow;
    public TrailRenderer trailRenderer;
    public bool isPiercing;
    public bool isRicochet;

    public AudioClip flySound;
    public AudioClip impactSound;
    public AudioClip criticalHitSound;

    SpriteRenderer spriteRenderer;
    BoxCollider2D boxCollider;
    AudioSource audioSource;
    GameObject myShadow;

    [HideInInspector] public int ricochetCount = 0;

    public float movementSpeed;
    Vector2 movementDirection;

   // private bool hasDealtDamage = false;

    public void SetProjectileRotation(float rotation)
    {
        // Rotate sprite
        transform.Rotate( 0, 0, rotation );
    }

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();

        // Random spread
        transform.Rotate( 0, 0, Random.Range(-spread, spread) );

        audioSource = GetComponent<AudioSource>();

        myShadow = Instantiate(shadow, new Vector2(transform.position.x, transform.position.y - 7), transform.rotation, transform);
        myShadow.transform.parent = transform;

        // Move "Forward"
        ChangeMovementDirection(transform.right);
    }

    void FixedUpdate ()
    {
        rigidBody.MovePosition( rigidBody.position + movementDirection * movementSpeed );
    }

    // Whenever we change directions, move our shadow and play a new sound
    void ChangeMovementDirection(Vector2 newDirection)
    {
        audioSource.pitch = Random.Range(1f, 1.2f);
        audioSource.PlayOneShot(flySound, .2f);
        myShadow.transform.position = new Vector2(transform.position.x, transform.position.y - 7f);
        movementDirection = newDirection;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        string colliderTag = collision.gameObject.tag;

        if (colliderTag != "Player" && colliderTag != "GatherableResource" && colliderTag != "DamageCollider" && colliderTag != "Projectile") // if this list keeps growing make a hashtable and .contains(colliderTag)
        {
            // Spawn hit effect
            Instantiate(hitEffect, collision.bounds.ClosestPoint(frontOfProjectile.position), collision.transform.rotation);
            audioSource.pitch = Random.Range(1f, 1.2f);
            audioSource.PlayOneShot(impactSound);

            // 100x more damage if shot is a head shot
            int headShotScalar = isHeadshot ? 100 : 1;

            DamageInfo damageInfo = new DamageInfo(projectileDamage * headShotScalar, movementDirection.normalized, 1f, 1f, isCriticalHit);

            if (collision.gameObject.layer != LayerMask.NameToLayer("World Colliders"))
            {
                collision.gameObject.SendMessage("ApplyDamage", damageInfo, SendMessageOptions.DontRequireReceiver);
            }

            if (colliderTag == "Enemy")
            {
                if (isPiercing)
                {
                    return;
                }
                else
                {
                    // Only do camera shake for headshots when the arroy is going to be destroyed
                    if (isHeadshot)
                        CameraShaker.Instance.ShakeOnce(2f, 2f, .1f, 1f);

                    StartCoroutine(DestroyArrow());
                }
            }
            else // Hitting wall
            {
                if (isRicochet && ricochetCount > 1)
                {
                    ricochetCount--;
                    RaycastHit2D[] raycastResults = new RaycastHit2D[1];
                    ContactFilter2D contactFilter = new ContactFilter2D();
                    contactFilter.layerMask = gameObject.layer;
                    contactFilter.useLayerMask = true;
                    Vector2 reflectionDirection;

                    if (Physics2D.Raycast(gameObject.transform.position, movementDirection, contactFilter, raycastResults, 4) > 0)
                    {
                        reflectionDirection = Vector2.Reflect(movementDirection, raycastResults[0].normal);

                        // Rotate arrow to correct direction
                        transform.right = reflectionDirection;

                        // Make it move in the correct direction
                        ChangeMovementDirection(reflectionDirection);
                    }
                }
                else
                {
                    StartCoroutine(DestroyArrow());
                }
            }

            if (isCriticalHit && colliderTag == "Enemy")
            {
                audioSource.pitch = Random.Range(.95f, 1.05f);
                audioSource.PlayOneShot(criticalHitSound, 1f);
            }
        }
    }

    IEnumerator DestroyArrow()
    {
        movementSpeed = 0f;
        boxCollider.enabled = false;
        spriteRenderer.enabled = false;
        audioSource.Stop();
        audioSource.PlayOneShot(impactSound);

        Destroy(myShadow);
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }


}
