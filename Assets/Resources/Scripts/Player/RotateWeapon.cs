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
    
    Transform parentPosition;
    Vector3 mousePosition;

    private void OnEnable ()
    {
        //playerPosition = PlayerManagement.GetNearestPlayer ( transform.position ).transform;
        parentPosition = transform.parent;
        leftSideWeaponTransform = transform.parent.Find("Left Hand");
        rightSideWeaponTransform = transform.parent.Find("Right Hand");
        // Force pump the update to make sure that the weapon is rotated correctly on the frame it becomes visible
        Update();
    }

    public void Update()
    {
        mousePosition = PlayerInput.mousePosition;

        if (mousePosition.x - parentPosition.position.x < 0)
        {
            transform.position = leftSideWeaponTransform.position;
        }
        else
        {
            transform.position = rightSideWeaponTransform.position;
        }

        currentArmLocation = transform.position;
        float rotationAmount = Utility.RotationAmount(transform.position, mousePosition);
        transform.rotation = Quaternion.Euler(0f, 0f, rotationAmount);

        float playerRotation = Utility.RotationAmount(parentPosition.position, mousePosition);

        Vector3 scale = transform.localScale;
        bool shouldBeFlipped = Utility.InSecondQuadrant(playerRotation) || Utility.InThirdQuadrant(playerRotation);
        scale.y = shouldBeFlipped ? -1 : 1;
        transform.localScale = scale;
    }
}
