using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackDamageAffix : BaseAffix
{
    public void Start()
    {
        playerStats.IncreaseDamageScalar(scalarIncreasePerAffix);
    }

    public override void AddAffixCount(int increment)
    {
        base.AddAffixCount(increment);
        for (int i = 0; i < increment; i++)
        {
            playerStats.IncreaseDamageScalar(scalarIncreasePerAffix);
        }
    }
}
