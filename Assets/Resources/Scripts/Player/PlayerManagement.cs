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
        RotateWeapon[] rotateArm = player.GetComponentsInChildren<RotateWeapon>();

        if (playerMovement != null)
            playerMovement.enabled = toggle;
        if (playerShoot != null)
            playerShoot.enabled = toggle;
        if (rotateArm != null)
        {
            foreach (RotateWeapon arm in rotateArm)
                arm.enabled = toggle;
        }
    }
}
