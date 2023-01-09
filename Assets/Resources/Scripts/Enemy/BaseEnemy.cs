using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Pool;

public class BaseEnemy : MonoBehaviour {

    // Assigned in inspector
    [Header("Basic Attributes")]
    public Rarity rarity;
    public bool forceUseInspectorRarity;
    public float attackRange;
    public float movementSpeed;
    [SerializeField]int attackDamage;
    public float attackCooldown;
    public float attackRadius;
    public float stopToAttackTime;
    public int maxHealth;
    public float droppableDropChance;

    [SerializeField]
    protected float[] rarityStatScalars = {1f, 2f, 3f, 4f, 5f, 6f};
    protected float rarityStatScalar = 1f;
    protected int rarityIndex;

    private TimeManager timeManager;
    protected GameObject damageCollider;
    protected Transform damageSpawnPoint;

    // Referenced in Start()
    protected AudioSource audioSource;
    protected SpriteRenderer spriteRenderer;
    GameObject healthUI;
    Image healthbar;
    protected float health;
    protected GameObject worldCollider;
    protected BoxCollider2D damageTrigger;
    protected FadeOutAndDestroyOverTime fadeComponent;
    protected GameObject spriteGameObject;
    protected AffixManager affixManager;
    protected EnemySpawner enemySpawner;
    protected Material shaderMaterial;
    [SerializeField]
    protected SpriteRenderer weaponSprite;
    
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
    protected float lastAttackTime = -100f;
    private IEnumerator disableMovement;
    private IEnumerator knockBack;

    protected IEnumerator telegraphAttack;
    protected IEnumerator hurt;

    bool isDisabled = false;

    protected ObjectPool<GameObject> myPool;
    protected ObjectPool<GameObject> coinPool;
    Transform coinPoolParent;

    StateMachine stateMachine;

    private void Awake() {
        stateMachine = new StateMachine();
    }

    protected void Start ()
    {
        // Give enemies attack range and movement speed some randomness
        //movementSpeed = Random.Range(movementSpeed * 0.8f, movementSpeed * 1.2f);
        
        spriteGameObject.transform.localScale *= Random.Range(1f, 1.1f);

        fadeComponent = GetComponent<FadeOutAndDestroyOverTime>();

        audioSource = gameObject.GetComponent<AudioSource>();
        spriteRenderer = transform.Find("Sprite").GetComponent<SpriteRenderer>();

        
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
        if (weaponSprite) { weaponSprite.enabled = true; }
        isDisabled = false;

        Debug.Assert(attackDamage != 0);
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

    public StateMachine GetStateMachine()
    {
        return stateMachine;
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
            stateMachine.ChangeState(EnemyState.IDLE);
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
					stateMachine.ChangeState(EnemyState.IDLE);
                }
                else
                {
					stateMachine.ChangeState(EnemyState.ATTACK);
                    Attack();
                    lastAttackTime = Time.time;
					hasStoppedToAttack = false;
                }
            }
            else
            {
                hasStoppedToAttack = false;
				stateMachine.ChangeState(EnemyState.MOVING);
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

	protected virtual int CalculateMaxHealth()
    {
        return (int)(maxHealth * rarityStatScalar);
        //int numberOfScalingSteps = (int)timeManager.GetTimeInRound() / 30;
        //return (int)((maxHealth + healthScalingStepSize * numberOfScalingSteps) * rarityStatScalar);
    }

    protected virtual int CalculateAttackDamage()
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

    public virtual void ChaseTarget()
    {
        if (distanceToTarget >= attackRange)
        {
            // Chase Target
            MoveTowards(target.transform.position, movementSpeed * Time.deltaTime);
        }
    }

    public virtual bool ShouldAttack()
    {
        return (inRangeToAttack && Time.time > lastAttackTime + attackCooldown);
    }

    public virtual void Attack()
    {
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
        stateMachine.ChangeState(EnemyState.DEAD);

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
        worldCollider.SetActive(false);
        damageTrigger.enabled = false;

        if (weaponSprite) { weaponSprite.enabled = false; }

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
        stateMachine.ChangeState(EnemyState.HURT);
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

public class StateMachine
{
    private EnemyState previousState;
    private EnemyState currentState;

    public StateMachine()
    {
        previousState = currentState = EnemyState.IDLE;
    }

    public void ChangeState(EnemyState newState)
    {
        previousState = currentState;
        currentState = newState;
    }

    public EnemyState GetCurrentState()
    {
        return currentState;
    }

    public EnemyState GetPreviousState()
    {
        return previousState;
    }
}

public enum EnemyState
{
    IDLE,
    MOVING,
    ATTACK,
    HURT,
    DEAD
}