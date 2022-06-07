using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public bool gamePaused = false;

    float roundStartTime = 0f;

    private void Awake()
    {
        SetMyReferrencesForPlayer();
    }

    public void SetMyReferrencesForPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        PlayerMovement movement = player.GetComponent<PlayerMovement>();
        GameObject weaponGameObject = player.transform.Find("Staff").gameObject;
        Weapon weapon = weaponGameObject.GetComponent<Weapon>();
        RotateWeapon rotate = weaponGameObject.GetComponent<RotateWeapon>();
        PlayerAnimation animation = player.GetComponent<PlayerAnimation>();
        weapon.SetTimeManager(this);
        movement.SetTimeManager(this);
        rotate.SetTimeManager(this);
        animation.SetTimeManager(this);
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
