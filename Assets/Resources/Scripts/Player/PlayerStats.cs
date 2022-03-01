using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    float damageScalar = 1f;
    float maxHealthScalar = 1f;
    float attackSpeedScalar = 1f;
    float movementSpeedScalar = 1f;
    float experienceGainedScalar = 1f;
    float magnetismDistanceScalar = 1f;

    PlayerHealth playerHealth;

    private void Start()
    {
        playerHealth = GetComponent<PlayerHealth>();
    }

    public void ResetStats()
    {
        damageScalar = 1f;
        maxHealthScalar = 1f;
        attackSpeedScalar = 1f;
        movementSpeedScalar = 1f;
        experienceGainedScalar = 1f;
        magnetismDistanceScalar = 1f;

        playerHealth.UpdateMaxHealth();
    }

    public int CalculateMaxHealth(int baseMaxHealth)
    {
        return (int)(baseMaxHealth * maxHealthScalar);
    }

    public float CalculateMagnetismDistance(float baseRadius)
    {
        return baseRadius * magnetismDistanceScalar;
    }

    public int CalculateDamage(int baseDamage)
    {
        return (int)(baseDamage * damageScalar);
    }

    public float CalculateMovementSpeed(float baseMovementSpeed)
    {
        return movementSpeedScalar * baseMovementSpeed;
    }

    public float CalculateAttackSpeed(float baseAttacksPerSecond)
    {
        return baseAttacksPerSecond * attackSpeedScalar;
    }

    public int CalculateExperienceGained(int baseExperienceGained)
    {
        return (int)(baseExperienceGained * experienceGainedScalar);
    }

    public void IncreaseDamageScalar(float amount)
    {
        damageScalar += amount;
    }

    public void IncreaseAttackSpeedScalar(float amount)
    {
        attackSpeedScalar += amount;
    }

    public void IncreaseMovementSpeedScalar(float amount)
    {
        movementSpeedScalar += amount;
    }

    public void IncreaseExperienceGainedScalar(float amount)
    {
        experienceGainedScalar += amount;
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
