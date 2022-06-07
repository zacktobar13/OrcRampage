using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public bool gamePaused = false;

    float roundStartTime = 0f;

    private void Awake()
    {
        SetMyReferrenceForPlayer();
    }

    public void SetMyReferrenceForPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.SendMessage("SetTimeManager", this);
    }

    public void RestartRoundTimer()
    {
        roundStartTime = Time.time;
    }

    public float GetTimeInRound()
    {
        return Time.time - roundStartTime;
    }

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
