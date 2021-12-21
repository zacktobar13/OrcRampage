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
        PlayerShoot playerShoot = player.GetComponentInChildren<PlayerShoot>();
        RotateArm[] rotateArm = player.GetComponentsInChildren<RotateArm>();

        if (playerMovement != null)
            playerMovement.enabled = toggle;
        if (playerShoot != null)
            playerShoot.enabled = toggle;
        if (rotateArm != null)
        {
            foreach (RotateArm arm in rotateArm)
                arm.enabled = toggle;
        }
    }
}
