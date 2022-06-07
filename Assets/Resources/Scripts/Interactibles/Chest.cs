using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Chest : MonoBehaviour
{
    public Color cantBuyColor;
    public GameObject costInfoGameObject;
    public TextMeshProUGUI text;
    public TextMeshProUGUI shadowText;
    public AudioClip openAudio;
    public AudioClip lockedAudio;
    public GameObject[] droppableItems;
    public float openDistance;
    public int cost;
    public bool debug;

    // Initialized in Start()
    Animator animator;
    GameObject player;
    Transform playerTransform;
    PlayerCurrencyManager playerCurrencyManager;
    AudioSource audioSource;

    bool isOpen;

    void Start() {
        animator = GetComponent<Animator>();
        player = PlayerManagement.player;
        playerTransform = player.transform;
        playerCurrencyManager = GameObject.Find("Game Management").GetComponent<PlayerCurrencyManager>();
        audioSource = GetComponent<AudioSource>();
        UpdateText(cost);
    }

    private void Update() {

        if (isOpen)
            return;

        costInfoGameObject.SetActive(IsPlayerInRange());

        if (!IsPlayerInRange())
            return;

        text.color = playerCurrencyManager.CanAfford(cost) ? Color.white : cantBuyColor;

        if (!PlayerInput.interact)
            return;

        if (playerCurrencyManager.RemoveCurrency(cost))
            Open();
    }

    public void Open() {
        isOpen = true;
        costInfoGameObject.SetActive(false);
        animator.SetBool("isOpen", true);
        gameObject.tag = "Untagged";
        SoundManager.PlayOneShot(audioSource, openAudio, new SoundManagerArgs(.3f, true));
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

