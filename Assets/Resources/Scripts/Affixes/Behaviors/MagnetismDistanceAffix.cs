using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetismDistanceAffix : BaseAffix
{
    public void Start()
    {
        playerStats.IncreaseMagnetismDistanceScalar(scalarIncreasePerAffix);
    }

    public override void AddAffixCount(int increment)
    {
        base.AddAffixCount(increment);
        for (int i = 0; i < increment; i++)
        {
            playerStats.IncreaseMagnetismDistanceScalar(scalarIncreasePerAffix);
        }
    }
}
