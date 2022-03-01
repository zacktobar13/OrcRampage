using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    float damageScalar = 1f;
    float attackSpeedScalar = 1f;
    float movementSpeedScalar = 1f;
    float experienceGainedScalar = 1f;

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
}
