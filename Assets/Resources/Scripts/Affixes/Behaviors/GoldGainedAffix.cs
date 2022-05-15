using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldGainedAffix : BaseAffix
{
    public void Start()
    {
        playerStats.IncreaseGoldGainedScalar(scalarIncreasePerAffix);
    }

    public override void AddAffixCount(int increment)
    {
        base.AddAffixCount(increment);
        for (int i = 0; i < increment; i++)
        {
            playerStats.IncreaseGoldGainedScalar(scalarIncreasePerAffix);
        }
    }
}
