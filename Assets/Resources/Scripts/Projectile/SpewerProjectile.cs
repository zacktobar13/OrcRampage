using UnityEngine;

public class SpewerProjectile : MonoBehaviour
{

    public int projectileDamage;
    public GameObject hitEffect;
    private Rigidbody2D rigidBody;
    public GameObject projectileGameObject;
    public GameObject projectileSpriteGameObject;
    public GameObject shadowGameObject;
    SpriteRenderer projectileSpriteRenderer;
    SpriteRenderer shadowSpriteRenderer;
    public float movementSpeed;
    public Vector2 movementDirection;
    Vector2 target;
    bool flipSprites;

    public Sprite[] restingSprites;
    public Sprite[] flyingSprites;
    AudioSource audioSource;
    public AudioClip hitGroundSFX;

    float gravity = 0f;
    float verticalProjectileSpeed = 0f;

    float hitEffectOffset = 0f;

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        projectileSpriteRenderer = projectileSpriteGameObject.GetComponent<SpriteRenderer>();
        shadowSpriteRenderer = shadowGameObject.GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();

        if (movementDirection.x > 0)
        {
            shadowSpriteRenderer.flipX = movementDirection.x < 0;
            projectileSpriteRenderer.flipX = movementDirection.x < 0;
            shadowSpriteRenderer.flipY = false;
            projectileSpriteRenderer.flipY = false;
        }
        else
        {
            shadowSpriteRenderer.flipX = movementDirection.x > 0;
            projectileSpriteRenderer.flipX = movementDirection.x > 0;
            shadowSpriteRenderer.flipY = true;
            projectileSpriteRenderer.flipY = true;
        }
    }

    void FixedUpdate()
    {
        if (projectileGameObject)
        {
            projectileGameObject.transform.Translate((Vector2.up * verticalProjectileSpeed));
            verticalProjectileSpeed -= gravity;
            rigidBody.MovePosition(rigidBody.position + (movementDirection * movementSpeed));

            if (projectileGameObject.transform.position.y <= shadowGameObject.transform.position.y)
            {
                Destroy(projectileGameObject);
                shadowSpriteRenderer.material = StaticResources.pixelSnap;
                shadowSpriteRenderer.sprite = restingSprites[(int)Random.Range(0f, restingSprites.Length)];

                shadowGameObject.transform.rotation = Quaternion.Euler(0f, 0f, 0f);

                if (hitGroundSFX)
                {
                    SoundManager.PlayOneShot(audioSource, hitGroundSFX);
                }

                return;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        string colliderTag = collision.gameObject.tag;
        if (colliderTag != "Enemy" && colliderTag != "GatherableResource" && colliderTag != "Projectile" && colliderTag != "DamageCollider" && colliderTag != "Projectile")
        {
            DamageInfo damageInfo = new DamageInfo(10, false, movementDirection);
            collision.gameObject.SendMessage("ApplyDamage", damageInfo, SendMessageOptions.DontRequireReceiver);
            Vector2 offset = (transform.position - collision.transform.position) * hitEffectOffset;
            Destroy(Instantiate(hitEffect, transform.position, transform.rotation), 2f);
            Destroy(gameObject);
        }
    }
}
