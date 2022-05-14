using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public GameObject[] droppableItems;
    PlayerCurrencyManager playerCurrencyManager;
    public float openDistance;
    public int cost;
    public bool debug;

    Transform playerTransform;
    Animator animator;

    void Start() {
        animator = GetComponent<Animator>();
        GameObject player = GameObject.Find("Player");
        playerTransform = player.transform;
        playerCurrencyManager = GameObject.Find("Game Management").GetComponent<PlayerCurrencyManager>();
    }

    private void Update() {

        if (!PlayerInput.interact)
            return;

        if (Vector2.Distance(transform.position, playerTransform.position) > openDistance)
            return;

        if (playerCurrencyManager.RemoveCurrency(cost))
            Open();
    }

    public void Open() {
        animator.SetBool("isOpen", true);

        GameObject drop = droppableItems[Random.Range(0, droppableItems.Length)];
        Instantiate(drop, transform.position, Quaternion.identity);
    }
}

