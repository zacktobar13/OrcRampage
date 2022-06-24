using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Health Info")]
    [SerializeField] public int baseMaxHealth;
    [SerializeField] float maxHealthScalar = 1f;

    [Header("Damage Info")]
    [SerializeField] int baseDamage;
    [SerializeField] float damageScalar = 1f;
    [SerializeField] float attackSpeedScalar = 1f;

    [Header("Crit Info")]
    [SerializeField] float criticalChance;
    [SerializeField] float criticalDamageScalar = 1.5f;

    [Header("Uncategorized Scalars")]
    [SerializeField] float movementSpeedScalar = 1f;
    [SerializeField] float goldGainedScalar = 1f;
    [SerializeField] float magnetismDistanceScalar = 1f;

    PlayerHealth playerHealth;


    int startingMaxHealth;
    int startingBaseDamage;
    float startingCritChance;
    private void Start()
    {
        startingMaxHealth = baseMaxHealth;
        startingBaseDamage = baseDamage;
        startingCritChance = criticalChance;

        playerHealth = GetComponent<PlayerHealth>();
    }

    public void LevelUp()
    {
        baseDamage += 1;
        baseMaxHealth += 2;
        playerHealth.UpdateMaxHealth();
    }

    public void ResetStats()
    {
        criticalDamageScalar = 1.5f;
        criticalChance = startingCritChance;

        damageScalar = 1f;
        maxHealthScalar = 1f;
        attackSpeedScalar = 1f;
        movementSpeedScalar = 1f;
        goldGainedScalar = 1f;
        magnetismDistanceScalar = 1f;

        baseMaxHealth = startingMaxHealth;
        baseDamage = startingBaseDamage;

        playerHealth.UpdateMaxHealth();
    }

    public int CalculateMaxHealth()
    {
        return (int)(baseMaxHealth * maxHealthScalar);
    }

    public float CalculateMagnetismDistance(float baseRadius)
    {
        return baseRadius * magnetismDistanceScalar;
    }

    public bool RollCritical()
    {
        return Random.Range(0f, 100f) <= criticalChance;
    }

    public int CalculateDamage(bool isCritical, float percentageOfBaseDamage)
    {
        Vector2 damageSpread = new Vector2(.9f, 1.1f);
        if (isCritical)
            return (int)((baseDamage * damageScalar * criticalDamageScalar) * percentageOfBaseDamage * Random.Range(damageSpread.x, damageSpread.y));
        else
            return (int)(baseDamage * damageScalar * percentageOfBaseDamage * Random.Range(damageSpread.x, damageSpread.y));
    }

    public int CalculateDamage(bool isCritical)
    {
        return CalculateDamage(isCritical, 1);
    }

    public float CalculateMovementSpeed(float baseMovementSpeed, float attackMovementScalar)
    {
        return movementSpeedScalar * baseMovementSpeed * attackMovementScalar;
    }

    public float CalculateAttackSpeed(float baseAttacksPerSecond)
    {
        return baseAttacksPerSecond * attackSpeedScalar;
    }

    public int CalculateGoldGained(int baseGoldGained)
    {
        return (int)(baseGoldGained * goldGainedScalar);
    }

    public void IncreaseDamageScalar(float amount)
    {
        damageScalar += amount;
    }

    public void IncreaseCriticalDamageScalar(float amount)
    {
        criticalDamageScalar += amount;
    }

    public void IncreaseCriticalChanceAffix(float amount)
    {
        criticalChance += amount;
    }

    public void IncreaseAttackSpeedScalar(float amount)
    {
        attackSpeedScalar += amount;
    }

    public void IncreaseMovementSpeedScalar(float amount)
    {
        movementSpeedScalar += amount;
    }

    public void IncreaseGoldGainedScalar(float amount)
    {
        goldGainedScalar += amount;
    }

    public void IncreaseMaxHealthScalar(float amount)
    {
        maxHealthScalar += amount;
        playerHealth.UpdateMaxHealth();
    }

    public void IncreaseMagnetismDistanceScalar(float amount)
    {
        magnetismDistanceScalar += amount;
    }
}
