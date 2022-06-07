using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuGameObject;
    public GameplayUI gameplayUI;

    bool gamePaused = false;

    void Update()
    {
        if (PlayerInput.pressedPause)
        {
            if (gamePaused)
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
        gamePaused = true;
        Time.timeScale = 0f;
        pauseMenuGameObject.SetActive(true);
    }

    public void ResumeGame()
    {
        gamePaused = false;
        Time.timeScale = 1f;
        pauseMenuGameObject.SetActive(false);
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
        SceneManager.LoadScene("Main Menu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
