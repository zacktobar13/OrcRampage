using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceGainedAffix : BaseAffix
{
    public void Start()
    {
        playerStats.IncreaseExperienceGainedScalar(scalarIncreasePerAffix);
    }

    public override void AddAffixCount(int increment)
    {
        base.AddAffixCount(increment);
        for (int i = 0; i < increment; i++)
        {
            playerStats.IncreaseExperienceGainedScalar(scalarIncreasePerAffix);
        }
    }
}
