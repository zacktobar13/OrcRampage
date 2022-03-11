using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct HealInfo
{
    public int healAmount;
    public AudioClip healSound;

    public HealInfo(int healAmount, AudioClip healSound)
    {
        this.healAmount = healAmount;
        this.healSound = healSound;
    }
}
