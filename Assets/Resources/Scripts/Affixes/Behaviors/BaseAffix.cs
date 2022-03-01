using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class adds no behavior but lets us do things like player.GetComponents<BaseAffix>() to get all affixes
/// </summary>

public class BaseAffix : MonoBehaviour
{
    public Sprite affixIcon;
    public string affixName;
    public string affixDescription;
    public bool isUnique;
    public float scalarIncreasePerAffix;
    public int baseDamage;
    public float baseProcChance;
    public int affixCount = 1;
    protected PlayerStats playerStats;

    public virtual void AddAffixCount(int increment)
    {
        affixCount += increment;
    }

    public virtual void IntializeFromScriptableObject(AffixObject data)
    {
        affixIcon = data.icon;
        affixName = data.name;
        affixDescription = data.affixDescription;
        isUnique = data.isUniqueAffix;
        baseDamage = data.baseDamage;
        baseProcChance = data.baseProcChance;
        scalarIncreasePerAffix = data.scalarIncreasePerAffix;
        playerStats = GetComponent<PlayerStats>();
    }
}
