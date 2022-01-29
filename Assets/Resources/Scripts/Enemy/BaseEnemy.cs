using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PowerTools;

public class BaseEnemy : MonoBehaviour {

    // Assigned in inspector
    [Header("Basic Attributes")]
    public float attackRange;
    public float movementSpeed;
    public int attackDamage;
    public int maxHealth;
    public GameObject[] droppables;
    public GameObject deadBody;
    public float healthGlobeDropChance;
    public float enemyDetectionDistanceCurrent;

    [Header("Sound Effects")]
    public AudioClip hitSound;
    public AudioClip hurtSound;
    public AudioClip deadSound;
    public AudioClip idleSound;

    [Header("Animations")]
    public AnimationClip idleAnimation;
    public AnimationClip wanderAnimation;
    public AnimationClip chaseAnimation;
    public AnimationClip movingAnimation;
    public AnimationClip hurtAnimation;
    public AnimationClip telegraphAnimation;
    public AnimationClip attackAnimation;

    private GameObject copperCoin;
    private GameObject floatingDamageNumber;
    private GameObject critDamageNumber;
    protected GameObject damageCollider;
    protected Transform damageSpawnPoint;

    // Referenced in Start()
    protected AudioSource audioSource;
    protected SpriteAnim spriteAnim;
    protected SpriteRenderer spriteRenderer;
    GameObject healthUI;
    Image healthbar;
    protected float health;
    protected Weapon currentWeapon;

    protected GameObject target = null;
    protected float distanceToTarget;
    protected bool canMove = true;
    protected bool canBeKnockedBack = true;
    protected bool inRangeToAttack = false;
    protected bool isTargetDetected = false;
    protected bool isStunned = false;
    private IEnumerator disableMovement;
    private IEnumerator knockBack;
    private IEnumerator flashWhiteForSeconds;

    protected IEnumerator telegraphAttack;
    protected IEnumerator hurt;

    protected void Start ()
    {
        // Give enemies attack range and movement speed some randomness
        attackRange = Random.Range ( attackRange * 0.95f, attackRange * 1.05f );
        movementSpeed = Random.Range(movementSpeed * 0.9f, movementSpeed * 1.1f);

        health = maxHealth;
        //Debug.Assert(attackRange <= enemyDetectionDistanceCurrent, "Attack range must be less than or equal to enemyDetectionDistance!");
        audioSource = gameObject.GetComponent<AudioSource>();
        spriteRenderer = transform.Find("Sprite").GetComponent<SpriteRenderer>();
        healthUI = transform.Find("Enemy Health Bar").gameObject;
        healthbar = transform.Find("Enemy Health Bar/Healthbar").GetComponent<Image>();
        spriteAnim = transform.Find("Sprite").GetComponent<SpriteAnim>();
        copperCoin = StaticResources.copperCoin;
        floatingDamageNumber = StaticResources.floatingDamageNumber;
        critDamageNumber = StaticResources.critDamageNumber;
        damageCollider = StaticResources.damageCollider;
        damageSpawnPoint = transform.Find("Damage Spawn Point");
        currentWeapon = GetComponentInChildren<Weapon>();
        if (currentWeapon)
            currentWeapon.PickupWeapon(gameObject);
    }
    
    protected void FixedUpdate () {

        if (isStunned)
            return;

        target = GetTarget();
        distanceToTarget = Vector2.Distance ( target.transform.position, transform.position );
        inRangeToAttack = distanceToTarget < attackRange;
        isTargetDetected = canSeePlayer();

        if (!isTargetDetected)
        {
            Idle();
        }
        else
        {
            if (ShouldAttack())
            {
                Attack();
            }
            else
            {
                ChaseTarget();
            }
        }
    }

