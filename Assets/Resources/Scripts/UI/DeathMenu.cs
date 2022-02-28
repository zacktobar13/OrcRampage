using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class DeathMenu : MonoBehaviour
{
    public GameplayUI gameplayUI;

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
        SceneManager.LoadScene(SceneManager.GetActiveScene().name.ToString());
        gameplayUI.SetRenderCamera(Camera.main);
    }

    public void QuitToMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

}
