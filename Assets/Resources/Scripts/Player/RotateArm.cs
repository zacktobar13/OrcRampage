﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PowerTools;

public class RotateArm : MonoBehaviour
{
    public static float currentArmRotation;
    public static Vector2 currentArmLocation;
    public Transform leftSideWeaponTransform;
    public Transform rightSideWeaponTransform;
    bool hasBeenFlipped = false;
    
    Transform playerPosition;
    Vector3 mousePosition;

    private void OnEnable ()
    {
        //playerPosition = PlayerManagement.GetNearestPlayer ( transform.position ).transform;
        playerPosition = transform.parent;
        // Force pump the update to make sure that the weapon is rotated correctly on the frame it becomes visible
        Update();
    }

    public void Update()
    {
        mousePosition = PlayerInput.mousePosition;

        if (mousePosition.x - playerPosition.position.x < 0)
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
        currentArmRotation = GetArmRotationBounded (rotationAmount);
        transform.rotation = Quaternion.Euler(0f, 0f, currentArmRotation);

        float playerRotation = Utility.RotationAmount ( playerPosition.position, mousePosition );

        if ( ( Utility.InSecondQuadrant( playerRotation ) || Utility.InThirdQuadrant ( playerRotation ) ) && !hasBeenFlipped )
        {
            Vector3 scale = transform.localScale;
            scale.y *= -1;
            transform.localScale = scale;
            hasBeenFlipped = true;
        }
        else if ( ( ( Utility.InFirstQuadrant ( playerRotation ) ) || ( Utility.InFourthQuadrant ( playerRotation ) ) ) && hasBeenFlipped )
        {
            hasBeenFlipped = false;
            Vector3 scale = transform.localScale;
            scale.y *= -1;
            transform.localScale = scale;
        }
    }

    // 105, -112
    float GetArmRotationBounded (float rotationAmount)
    {
        // Gun is in left hand
        if ( transform.position == leftSideWeaponTransform.position )
        {
            if ( rotationAmount < 65 && rotationAmount > 0 )
            {
                rotationAmount = 65;
            }
            else if ( rotationAmount > -55 && rotationAmount < 0)
            {
                rotationAmount = -55;
            }
        }
        else
        {
            if (rotationAmount > 105)
            {
                rotationAmount = 105;
            }
            else if (rotationAmount < -112)
            {
                rotationAmount = -112;
            }
        }

        return rotationAmount;
    }

}
