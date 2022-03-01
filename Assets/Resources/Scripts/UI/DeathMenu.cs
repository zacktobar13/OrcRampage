using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class DeathMenu : MonoBehaviour
{
    public GameplayUI gameplayUI;
    TimeManager timeManager;

    public void ClickRetry()
    {
        StartCoroutine(ReloadLevel());
    }
    
    IEnumerator ReloadLevel()
    {
        gameplayUI.FadeToBlack();
        yield return new WaitForSeconds(2f);
        gameplayUI.HideDeathPanel();
        gameplayUI.ShowPlayerInfoPanel();
        PlayerManagement.RestorePlayer();
        gameplayUI.InitializePlayerInfoPanel();
        gameplayUI.SetRenderCamera(Camera.main);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name.ToString());
    }

    public void QuitToMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

}
