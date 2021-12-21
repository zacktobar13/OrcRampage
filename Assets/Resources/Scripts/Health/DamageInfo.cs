using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct DamageInfo
{
    static private float defaultKnockbackSpeed = 0.15f;
    static private float defaultKnockbackDuration = 0.1f;
    public int damageAmount;
    public Vector2 knockbackDirection;
    public float knockbackSpeed;
    public float knockbackDuration;
    public bool criticalHit;
    public bool spawnBlood;

    public DamageInfo(int damage, bool isCrit)
    {
        damageAmount = damage;
        criticalHit = isCrit;

        knockbackDirection = Vector2.zero;
        knockbackSpeed = defaultKnockbackSpeed;
        knockbackDuration = defaultKnockbackDuration;

        spawnBlood = false;
    }

    public DamageInfo(int damage, bool isCrit, Vector2 dir)
    {
        damageAmount = damage;
        criticalHit = false;

        knockbackDirection = dir.normalized;
        knockbackSpeed = defaultKnockbackSpeed;
        knockbackDuration = defaultKnockbackDuration;

        spawnBlood = false;
    }

    public DamageInfo( int damage, Vector2 dir, float knockbackSpeedScalar )
    {
        damageAmount = damage;
        criticalHit = false;

        knockbackDirection = dir.normalized;
        knockbackSpeed = defaultKnockbackSpeed * knockbackSpeedScalar;
        knockbackDuration = defaultKnockbackDuration;

        spawnBlood = false;
    }

    public DamageInfo( int damage, Vector2 dir, float knockbackSpeedScalar, float knockbackDistanceScalar )
    {
        damageAmount = damage;
        criticalHit = false;

        knockbackDirection = dir.normalized * knockbackDistanceScalar;
        knockbackSpeed = defaultKnockbackSpeed * knockbackSpeedScalar;
        knockbackDuration = defaultKnockbackDuration;

        spawnBlood = false;
    }

    public DamageInfo(int damage, Vector2 dir, float knockbackSpeedScalar, float knockbackDistanceScalar, bool crit )
    {
        damageAmount = damage;
        criticalHit = crit;

        knockbackDirection = dir.normalized * knockbackDistanceScalar;
        knockbackSpeed = defaultKnockbackSpeed * knockbackSpeedScalar;
        knockbackDuration = defaultKnockbackDuration;

        spawnBlood = false;
    }

    public DamageInfo(int damage, Vector2 dir, float knockbackSpeedScalar, float knockbackDistanceScalar, bool crit, bool blood)
    {
        damageAmount = damage;
        criticalHit = crit;

        knockbackDirection = dir.normalized * knockbackDistanceScalar;
        knockbackSpeed = defaultKnockbackSpeed * knockbackSpeedScalar;
        knockbackDuration = defaultKnockbackDuration;

        spawnBlood = blood;
    }
}
