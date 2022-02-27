using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XPGlobe : DroppedItem
{
    // Start is called before the first frame update
    protected new void Start()
    {
        base.Start();
        Debug.Log(magnetism);
    }

    // Update is called once per frame
    protected override void OnTriggerStay2D(Collider2D collision)
    {
        base.OnTriggerStay2D(collision);
        // Add xp
    }

	
}
