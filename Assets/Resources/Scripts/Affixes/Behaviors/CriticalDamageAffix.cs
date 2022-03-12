using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CriticalDamageAffix : BaseAffix
{
    public void Start()
    {
        playerStats.IncreaseCriticalDamageScalar(scalarIncreasePerAffix);
    }

    public override void AddAffixCount(int increment)
    {
        base.AddAffixCount(increment);
        for (int i = 0; i < increment; i++)
        {
            playerStats.IncreaseCriticalDamageScalar(scalarIncreasePerAffix);
        }
    }
}
