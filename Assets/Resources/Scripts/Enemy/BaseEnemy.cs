using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using PowerTools;
using UnityEngine.Pool;

public class BaseEnemy : MonoBehaviour {

    // Assigned in inspector
    [Header("Basic Attributes")]
    public Rarity rarity;
    public bool forceUseInspectorRarity;
    public float attackRange;
    public float movementSpeed;
    public int attackDamage;
    public float attackCooldown;
    public float attackRadius;
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

    [Header("Animations")]
    public AnimationClip idleAnimation;
    public AnimationClip movingAnimation;
    public AnimationClip hurtAnimation;
    public AnimationClip deathAnimation;

    private TimeManager timeManager;
    protected GameObject damageCollider;
    protected Transform damageSpawnPoint;

    [Header("Portal")]
    public bool spawnPortalOnDeath;
    public GameObject portal;
    public string nextScene;

    // Referenced in Start()
    protected AudioSource audioSource;
    protected SpriteAnim spriteAnim;
    protected SpriteRenderer spriteRenderer;
    GameObject healthUI;
    Image healthbar;
    protected float health;
    public GameObject currentWeapon;
    protected GameObject worldCollider;
    protected BoxCollider2D damageTrigger;
    protected FadeOutAndDestroyOverTime fadeComponent;
    protected GameObject spriteGameObject;
    protected AffixManager affixManager;
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
    protected bool targetIsOnRight;
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

    protected ObjectPool<GameObject> myPool;
    protected ObjectPool<GameObject> coinPool;
    Transform coinPoolParent;

    protected void Start ()
    {
        // Give enemies attack range and movement speed some randomness
        //movementSpeed = Random.Range(movementSpeed * 0.8f, movementSpeed * 1.2f);

        spriteGameObject.transform.localScale *= Random.Range(1f, 1.1f);

        fadeComponent = GetComponent<FadeOutAndDestroyOverTime>();

        audioSource = gameObject.GetComponent<AudioSource>();
        spriteRenderer = transform.Find("Sprite").GetComponent<SpriteRenderer>();

        spriteAnim = transform.Find("Sprite").GetComponent<SpriteAnim>();
        damageSpawnPoint = transform.Find("Damage Spawn Point");

        GameObject gameManagement = GameObject.Find("Game Management");
        affixManager = gameManagement.GetComponent<AffixManager>();
        timeManager = gameManagement.GetComponent<TimeManager>();
        Debug.Assert(timeManager != null);
    }

    private void OnEnable()
    {
        coinPool = GameObject.Find("Game Management").GetComponent<PoolManager>().GetObjectPool(StaticResources.copperCoin);
        string copperCoinPoolName = "Game Management/" + StaticResources.copperCoin.name + " Pool";
        coinPoolParent = GameObject.Find(copperCoinPoolName).transform;
        healthUI = transform.Find("Enemy Health Bar").gameObject;
        healthbar = transform.Find("Enemy Health Bar/Healthbar").GetComponent<Image>();
        damageTrigger = GetComponent<BoxCollider2D>();
        worldCollider = transform.Find("World Collider").gameObject;
        spriteGameObject = transform.Find("Sprite").gameObject;
        shaderMaterial = transform.Find("Sprite").GetComponent<SpriteRenderer>().material;
        rarity = forceUseInspectorRarity ? rarity : RollRarity();
        ProcessRarity();

        maxHealth = CalculateMaxHealth();
        attackDamage = CalculateAttackDamage();
        health = maxHealth;

        isDisabled = false;
        if (currentWeapon)
        {
            currentWeapon.gameObject.SetActive(true);
            Debug.Assert(attackDamage != 0);
        }
        worldCollider.SetActive(true);
        damageTrigger.enabled = true;

        if (fadeComponent)
        {
            fadeComponent.enabled = false;
        }

        // Reset health bar UI
        healthbar.fillAmount = 1f;
        healthUI.SetActive(false);

        RandomizeAccentColor();
    }

    public void SetEnemySpawner(EnemySpawner spawner, ObjectPool<GameObject> pool) {
        enemySpawner = spawner;
        myPool = pool;
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
		if (rarity == Rarity.COMMON)
        {
            shaderMaterial.SetFloat("_OutlineAlpha", 0f);
        }

        rarityIndex = (int)rarity;

        Color rarityColor = RarityUtil.GetRarityColor(rarity);
        shaderMaterial.SetColor("_OutlineColor", rarityColor);

        spriteGameObject.transform.localScale *= RarityUtil.GetRaritySizeScalar(rarity);
        rarityStatScalar = rarityStatScalars[rarityIndex];
	}

	int CalculateMaxHealth()
    {
        return (int)(maxHealth * rarityStatScalar);
        //int numberOfScalingSteps = (int)timeManager.GetTimeInRound() / 30;
        //return (int)((maxHealth + healthScalingStepSize * numberOfScalingSteps) * rarityStatScalar);
    }

