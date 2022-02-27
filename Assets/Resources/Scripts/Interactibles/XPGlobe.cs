using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XPGlobe : DroppedItem
{
    public int xpValue;
    PlayerExperience playerExperience;
    protected new void Start()
    {
        playerExperience = PlayerManagement.player.GetComponent<PlayerExperience>();
        base.Start();
    }

    // Update is called once per frame
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        playerExperience.AddXp(xpValue);
        base.OnTriggerEnter2D(collision);
    }

}
