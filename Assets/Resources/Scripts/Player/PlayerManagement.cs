﻿using System.Collections.Generic;
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

        PlayerExperience playerExperience = player.GetComponent<PlayerExperience>();
        if (playerExperience)
        {
            playerExperience.currentXp = 0;
            playerExperience.playerLevel = 1;
            playerExperience.xpToNextLevel = 100;
        }

        foreach (BaseAffix affix in player.GetComponentsInChildren<BaseAffix>())
        {
            Destroy(affix);
        }

        PlayerStats playerStats = player.GetComponent<PlayerStats>();
        playerStats.ResetStats();

        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        playerHealth.Respawn();

        TogglePlayerControl(true);
    }
}