    public void MoveTowards(Vector2 targetPosition, float speed)
    {
        if (targetPosition.x < transform.position.x)
            spriteRenderer.flipX = true;
        else
            spriteRenderer.flipX = false;
        //spriteRenderer.flipX = targetPosition.x < transform.position.x;
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed);
    }

    public virtual GameObject GetTarget()
    {
        return PlayerManagement.player;
    }

    public virtual bool canSeePlayer()
    {
        return distanceToTarget < enemyDetectionDistanceCurrent;
    }

    public virtual void ChaseTarget()
    {

    }

    public virtual void Idle()
    {

    }

    public virtual bool ShouldAttack()
    {
        return false;
    }

    public virtual void Attack()
    {

    }

    public void DeathInternal()
    {
        WaveManager.EnemyDied(gameObject);
        if(deadBody)
        {
            GameObject body = Instantiate(deadBody, transform.position, transform.rotation);
            body.transform.Find("Sprite").GetComponent<SpriteRenderer>().flipX = spriteRenderer.flipX;
        }

        //PlayerExperience.GiveExperience(20);

        // Drop currency
        GameObject currencyDropped = Instantiate(copperCoin, transform.position, Quaternion.identity);
        
        if (healthGlobeDropChance >= Random.Range(0, 100))
            DropItem();

        Death();
        Destroy(gameObject);
    }

    public virtual void Death()
    {

    }

    public virtual void ApplyDamage(DamageInfo damageInfo)
    {
        // Enable our health UI if it's the first time we're hit.
        if (health == maxHealth)
            healthUI.SetActive(true);

        health -= damageInfo.damageAmount;

        // TODO: replace with "size" of hit
        if (damageInfo.damageAmount < 4)
            damageInfo.knockbackDuration /= 3;
        if (damageInfo.damageAmount > 8)
            damageInfo.knockbackDuration *= 1.5f;


        // Spawn floating damage numbers
        GameObject damageNumberToSpawn = damageInfo.criticalHit ? critDamageNumber : floatingDamageNumber;
        Vector3 damageNumberPosition = transform.position;
        damageNumberPosition.y += 7; // TODO: will this work for enemies of all sizes? Maybe map this number to "size" of enemy that doesn't exist yet but will need to for fire
        GameObject damageNumber = Instantiate(damageNumberToSpawn, damageNumberPosition, Quaternion.identity );
        damageNumber.SendMessage( "SetNumber", damageInfo.damageAmount.ToString() );

        if ( damageInfo.spawnBlood )
            Instantiate(StaticResources.blood, transform.position, Quaternion.identity);

        if ( health <= 0 )
        {
            DeathInternal();
            return;
        }

        // Play hit sound
        if (hitSound != null)
            audioSource.PlayOneShot(hitSound);

        healthbar.fillAmount = health / maxHealth;

        if (hurt != null)
            StopCoroutine(hurt);
        hurt = Hurt(damageInfo);
        StartCoroutine(hurt);

        if (canBeKnockedBack && canMove)
        {
            // Finish our old knockback before we can be knocked back again
            if (knockBack == null)
            {
                knockBack = Knockback(damageInfo);
                StartCoroutine(knockBack);
            }
        }
    }

    IEnumerator Knockback(DamageInfo damageInfo)
    {
        damageInfo.knockbackDirection.Normalize();
        Vector2 knockbackDirection = (Vector2)gameObject.transform.position + damageInfo.knockbackDirection * 100;
        float knockbackStartTime = Time.time;
        while ( true )
        {
            Vector2 position = gameObject.transform.position;
            if (Time.time > knockbackStartTime + damageInfo.knockbackDuration)
                break;
            gameObject.transform.position = Vector2.MoveTowards( gameObject.transform.position, knockbackDirection, damageInfo.knockbackSpeed );
            yield return new WaitForEndOfFrame();
        }

        knockBack = null;
    }

    public IEnumerator DisableMovementForSeconds(float seconds)
    {
        canMove = false;
        yield return new WaitForSeconds( seconds );
        canMove = true;
    }

    public virtual IEnumerator Hurt(DamageInfo damageInfo)
    {
        isStunned = true;
        float hurtAnimationSpeed = 1f;// hurtAnimation.length / damageInfo.knockbackDuration;
        spriteAnim.Play(hurtAnimation, hurtAnimationSpeed);
        yield return new WaitForSeconds(damageInfo.knockbackDuration);
        isStunned = false;
    }

    /* TODO: Again, eventually maybe don't instantiate an item here. */
    protected void DropItem()
    {
        int dropIndex = Random.Range ( 0, droppables.Length );

        if (droppables.Length == 0)
        {
            return;
        }

        // TODO: a little offset off of enemy.
        Instantiate ( droppables[ dropIndex ], transform.position, Quaternion.identity );
    }

    protected bool IsPlayerOnRightSide ()
    {
        return Vector2.Dot ( transform.right, target.transform.position ) > Vector2.Dot ( transform.right, transform.position );
    }
}
