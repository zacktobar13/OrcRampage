using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class DeathMenu : MonoBehaviour
{
    public TextMeshProUGUI menuTitle;
    public GameObject respawnButton;
    public GameObject mainMenuButton;
    public GameObject quitButton;
    GameplayUI gameplayUI;

    private void Start()
    {
        PlayerHealth.onDeath += ToggleDeathMenu;
        PlayerHealth.onRespawn += ToggleDeathMenu;

        gameplayUI = GameObject.Find("Gameplay UI").GetComponent<GameplayUI>();
        GameplayUI.onScreenBlack += RespawnAtHub;
    }

    public void ClickRespawnAtHub()
    {
        gameplayUI.FadeToBlack();
    }

    public void QuitToMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ToggleDeathMenu(PlayerHealth playerHealth)
    {
        menuTitle.enabled = playerHealth.isCurrentlyDead;
        respawnButton.SetActive(playerHealth.isCurrentlyDead);
        mainMenuButton.SetActive(playerHealth.isCurrentlyDead);
        quitButton.SetActive(playerHealth.isCurrentlyDead);
    }

    private void OnDestroy()
    {
        PlayerHealth.onDeath -= ToggleDeathMenu;
        PlayerHealth.onRespawn -= ToggleDeathMenu;
        GameplayUI.onScreenBlack -= RespawnAtHub;
    }

    private void RespawnAtHub()
    {
        if (PlayerManagement.player.GetComponent<PlayerHealth>().isCurrentlyDead)
        {
            GameObject.Find("Level Info").GetComponent<LevelInfo>().LoadHubWorld();
        }
    }

}
