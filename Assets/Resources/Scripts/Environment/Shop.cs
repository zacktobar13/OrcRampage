using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : BaseInteractible
{
    public delegate void OnPlayerShopOpen();
    public static event OnPlayerShopOpen onPlayerShopOpen;

    public delegate void OnPlayerShopClosed();
    public static event OnPlayerShopClosed onPlayerShopClosed;

    [HideInInspector] public bool shopOpen = false;

    public override void OnPlayerInteract()
    {
        // Opening shop.
        if (!shopOpen)
        {
            shopOpen = true;

            if (onPlayerShopOpen != null)
                onPlayerShopOpen();

            PlayerManagement.TogglePlayerControl(false);

            Debug.Log("Opened shop");

            return;
        }

        // Closing shop.
        if (shopOpen)
        {
            shopOpen = false;

            if (onPlayerShopClosed != null)
                onPlayerShopClosed();

            PlayerManagement.TogglePlayerControl(true);

            Debug.Log("Closed shop");

            return;
        }
    }


}
