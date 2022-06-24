using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuGameObject;
    public GameplayUI gameplayUI;
    public TimeManager timeManager;

    void Update()
    {
        if (PlayerInput.pressedPause)
        {
            if (timeManager.IsGamePaused())
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        pauseMenuGameObject.SetActive(true);
        timeManager.PauseGame(true);
    }

    public void ResumeGame()
    {
        pauseMenuGameObject.SetActive(false);
        timeManager.PauseGame(false);
    }

    public void Restart()
    {
        ResumeGame();
        PlayerManagement.RestorePlayer();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name.ToString());
    }

    public void GoToMainMenu()
    {
        ResumeGame();
        Destroy(PlayerManagement.player);
        SceneManager.LoadScene("Main Menu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