    int CalculateAttackDamage()
    {
        return (int)(attackDamage * rarityStatScalar);
        //int numberOfScalingSteps = (int)timeManager.GetTimeInRound() / 30;
        //return (int)((attackDamage + attackDamageScalingStepSize * numberOfScalingSteps) * rarityStatScalar);
    }

    public void MoveTowards(Vector2 targetPosition, float speed)
    {
        bool targetIsOnLeft = targetPosition.x < transform.position.x;
        spriteRenderer.flipX = targetIsOnLeft;
        targetIsOnRight = !targetIsOnLeft;
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
        if (distanceToTarget >= attackRange)
        {
            // Chase Target
            spriteAnim.Play(movingAnimation);
            MoveTowards(target.transform.position, movementSpeed * Time.deltaTime);
        }
        else
        {
            // Idle
            spriteAnim.Play(idleAnimation);
        }
    }

    public virtual void Idle()
    {
        spriteAnim.Play(idleAnimation);
    }

    public virtual bool ShouldAttack()
    {
        return (inRangeToAttack && Time.time > lastAttackTime + attackCooldown);
    }

    public virtual void Attack()
    {
        if (!ShouldAttack())
            return;

        Vector2 directionToTarget = (target.transform.position - transform.position).normalized;
        Collider2D[] objectsHit = Physics2D.OverlapCircleAll((Vector2)transform.position + directionToTarget, attackRadius);

        StartCoroutine(VisualEffects());
        PlayAttackSound();

        foreach (Collider2D collider in objectsHit)
        {
            PlayerHealth playerHealth;
            if (collider.TryGetComponent(out playerHealth))
            {
                DamageInfo damageInfo = new DamageInfo(CalculateAttackDamage(), false, directionToTarget);
                playerHealth.ApplyDamage(damageInfo);
                continue;
            }
            MapClutter mapClutter;
            if (collider.TryGetComponent(out mapClutter))
            {
                DamageInfo damageInfo = new DamageInfo(CalculateAttackDamage(), false, directionToTarget);
                mapClutter.ApplyDamage(damageInfo);
                continue;
            }
        }
        
        lastAttackTime = Time.time;
    }

    public virtual IEnumerator VisualEffects()
    {
        yield return new WaitForEndOfFrame();
        // if (visualEffect)
        // {
        //     GameObject visuals = Instantiate(visualEffect, visualEffectsSpawnPoint.position, visualEffectsSpawnPoint.rotation);
        //     visuals.GetComponent<PowerTools.SpriteAnim>().SetSpeed(1/visualsCooldown);
        // }

        // sprite.sprite = firingSprite;
        // yield return new WaitForSeconds(visualsCooldown);
        // sprite.sprite = notFiringSprite;
    }

    public virtual void PlayAttackSound()
    {
        //SoundManager.PlayOneShot(transform.position, shootSound, new SoundManagerArgs(true, shootSound.name));
    }

    public void DeathInternal()
    {
        enemySpawner.EnemyDeath(this, null);

        if (spawnPortalOnDeath)
        {
            GameObject newPortal = Instantiate(portal, transform.position, Quaternion.identity);
            Portal portalData = newPortal.GetComponent<Portal>();
            portalData.SetNextScene(nextScene);
        }

        spriteAnim.Play(deathAnimation);
        DisableComponentsOnDeath();

        /* Drop number of coins based on enemy's rarity index (1-6) */
        for (int i = 0; i < Random.Range(1, 3)*(rarityIndex + 1); i++)
        {
            GameObject coinObject = coinPool.Get();
            coinObject.transform.position = transform.position;
            DroppedItem droppedItemComponent = coinObject.GetComponent<DroppedItem>();
            droppedItemComponent.SetMyPool(coinPool);
            coinObject.transform.parent = coinPoolParent.transform;
        }
        
        if (droppableDropChance >= Random.Range(0, 100))
            DropItem();

        PlayerSerializedStats.IncrementLifetimeKills();

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
            fadeComponent.myPool = myPool;
        }
        else
        {
            if (myPool != null)
                myPool.Release(gameObject);
            else
                Destroy(gameObject);
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
        Rarity[] rarities = { Rarity.COMMON, Rarity.UNCOMMON, Rarity.MAGIC, Rarity.EPIC, Rarity.LEGENDARY, Rarity.ANCIENT, };
        Instantiate (affixManager.GetRandomAffixDrop(rarities[Random.Range(0, rarities.Length)]), transform.position, Quaternion.identity );
    }

    protected bool IsPlayerOnRightSide ()
    {
        return Vector2.Dot ( transform.right, target.transform.position ) > Vector2.Dot ( transform.right, transform.position );
    }
}
