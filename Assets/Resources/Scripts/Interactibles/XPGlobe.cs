using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XPGlobe : DroppedItem
{
    public int xpValue;
    PlayerExperience playerExperience;
    protected new void OnEnable()
    {
        playerExperience = PlayerManagement.player.GetComponent<PlayerExperience>();
        base.OnEnable();
    }

    // Update is called once per frame
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        playerExperience.AddXp(xpValue);
        base.OnTriggerEnter2D(collision);
    }

}
