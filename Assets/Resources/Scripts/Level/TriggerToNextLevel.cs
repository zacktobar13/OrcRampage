using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerToNextLevel : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    LevelInfo levelInfo;
    BoxCollider2D boxCollider;
    public GameObject bossSkull;
    GameplayUI gameplayUI;
    PlayerHealth playerHealth;

    public delegate void OnCollideWithPlayer();
    public static event OnCollideWithPlayer onCollideWithPlayer;

    private void Start()
    {
        playerHealth = PlayerManagement.player.GetComponent<PlayerHealth>();
        gameplayUI = GameObject.Find("Gameplay UI").GetComponent<GameplayUI>();
        levelInfo = GameObject.Find("Level Info").GetComponent<LevelInfo>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
        spriteRenderer.enabled = false;
        boxCollider.enabled = false;
        WaveManager.onExitSpawned += EnableTrigger;
        WaveManager.onExitSpawned += ShowPortalText;
        WaveManager.onBossExitSpawned += EnableTrigger;
        WaveManager.onBossExitSpawned += ShowBossPortalText;
        PlayerHealth.onDeath += DestroySelf;

        GameplayUI.onScreenBlack += LoadNextLevel;

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if ( other.tag == "Player" )
        {
            if (onCollideWithPlayer != null)
                onCollideWithPlayer();

            PlayerManagement.TogglePlayerControl(false);
            gameplayUI.FadeToBlack();
        }
    }

    private void EnableTrigger()
    {
        spriteRenderer.enabled = true;
        boxCollider.enabled = true;
    }

    private void ShowPortalText()
    {
        Debug.Log("A portal to the next level has appeared!");
    }

    private void ShowBossPortalText()
    {
        Debug.Log("A portal to the boss has appeared!");
        bossSkull.SetActive(true);
    }

    public void LoadNextLevel()
    {
        // Make sure we only have Level Info choose the next level if we aren't currently dead.
        if (!playerHealth.isCurrentlyDead)
        {
            levelInfo.ChooseNextLevel();
        }
    }

    private void OnDestroy()
    {
        WaveManager.onExitSpawned -= EnableTrigger;
        WaveManager.onExitSpawned -= ShowPortalText;
        WaveManager.onBossExitSpawned -= EnableTrigger;
        WaveManager.onBossExitSpawned -= ShowBossPortalText;
        PlayerHealth.onDeath -= DestroySelf;
        GameplayUI.onScreenBlack -= LoadNextLevel;
    }

    private void DestroySelf(PlayerHealth playerHealth)
    {
        Destroy(gameObject);
    }
}
