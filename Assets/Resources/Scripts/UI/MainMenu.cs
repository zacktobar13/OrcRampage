using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public delegate void OnGameStart();
    public static event OnGameStart onGameStart;

    public void StartGame()
    {
        if (onGameStart != null)
            onGameStart();

        SceneManager.LoadScene("Village");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
