using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSpeedAffix : BaseAffix
{
    public void Start()
    {
        playerStats.IncreaseAttackSpeedScalar(scalarIncreasePerAffix);
    }

    public override void AddAffixCount(int increment)
    {
        base.AddAffixCount(increment);
        for (int i = 0; i < increment; i++)
        {
            playerStats.IncreaseAttackSpeedScalar(scalarIncreasePerAffix);
        }
    }
}
