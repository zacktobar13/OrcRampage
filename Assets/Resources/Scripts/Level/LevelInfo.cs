using UnityEngine;

/// <summary>
/// This is used as a container to hold important data about a level. 
/// </summary>

using UnityEngine.SceneManagement;

public class LevelInfo : MonoBehaviour
{
    public static int levelsCompleted;
    public static int levelsUntilBoss = 3;
    public string[] possibleLevels;
    public string[] possibleBossLevels;

    public GameObject[] enemies;

    public delegate void OnEnterHubWorld();
    public static event OnEnterHubWorld onEnterHubWorld;

    public delegate void OnNewLevel();
    public static event OnNewLevel onNewLevel;

    public void ChooseNextLevel()
    {
        int randomRoll = (int)Random.Range(0, possibleLevels.Length);

        if (levelsCompleted < levelsUntilBoss)
        {
            SceneManager.LoadScene(possibleLevels[randomRoll]);
        }
        else
        {
            SceneManager.LoadScene(possibleBossLevels[randomRoll]);
        }

        if (onNewLevel != null)
            onNewLevel();

        PlayerManagement.TogglePlayerControl(true);
    }

    public void LoadHubWorld()
    {
        if (onEnterHubWorld != null)
            onEnterHubWorld();

        SceneManager.LoadScene("Hub World");
        PlayerManagement.player.GetComponent<PlayerHealth>().Respawn();
        PlayerManagement.TogglePlayerControl(false);
    }
}
