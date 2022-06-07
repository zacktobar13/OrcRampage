using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionHelperArrow : MonoBehaviour
{
    public string tagToFind;
    float distanceFromPlayer = 5f;
    float yOffset = 2.5f;
    Transform playerTransform;
    SpriteRenderer spriteRenderer;

    private void Start()
    {
        playerTransform = transform.parent;
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        Vector2 directionToChest = GetDirectionToNearestObjectWithTag();
        if (directionToChest.magnitude > 2) // hacky way to check if we found a valid object
        {
            spriteRenderer.enabled = false;
            return;
        }

        spriteRenderer.enabled = true;
        directionToChest.Normalize();
        Vector3 playerPosition = playerTransform.position;

        Vector2 arrowPosition = (Vector2)playerPosition + (directionToChest * distanceFromPlayer);
        arrowPosition.y += yOffset;

        float rotationAmount = Mathf.Atan2(directionToChest.y, directionToChest.x) * Mathf.Rad2Deg;
        Quaternion arrowRotation = Quaternion.Euler(0, 0, rotationAmount);
        gameObject.transform.position = arrowPosition;
        gameObject.transform.rotation = arrowRotation;
    }

    Vector2 GetDirectionToNearestObjectWithTag()
    {
        GameObject nearestChest = FindNearestObjectOfTag();
        if (nearestChest == null)
            return Vector2.positiveInfinity;

        return ((Vector2)(nearestChest.transform.position - playerTransform.position)).normalized;
    }

    GameObject FindNearestObjectOfTag()
    {
        GameObject[] allChests = GameObject.FindGameObjectsWithTag(tagToFind);
        float nearestChestDistance = float.PositiveInfinity;
        GameObject nearestChest = null;
        foreach (GameObject chest in allChests)
        {
            float currentDistance = Vector2.Distance(chest.transform.position, playerTransform.position);
            if (currentDistance < nearestChestDistance)
            {
                nearestChestDistance = currentDistance;
                nearestChest = chest;
            }
        }
        return nearestChest;
    }
}
