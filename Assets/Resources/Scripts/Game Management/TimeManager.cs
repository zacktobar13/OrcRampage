using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{

    bool gamePaused = false;

    public void PauseGame(bool toggle)
    {
        gamePaused = toggle;
        Time.timeScale = toggle ? 0 : 1;
    }

    public void TogglePause()
    {
        PauseGame(!gamePaused);
    }

    public bool IsGamePaused()
    {
        return gamePaused;
    }
}
