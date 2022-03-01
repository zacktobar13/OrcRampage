using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxHealthAffix : BaseAffix
{
    public void Start()
    {
        playerStats.IncreaseMaxHealthScalar(scalarIncreasePerAffix);
    }

    public override void AddAffixCount(int increment)
    {
        base.AddAffixCount(increment);
        for (int i = 0; i < increment; i++)
        {
            playerStats.IncreaseMaxHealthScalar(scalarIncreasePerAffix);
        }
    }
}
