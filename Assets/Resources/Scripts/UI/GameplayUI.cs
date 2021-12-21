using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameplayUI : MonoBehaviour
{
    WaveManager waveManager;
    public TextMeshProUGUI waveNumberText;
    public TextMeshProUGUI enemiesRemainingText;
    public TextMeshProUGUI waveCompletedText;
    public TextMeshProUGUI waveCountdownText;
    public TextMeshProUGUI ammoText;

    public GameObject[] waveInfoGroup;
    public GameObject[] experienceInfoGroup;
    public GameObject[] weaponInfoGroup;

    public Image blackFade;
    bool fadingFromBlack = true;
    bool fadingToBlack = false;
    float blackFadeAlpha = 1f;

    public delegate void OnScreenBlack();
    public static event OnScreenBlack onScreenBlack;

    public delegate void OnFadeCompleted();
    public static event OnFadeCompleted onFadeCompleted;

    private void Start()
    {
        waveManager = GameObject.Find("Game Management").GetComponent<WaveManager>();
        WaveManager.onNewWave += UpdateWaveNumber;
        WaveManager.onEnemyDied += UpdateEnemiesRemaining;
        WaveManager.onWaveCompleted += ToggleWaveCompletedText;
        WaveManager.onWaveCompleted += ToggleWaveInfoText;
        WaveManager.onWaveCompleted += StartWaveCountdownText;
        waveNumberText = transform.Find("Wave Number").GetComponent<TextMeshProUGUI>();
        enemiesRemainingText = transform.Find("Enemies Remaining").GetComponent<TextMeshProUGUI>();
        waveCompletedText = transform.Find("Wave Complete Text").GetComponent<TextMeshProUGUI>();
        waveCountdownText = transform.Find("Wave Countdown Text").GetComponent<TextMeshProUGUI>();
        SceneManager.sceneLoaded += FadeFromBlack;
        DontDestroyOnLoad(gameObject);
    }

    private void FixedUpdate()
    {
        if (fadingFromBlack)
        {
            blackFadeAlpha -= .005f;
            var tempColor = blackFade.color;
            tempColor.a = blackFadeAlpha;
            blackFade.color = tempColor;

            if (blackFadeAlpha <= 0f)
            {
                fadingFromBlack = false;
                blackFade.enabled = false;

                if (onFadeCompleted != null)
                {
                    onFadeCompleted();
                }
            }
        }

        if (fadingToBlack)
        {
            blackFadeAlpha += .005f;
            var tempColor = blackFade.color;
            tempColor.a = blackFadeAlpha;
            blackFade.color = tempColor;

            if (blackFadeAlpha >= 1f)
            {
                fadingToBlack = false;
                if (onScreenBlack != null)
                {
                    onScreenBlack();
                }
            }
        }
    }

    public void UpdateWaveNumber(int number)
    {
        waveNumberText.SetText("Wave Number: " + number);
    }

    public void UpdateEnemiesRemaining(int number)
    {
        enemiesRemainingText.SetText("Enemies Remaining: " + number);
    }

    public void ToggleWaveCompletedText(bool completed)
    {
        waveCompletedText.enabled = (completed && waveManager.waveNumber > 0);
        waveCountdownText.enabled = completed;
    }

    public void ToggleWaveInfoText(bool completed)
    {
        waveNumberText.enabled = !completed;
        enemiesRemainingText.enabled = !completed;
    }

    public void StartWaveCountdownText(bool completed)
    {
        if (completed)
            StartCoroutine(UpdateWaveCountdownText());
    }

    IEnumerator UpdateWaveCountdownText()
    {
        while (waveManager.nextWaveTime >= Time.time)
        {
            waveCountdownText.SetText("Wave " + (waveManager.waveNumber + 1) + " starts in " + (int)waveManager.waveTimer + " seconds");
            yield return new WaitForEndOfFrame();
        }
    }

    // Toggles wave info group UI
    public void ToggleWaveInfoGroup(bool toggle)
    {
        foreach (GameObject member in waveInfoGroup)
        {
            member.SetActive(toggle);
        }
    }

    // Toggles wave info group UI
    public void ToggleExperienceInfoGroup(bool toggle)
    {
        foreach (GameObject member in experienceInfoGroup)
        {
            member.SetActive(toggle);
        }
    }

    // Toggles weapon info group UI
    public void ToggleWeaponInfoGroup(bool toggle)
    {
        foreach (GameObject member in weaponInfoGroup)
        {
            member.SetActive(toggle);
        }
    }

    public void FadeFromBlack(Scene scene, LoadSceneMode sceneLoadMode)
    {
        blackFade.enabled = true;
        fadingFromBlack = true;
    }

    public void FadeToBlack()
    { 
        blackFade.enabled = true;
        fadingToBlack = true;
    }

    private void OnDestroy()
    {
        //PlayerShootOld.onShoot -= OnShoot;
        WaveManager.onNewWave -= UpdateWaveNumber;
        WaveManager.onEnemyDied -= UpdateEnemiesRemaining;
        WaveManager.onWaveCompleted -= ToggleWaveCompletedText;
        WaveManager.onWaveCompleted -= ToggleWaveInfoText;
        WaveManager.onWaveCompleted -= StartWaveCountdownText;
        //PlayerShootOld.onWeaponChange -= OnWeaponChange;
        //PlayerShootOld.onCriticalReload -= OnReload;
        //PlayerShootOld.onFailedReload -= OnReload;
    }
}
