using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Chest : MonoBehaviour
{
    // Referenced in Start()
    Animator animator;
    GameObject player;
    Transform playerTransform;
    PlayerCurrencyManager playerCurrencyManager;

    public GameObject costInfoGameObject;
    public TextMeshProUGUI text;
    public TextMeshProUGUI shadowText;
    public GameObject[] droppableItems;
    public float openDistance;
    public int cost;
    public bool debug;
    bool isOpen;

    void Start() {
        animator = GetComponent<Animator>();
        player = PlayerManagement.player;
        playerTransform = player.transform;
        playerCurrencyManager = GameObject.Find("Game Management").GetComponent<PlayerCurrencyManager>();
        UpdateText(cost);
    }

    private void Update() {

        if (isOpen)
            return;

        costInfoGameObject.SetActive(IsPlayerInRange());

        if (!PlayerInput.interact)
            return;

        if (!IsPlayerInRange())
            return;


        if (playerCurrencyManager.RemoveCurrency(cost))
            Open();
    }

    public void Open() {
        isOpen = true;
        costInfoGameObject.SetActive(false);
        animator.SetBool("isOpen", true);

        GameObject drop = droppableItems[Random.Range(0, droppableItems.Length)];
        Instantiate(drop, transform.position, Quaternion.identity);
    }

    public bool IsPlayerInRange()
    {
        return Vector2.Distance(transform.position, playerTransform.position) <= openDistance;
    }

    public void UpdateText(int amount)
    {
        text.text = amount.ToString();
        shadowText.text = amount.ToString();
    }
}

