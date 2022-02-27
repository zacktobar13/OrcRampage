using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using EZCameraShake;

public class PlayerHealth : MonoBehaviour
{
    public int health;
    public int maxHealth;
    public GameObject floatingDamageNumber;
    public GameObject floatingHealNumber;
    GameObject bow;
    [HideInInspector] public SpriteRenderer spriteRenderer;
    PlayerMovement playerMovement;
    PlayerAnimation playerAnimation;
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
        health = maxHealth;
    }

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerAnimation = GetComponent<PlayerAnimation>();

        spriteRenderer = transform.Find("Sprite").GetComponent<SpriteRenderer>();
        gameplayUI = GameObject.Find("Gameplay UI").GetComponent<GameplayUI>();
        gameplayUI.UpdatePlayerHealth(health, maxHealth);

        if (onRespawn != null)
            onRespawn(this);
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

        gameplayUI.UpdatePlayerHealth(health, maxHealth);
    }
    public void ApplyHeal(HealInfo healInfo)
    {
        health += healInfo.healAmount;

        if (onHeal != null)
            onHeal(this);

        if (health > maxHealth)
            health = maxHealth;

        GameObject healNumber = Instantiate(floatingHealNumber, new Vector3(transform.position.x, transform.position.y + 7f, transform.position.z), Quaternion.identity);
        healNumber.SendMessage("SetNumber", healInfo.healAmount.ToString());

        gameplayUI.UpdatePlayerHealth(health, maxHealth);
    }

    public void Death()
    {
        isCurrentlyDead = true;

        if (onDeath != null)
            onDeath(this);

        playerMovement.movementEnabled = false;
    }

    public void Respawn()
    {
        isCurrentlyDead = false;
        health = maxHealth;

        if (onRespawn != null)
            onRespawn(this);

        bow.SetActive(true);
        playerMovement.movementEnabled = true;

        BaseAffix[] affixes = gameObject.GetComponents<BaseAffix>();
        foreach (BaseAffix affix in affixes)
        {
            Destroy(affix);
        }
    }
}
