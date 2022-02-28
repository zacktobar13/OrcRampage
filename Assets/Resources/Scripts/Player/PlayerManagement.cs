using System.Collections.Generic;
using UnityEngine;

public class PlayerManagement : MonoBehaviour
{
    public static GameObject player;
    public GameObject[] players;

    public static void SetPlayer(GameObject inc)
    {
        player = inc;
    }

    public static void TogglePlayerControl(bool toggle)
    {
        PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
        PlayerAttack playerShoot = player.GetComponentInChildren<PlayerAttack>();
        RotateWeapon[] rotateWeapon = player.GetComponentsInChildren<RotateWeapon>(true);

        if (playerMovement != null)
            playerMovement.enabled = toggle;
        if (playerShoot != null)
            playerShoot.enabled = toggle;
        if (rotateWeapon != null)
        {
            foreach (RotateWeapon weapon in rotateWeapon)
            {
                weapon.enabled = toggle;
            }
        }
    }

    public static void RestorePlayer()
    {
        player.GetComponent<PlayerHealth>().maxHealth = 100;
        player.GetComponent<PlayerHealth>().health = 100;
        player.GetComponent<PlayerHealth>().isCurrentlyDead = false;
        player.GetComponent<PlayerExperience>().currentXp = 0;
        player.GetComponent<PlayerExperience>().xpToNextLevel = 100;

        foreach (BaseAffix affix in player.GetComponentsInChildren<BaseAffix>())
        {
            Destroy(affix);
        }
        // Stat reset goes here

        TogglePlayerControl(true);
    }
}
