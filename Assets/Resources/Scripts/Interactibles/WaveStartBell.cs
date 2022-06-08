using System.Collections;
using UnityEngine;

public class WaveStartBell : MonoBehaviour
{
    public float interactRange;
    public float interactCooldown;
    public GameObject text;

    // Referenced in Start()
    EnemySpawner enemySpawner;
    GameObject player;

    float distanceToPlayer;
    bool canInteract = true;

    void Start()
    {
        enemySpawner = GameObject.Find("Game Management").GetComponent<EnemySpawner>();
        player = PlayerManagement.player;
    }

    // Update is called once per frame
    void Update()
    {
        distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

        if (distanceToPlayer <= interactRange && canInteract)
        {
            text.SetActive(true);

            if (PlayerInput.interact)
            {
                enemySpawner.BeginWave();
                StartCoroutine(WaitThenReEnable());
            }
        }
        else
        {
            text.SetActive(false);
        }
    }

    IEnumerator WaitThenReEnable()
    {
        canInteract = false;
        yield return new WaitForSeconds(interactCooldown);
        canInteract = true;
    }
}
