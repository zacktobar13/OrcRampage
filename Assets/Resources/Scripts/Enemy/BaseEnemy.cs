using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PowerTools;

public class BaseEnemy : MonoBehaviour {

    // Assigned in inspector
    [Header("Basic Attributes")]
    public Rarity rarity;
    public float attackRange;
    public float movementSpeed;
    public int attackDamage;
    public float stopToAttackTime;
    public int maxHealth;
    public GameObject[] droppables;
    public float droppableDropChance;

    [Header("Scaling")]
    public int healthScalingStepSize = 10;
    public int attackDamageScalingStepSize = 3;
    [SerializeField]
    protected float[] rarityStatScalars = {1f, 2f, 3f, 4f, 5f, 6f};
    protected float rarityStatScalar = 1f;
    protected int rarityIndex;

    [Header("Sound Effects")]
    public AudioClip hitSound;
    public AudioClip hurtSound;
    public AudioClip deadSound;
    public AudioClip idleSound;

    [Header("Animations")]
    public AnimationClip idleAnimation;
    public AnimationClip movingAnimation;
    public AnimationClip hurtAnimation;
    public AnimationClip deathAnimation;

    private TimeManager timeManager;
    protected GameObject damageCollider;
    protected Transform damageSpawnPoint;

    // Referenced in Start()
    protected AudioSource audioSource;
    protected SpriteAnim spriteAnim;
    protected SpriteRenderer spriteRenderer;
    GameObject healthUI;
    Image healthbar;
    protected float health;
    public Weapon currentWeapon;
    protected GameObject worldCollider;
    protected BoxCollider2D damageTrigger;
    protected FadeOutAndDestroyOverTime fadeComponent;
    protected GameObject spriteGameObject;
    protected EnemySpawner enemySpawner;
    protected Material shaderMaterial;

    // Sometimes when enemies die, they need a different shadow for it to look natural (bigger, smaller etc)
    public GameObject aliveShadow;
    public GameObject deadShadow;
    
    public Color[] baseAccentColorChoices;
    protected Color baseAccentColor;
    public float accentColorDelta;

    public Color[] baseAccentColor2Choices;
    protected Color baseAccent2Color;
    public float accentColor2Delta;

    protected GameObject target = null;
    protected float distanceToTarget;
    protected bool canMove = true;
    protected bool canBeKnockedBack = true;
    protected bool inRangeToAttack = false;
    protected bool isTargetDetected = false;
    protected bool isStunned = false;
    protected float lastAttackTime = Mathf.NegativeInfinity;
    private IEnumerator disableMovement;
    private IEnumerator knockBack;

    protected IEnumerator telegraphAttack;
    protected IEnumerator hurt;

    bool isDisabled = false;

    protected void Start ()
    {
        // Give enemies attack range and movement speed some randomness
        //movementSpeed = Random.Range(movementSpeed * 0.8f, movementSpeed * 1.2f);
        damageTrigger = GetComponent<BoxCollider2D>();
        spriteGameObject = transform.Find("Sprite").gameObject;
        spriteGameObject.transform.localScale *= Random.Range(1f, 1.1f);

        worldCollider = transform.Find("World Collider").gameObject;
        fadeComponent = GetComponent<FadeOutAndDestroyOverTime>();

        audioSource = gameObject.GetComponent<AudioSource>();
        spriteRenderer = transform.Find("Sprite").GetComponent<SpriteRenderer>();
        healthUI = transform.Find("Enemy Health Bar").gameObject;
        healthbar = transform.Find("Enemy Health Bar/Healthbar").GetComponent<Image>();
        spriteAnim = transform.Find("Sprite").GetComponent<SpriteAnim>();
        damageSpawnPoint = transform.Find("Damage Spawn Point");

        GameObject gameManagement = GameObject.Find("Game Management");
        enemySpawner = gameManagement.GetComponent<EnemySpawner>();
        timeManager = gameManagement.GetComponent<TimeManager>();
        Debug.Assert(timeManager != null);

        shaderMaterial = transform.Find("Sprite").GetComponent<SpriteRenderer>().material;
        rarity = RollRarity();
        ProcessRarity();

        maxHealth = CalculateMaxHealth();
        attackDamage = CalculateAttackDamage();
        health = maxHealth;

        if (currentWeapon)
        {
            currentWeapon.attackDamage = attackDamage;
            currentWeapon.PickupWeapon(gameObject);
        }

        RandomizeAccentColor();
    }

    float timeUntilAttackAfterStop;
    bool hasStoppedToAttack = false;
    protected void FixedUpdate () {

        if (isDisabled)
            return;

        if (isStunned)
            return;

        target = GetTarget();
        distanceToTarget = Vector2.Distance ( target.transform.position, transform.position );
        inRangeToAttack = distanceToTarget < attackRange;
        isTargetDetected = true;

        if (!isTargetDetected)
        {
            Idle();
        }
        else
        {
            if (ShouldAttack())
            {
                if (!hasStoppedToAttack)
                {
                    hasStoppedToAttack = true;
                    timeUntilAttackAfterStop = Time.time;
                }

                if (Time.time < timeUntilAttackAfterStop + stopToAttackTime)
                {
                    Idle();
                }
                else
                {
                    Attack();
                    hasStoppedToAttack = false;
                }
            }
            else
            {
                hasStoppedToAttack = false;
                ChaseTarget();
            }
        }
    }

