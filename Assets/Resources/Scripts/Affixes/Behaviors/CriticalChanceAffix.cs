using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CriticalChanceAffix : BaseAffix
{
    public void Start()
    {
        playerStats.IncreaseCriticalChanceAffix(scalarIncreasePerAffix);
    }

    public override void AddAffixCount(int increment)
    {
        base.AddAffixCount(increment);
        for (int i = 0; i < increment; i++)
        {
            playerStats.IncreaseCriticalChanceAffix(scalarIncreasePerAffix);
        }
    }
}
