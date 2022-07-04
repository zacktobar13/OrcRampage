using UnityEngine;
using UnityEngine.Pool;


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
    public Vector2 movementDirection;
    TrailRenderer trail;
    SpriteRenderer spriteRenderer;
    ObjectPool<GameObject> myPool;
    GameObject newShadow;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }


    void Start()
    {
        spriteGameObject = transform.Find("Sprite").gameObject;
        spriteRenderer = spriteGameObject.GetComponent<SpriteRenderer>();
        trail = transform.Find("Trail").GetComponent<TrailRenderer>();
        anim.speed = Random.Range(.8f, 1.2f);
        movementSpeed *= Random.Range(.98f, 1.01f);
    }

    public void SetMyPool(ObjectPool<GameObject> pool)
    {
        myPool = pool;
    }

    public void SetProjectileRotation(float rotation) // TODO Need to stop instantiating shadows
    {
        newShadow = Instantiate(shadow, new Vector3(transform.position.x, transform.position.y - 2.5f, 0f), Quaternion.identity);
        
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
        if (!isActiveAndEnabled)
            return;

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

        numberOfPierces--;
        
        if (numberOfPierces <= 0 && !hitMapClutter)
        {
            myPool.Release(gameObject);
        }
    }

    private void OnDisable()
    {
        Reset();
    }

    public void Reset()
    {
        trail.Clear();
        projectileDamage = 0;
        isCriticalHit = false;
        shotByPlayer = false;
        Destroy(newShadow);
    }

    private void OnBecameInvisible()
    {
        myPool.Release(gameObject);
    }
}
