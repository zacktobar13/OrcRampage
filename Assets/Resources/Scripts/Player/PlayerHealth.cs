using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int health;
    public int currentMaxHealth;
    [SerializeField] int baseMaxHealth;
    public GameObject floatingDamageNumber;
    public GameObject floatingHealNumber;
    GameObject bow;
    [HideInInspector] public SpriteRenderer spriteRenderer;
    AudioSource audioSource;
    PlayerMovement playerMovement;
    PlayerAnimation playerAnimation;
    PlayerStats playerStats;
    GameplayUI gameplayUI;

    public bool isCurrentlyDead = false;
    public bool hurtThisFrame = false;

    public delegate void OnDeath(PlayerHealth playerHealth);
    public static event OnDeath onDeath;

    public delegate void OnRespawn(PlayerHealth playerHealth);
    public static event OnRespawn onRespawn;

    public delegate void OnDamageTaken(PlayerHealth playerHealth);
    public static event OnDamageTaken onDamageTaken;

    public delegate void OnHeal(PlayerHealth playerHealth);
    public static event OnHeal onHeal;


    void Awake()
    {
        health = baseMaxHealth;
    }

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerAnimation = GetComponent<PlayerAnimation>();
        playerStats = GetComponent<PlayerStats>();
        audioSource = GetComponent<AudioSource>();

        spriteRenderer = transform.Find("Sprite").GetComponent<SpriteRenderer>();
        gameplayUI = GameObject.Find("Gameplay UI").GetComponent<GameplayUI>();
        currentMaxHealth = playerStats.CalculateMaxHealth(baseMaxHealth);
        gameplayUI.UpdatePlayerHealth(health, baseMaxHealth);
        
        if (onRespawn != null)
            onRespawn(this);
    }

    public void UpdateMaxHealth()
    {
        float currentHealthPercentage = health / (float)currentMaxHealth;
        currentMaxHealth = playerStats.CalculateMaxHealth(baseMaxHealth);
        health = Mathf.CeilToInt(currentMaxHealth * currentHealthPercentage);
        gameplayUI.UpdatePlayerHealth(health, currentMaxHealth);
    }

    public bool IsAtMaxHealth()
    {
        return currentMaxHealth == health;
    }

    public bool IsImmuneToDamage()
    {
        return playerMovement.isDodgeRolling;
    }

    public void ApplyDamage(DamageInfo damageInfo)
    {
        if (IsImmuneToDamage())
            return;

        health -= damageInfo.damageAmount;
        if (health < 0)
        {
            health = 0;
        }

        if (onDamageTaken != null)
            onDamageTaken(this);

        StartCoroutine(playerAnimation.PlayHurtAnimation(damageInfo.knockbackDirection));

        if (!isCurrentlyDead && health <= 0)
        {
            Death();
        }

        GameObject damageNumber = Instantiate(floatingDamageNumber, new Vector3(transform.position.x, transform.position.y + 7f, transform.position.z), Quaternion.identity);
        damageNumber.SendMessage("SetNumber", damageInfo.damageAmount.ToString());

        if (damageInfo.spawnBlood)
        {
            GameObject bloodParticle = Instantiate(StaticResources.blood, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
        }

        gameplayUI.UpdatePlayerHealth(health, currentMaxHealth);
    }
    public void ApplyHeal(HealInfo healInfo)
    {
        SoundManager.PlayOneShot(audioSource, healInfo.healSound, new SoundManagerArgs(5));
        if (IsAtMaxHealth())
            return;

        health += healInfo.healAmount;
        int overHealAmount = 0;

        if (onHeal != null)
            onHeal(this);

        if (health > currentMaxHealth)
        {
            overHealAmount = health - currentMaxHealth;
            health = currentMaxHealth;
        }

        GameObject healNumber = Instantiate(floatingHealNumber, new Vector3(transform.position.x, transform.position.y + 7f, transform.position.z), Quaternion.identity);
        healNumber.SendMessage("SetNumber", (healInfo.healAmount - overHealAmount).ToString());

        gameplayUI.UpdatePlayerHealth(health, currentMaxHealth);
    }

    public void Death()
    {
        isCurrentlyDead = true;

        if (onDeath != null)
            onDeath(this);

        PlayerManagement.TogglePlayerControl(false);
        gameplayUI.StartCoroutine("ShowDeathPanelAfterSeconds", 1f);
    }

    public void Respawn()
    {
        isCurrentlyDead = false;
        health = baseMaxHealth;
    }
}
