using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuGameObject;

    bool gamePaused = false;

    // Update is called once per frame
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
        SceneManager.LoadScene("Village");
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