    public void RandomizeAccentColor()
    {
        baseAccentColor = baseAccentColorChoices[Random.Range(0, baseAccentColorChoices.Length)];
        Color randomColor = new Color(baseAccentColor.r + Random.Range(-accentColorDelta, accentColorDelta),
                                      baseAccentColor.g + Random.Range(-accentColorDelta, accentColorDelta),
                                      baseAccentColor.b + Random.Range(-accentColorDelta, accentColorDelta),
                                      baseAccentColor.a);

        shaderMaterial.SetColor("_ColorChangeNewCol", randomColor);

        if (baseAccentColor2Choices.Length > 0)
        {
            baseAccent2Color = baseAccentColor2Choices[Random.Range(0, baseAccentColor2Choices.Length)];
            Color randomColor2 = new Color(baseAccent2Color.r + Random.Range(-accentColor2Delta, accentColor2Delta),
                                           baseAccent2Color.g + Random.Range(-accentColor2Delta, accentColor2Delta),
                                           baseAccent2Color.b + Random.Range(-accentColor2Delta, accentColor2Delta),
                                           baseAccent2Color.a);

            shaderMaterial.SetColor("_ColorChangeNewCol2", randomColor2);
        }
    }

    /** Sets the rarity of the enemy */
    public void SetRarity(Rarity r)
    {
        rarity = r;
    }

    private Rarity RollRarity()
    {
        float roll = Random.Range(0f, 100f);
        if (roll <= 95f)
            return Rarity.COMMON;
        else if (roll <= 99.3f)
            return Rarity.UNCOMMON;
        else if (roll <= 99.6f)
            return Rarity.MAGIC;
        else if (roll <= 99.8f)
            return Rarity.EPIC;
        else if (roll <= 99.95f)
            return Rarity.LEGENDARY;
        else
            return Rarity.ANCIENT;
    }

    /** Handles the logic for changing the enemy based on their rarity.
     * i.e., setting the stat scalar value, size, changing outline color etc. */
    public void ProcessRarity()
    {
		if (rarity != Rarity.COMMON)
        {
            shaderMaterial.SetFloat("_OutlineAlpha", 1f);
        }

        rarityIndex = (int)rarity;

        Color rarityColor = RarityUtil.GetRarityColor(rarity);
        shaderMaterial.SetColor("_OutlineColor", rarityColor);

        spriteGameObject.transform.localScale *= RarityUtil.GetRaritySizeScalar(rarity);
        rarityStatScalar = rarityStatScalars[rarityIndex];
	}

	int CalculateMaxHealth()
    {
        int numberOfScalingSteps = (int)timeManager.GetTimeInRound() / 30;
        return (int)((maxHealth + healthScalingStepSize * numberOfScalingSteps) * rarityStatScalar);
    }

    int CalculateAttackDamage()
    {
        int numberOfScalingSteps = (int)timeManager.GetTimeInRound() / 30;
        return (int)((attackDamage + attackDamageScalingStepSize * numberOfScalingSteps) * rarityStatScalar);
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

    public virtual void Anim_EnableDeadShadow()
    {
        aliveShadow.SetActive(false);
        deadShadow.SetActive(true);
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
        enemySpawner.EnemyDeath(this);

        spriteAnim.Play(deathAnimation);
        DisableComponentsOnDeath();

        /* Drop number of XP globes based on enemy's rarity index (1-6) */
        for (int i = 0; i < rarityIndex + 1; i++)
        {
            GameObject xpDropped = Instantiate(StaticResources.xpGlobe, transform.position, Quaternion.identity);
        }
        
        if (droppableDropChance >= Random.Range(0, 100))
            DropItem();

        Death();
    }

    public virtual void Death()
    {

    }

    void DisableComponentsOnDeath()
    {
        isDisabled = true;
        currentWeapon.gameObject.SetActive(false);
        worldCollider.SetActive(false);
        damageTrigger.enabled = false;

        if (fadeComponent)
        {
            fadeComponent.enabled = true;
        }

        // Reset health bar UI
        healthbar.fillAmount = 1f;
        healthUI.SetActive(false);
    }

    public virtual void ApplyDamage(DamageInfo damageInfo)
    {
        // We're already dead -- don't do anything
        if (health <= 0)
            return;

        // Enable our health UI if it's the first time we're hit.
        if (health == maxHealth)
            healthUI.SetActive(true);

        health -= damageInfo.damageAmount;

        // Spawn floating damage numbers
        GameObject damageNumberToSpawn = damageInfo.criticalHit ? StaticResources.critDamageNumber : StaticResources.damageNumber;
        Vector3 damageNumberPosition = transform.position;
        damageNumberPosition.y += 7; // TODO: will this work for enemies of all sizes? Maybe map this number to "size" of enemy that doesn't exist yet but will need to for fire
        GameObject damageNumber = Instantiate( damageNumberToSpawn, damageNumberPosition, Quaternion.identity );
        damageNumber.SendMessage( "SetNumber", damageInfo.damageAmount.ToString() );

        if ( damageInfo.spawnBlood )
            Instantiate(StaticResources.blood, transform.position, Quaternion.identity);


        // Play hit sound
        //if (hitSound != null)
        //    SoundManager.PlayOneShot(audioSource, hitSound);

        healthbar.fillAmount = health / maxHealth;


        if (canBeKnockedBack && canMove)
        {
            // Finish our old knockback before we can be knocked back again
            if (knockBack == null)
            {
                knockBack = Knockback(damageInfo);
                StartCoroutine(knockBack);
            }
        }

        if ( health <= 0 )
        {
            DeathInternal();
            return;
        }

        if (hurt != null)
            StopCoroutine(hurt);
        hurt = Hurt(damageInfo);
        StartCoroutine(hurt);
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
            yield return new WaitForSeconds(Time.fixedDeltaTime);
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
