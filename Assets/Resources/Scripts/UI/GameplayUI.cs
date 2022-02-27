using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameplayUI : MonoBehaviour
{
    public Image playerHealthBar;
    public Image playerExperienceBar;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI currencyInfo;
    public TextMeshProUGUI playerLevelInfo;
    public TextMeshProUGUI xpInfo;
    public Animator currencyAnim;

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
        SceneManager.sceneLoaded += FadeFromBlack;
        DontDestroyOnLoad(gameObject);
    }

    public void UpdatePlayerHealth(int currentHealth, int maxHealth)
    {
        healthText.text = currentHealth + " / " + maxHealth;
        playerHealthBar.fillAmount = (float) currentHealth / maxHealth;
    }

    public void UpdatePlayerLevel(int currentLevel)
    {
        playerLevelInfo.text = "LVL " + currentLevel.ToString();
    }

    public void UpdatePlayerExperienceBar(int currentXP, int xpToNextLevel)
    {
        xpInfo.text = currentXP + " / " + xpToNextLevel;
        playerExperienceBar.fillAmount = (float) currentXP / xpToNextLevel;
    }

    public void UpdateCurrencyInfo(int newCurrencyAmount)
    {
        currencyAnim.SetTrigger("AddCurrency");
        currencyInfo.text = "$ " + newCurrencyAmount.ToString();
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
}
