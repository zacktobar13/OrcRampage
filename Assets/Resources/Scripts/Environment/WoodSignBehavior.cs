using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class WoodSignBehavior : BaseInteractible
{
    LevelInfo levelInfo;
    GameplayUI gameplayUI;

    public delegate void OnPlayerInteracted();
    public static event OnPlayerInteracted onPlayerInteracted;

    void Start()
    {
        levelInfo = GameObject.Find("Level Info").GetComponent<LevelInfo>();
        gameplayUI = GameObject.Find("Gameplay UI").GetComponent<GameplayUI>();

        GameplayUI.onScreenBlack += StartNewRun;
    }

    public override void OnPlayerInteract()
    {
        if (onPlayerInteracted != null)
            onPlayerInteracted();

        if (levelInfo != null)
        {
            PlayerManagement.TogglePlayerControl(false);
            gameplayUI.FadeToBlack();
        }
        else
        {
            Debug.LogError("Level change failed because reference to Level Info on " + gameObject.name + " is null.");
        }
    }

    public void StartNewRun()
    {
        levelInfo.ChooseNextLevel();
    }

    private void OnDestroy()
    {
        GameplayUI.onScreenBlack -= StartNewRun;
    }
}
