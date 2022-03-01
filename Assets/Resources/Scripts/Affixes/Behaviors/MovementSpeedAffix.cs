using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementSpeedAffix : BaseAffix
{
    public void Start()
    {
        playerStats.IncreaseMovementSpeedScalar(scalarIncreasePerAffix);
    }

    public override void AddAffixCount(int increment)
    {
        base.AddAffixCount(increment);
        for ( int i = 0; i < increment; i++)
        {
            playerStats.IncreaseMovementSpeedScalar(scalarIncreasePerAffix);
        }
    }
}
