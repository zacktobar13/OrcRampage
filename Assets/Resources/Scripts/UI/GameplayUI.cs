using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameplayUI : MonoBehaviour
{
    Canvas canvas;
    public AudioSource audioSource;
    public AudioClip collectExperienceAudio;
    public Image playerHealthBar;
    public Image playerExperienceBar;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI currencyInfo;
    public TextMeshProUGUI playerLevelInfo;
    public TextMeshProUGUI xpInfo;
    public Animator currencyAnim;
    public GameObject affixPanel;
    public GameObject playerInfoPanel;
    public GameObject deathPanel;
    public GameObject playerAffixDisplay;
    public GameObject affixDisplayObject;

    public GameObject[] weaponInfoGroup;

    public Image blackFade;

    TimeManager timeManager;
    bool fadingFromBlack = true;
    bool fadingToBlack = false;
    float blackFadeAlpha = 1f;

    public delegate void OnScreenBlack();
    public static event OnScreenBlack onScreenBlack;

    public delegate void OnFadeCompleted();
    public static event OnFadeCompleted onFadeCompleted;

    private void Start()
    {
        Initialize();
        SceneManager.sceneLoaded += FadeFromBlack;
        SceneManager.sceneLoaded += LoadNewLevel;
    }

	private void LoadNewLevel(Scene scene, LoadSceneMode sceneLoadMode)
    {
        Initialize();
    }

    private void Initialize()
    {
        canvas = GetComponent<Canvas>();
        SetRenderCamera(Camera.main);
        timeManager = GameObject.Find("Game Management").GetComponent<TimeManager>();
        audioSource = GetComponent<AudioSource>();
        SceneManager.sceneLoaded += FadeFromBlack;
        SceneManager.sceneLoaded += LoadNewLevel;
    }

    public void SetRenderCamera(Camera camera)
    {
        canvas.worldCamera = camera;
    }

    public void UpdatePlayerAffixDisplay(BaseAffix newAffix)
    {
        Sprite affixIcon = newAffix.affixIcon;
        GameObject affixIconDisplay = Instantiate(affixDisplayObject, playerAffixDisplay.transform);
        Image spriteRenderer = affixIconDisplay.GetComponent<Image>();
        spriteRenderer.sprite = affixIcon;
    }

    // AFFIX PANEL //
    public void ShowAffixPanel(int numberToChoose)
    {
        affixPanel.GetComponent<PickAffixMenu>().SetQuantityToChoose(numberToChoose);
        StartCoroutine(ShowAffixPanelAfterSeconds(.2f));
    }
    public void HideAffixPanel()
    {
        timeManager.PauseGame(false);
        affixPanel.SetActive(false);
    }

    IEnumerator ShowAffixPanelAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        HidePlayerInfoPanel();
        timeManager.PauseGame(true);
        affixPanel.SetActive(true);
    }
    // -------------------------------------------------------


    // PLAYER INFO PANEL //
    public void ShowPlayerInfoPanel()
    {
        playerInfoPanel.SetActive(true);
    }

    public void HidePlayerInfoPanel()
    {
        playerInfoPanel.SetActive(false);
    }

    public void InitializePlayerInfoPanel()
    {
        int health = PlayerManagement.player.GetComponent<PlayerHealth>().health;
        int experience = PlayerManagement.player.GetComponent<PlayerExperience>().xpToNextLevel;
        UpdatePlayerHealth(health, health);
        UpdatePlayerLevel(1);
        UpdatePlayerExperienceBar(0, experience);
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
        PlayCollectExperienceSound();
        xpInfo.text = currentXP + " / " + xpToNextLevel;
        playerExperienceBar.fillAmount = (float) currentXP / xpToNextLevel;
    }

    public void UpdateCurrencyInfo(int newCurrencyAmount)
    {
        currencyAnim.SetTrigger("AddCurrency");
        currencyInfo.text = "$ " + newCurrencyAmount.ToString();
    }
    // ----------------------------------------------------

    //  DEATH PANEL //
    public void ShowDeathPanel()
    {
        deathPanel.SetActive(true);
    }

    public void HideDeathPanel()
    {
        deathPanel.SetActive(false);
    }

    IEnumerator ShowDeathPanelAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        HidePlayerInfoPanel();
        ShowDeathPanel();
    }
    // ------------------------------------------------------

    private void PlayCollectExperienceSound()
    {
        SoundManagerArgs soundManagerArgs = new SoundManagerArgs(.4f, new Vector2(.35f, .45f));
        SoundManager.PlayOneShot(audioSource, collectExperienceAudio, soundManagerArgs);
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
