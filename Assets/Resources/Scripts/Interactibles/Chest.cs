using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public GameObject copperCoin;
    public GameObject silverCoin;
    public GameObject goldCoin;
    public float openDistance;
    public int keysRequired = 0;
    public bool debug;

    Transform playerTransform;
    Animator animator;

    void Start() {
        animator = GetComponent<Animator>();
        playerTransform = GameObject.Find("Player").GetComponent<Transform>();
    }

    private void Update() {
        if (PlayerInput.interact && Vector2.Distance(transform.position, playerTransform.position) <= openDistance) {
            AttemptOpen();
        }
    }

    public void AttemptOpen() {
        animator.SetBool("isOpen", true);

        for (int i = 0; i < 10; i++) {
            Instantiate(copperCoin, transform.position, Quaternion.identity);
        }

        for (int i = 0; i < 7; i++) {
            Instantiate(silverCoin, transform.position, Quaternion.identity);
        }

        for (int i = 0; i < 5; i++) {
            Instantiate(goldCoin, transform.position, Quaternion.identity);
        }
    }
}

