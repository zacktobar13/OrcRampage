using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PowerTools;

public class RotateWeapon : MonoBehaviour
{
    public static float currentArmRotation;
    public static Vector2 currentArmLocation;
    public Transform leftSideWeaponTransform;
    public Transform rightSideWeaponTransform;
    bool isOnPlayer;
    
    Transform parentPosition;
    Transform playerTransform;
    Vector3 targetPosition;

    TimeManager timeManager;

    private void OnEnable ()
    {
        parentPosition = transform.parent;
        leftSideWeaponTransform = transform.parent.Find("Left Hand");
        rightSideWeaponTransform = transform.parent.Find("Right Hand");
        isOnPlayer = transform.parent.CompareTag("Player");
        if (!isOnPlayer)
            playerTransform = GameObject.FindWithTag("Player").transform;

        timeManager = GameObject.Find("Game Management").GetComponent<TimeManager>();

        // Force pump the update to make sure that the weapon is rotated correctly on the frame it becomes visible
        Update();
    }

    public void Update()
    {
        if (timeManager.IsGamePaused())
            return;

        targetPosition = isOnPlayer ? PlayerInput.mousePosition : (Vector2)playerTransform.position;

        if (targetPosition.x - parentPosition.position.x < 0)
        {
            transform.position = leftSideWeaponTransform.position;
        }
        else
        {
            transform.position = rightSideWeaponTransform.position;
        }

        currentArmLocation = transform.position;
        float rotationAmount = Utility.RotationAmount(transform.position, targetPosition);
        transform.rotation = Quaternion.Euler(0f, 0f, rotationAmount);

        float playerRotation = Utility.RotationAmount(parentPosition.position, targetPosition);

        Vector3 scale = transform.localScale;
        bool shouldBeFlipped = Utility.InSecondQuadrant(playerRotation) || Utility.InThirdQuadrant(playerRotation);
        scale.y = shouldBeFlipped ? -1 : 1;
        transform.localScale = scale;
    }
}
