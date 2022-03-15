using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class GameplayUI : MonoBehaviour
{
    Canvas canvas;
    public AudioSource audioSource;
    public AudioClip collectExperienceAudio;
    public Image playerHealthBar;
    public Image playerExperienceBar;
    public Image playerBossBar;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI currencyInfo;
    public TextMeshProUGUI playerLevelInfo;
    public TextMeshProUGUI xpInfo;
    public TextMeshProUGUI killCountText;
    public Animator currencyAnim;
    public GameObject affixPanel;
    public GameObject playerInfoPanel;
    public GameObject deathPanel;
    public GameObject playerAffixDisplay;
    public GameObject affixDisplayObject;

    public Shader affixShader;

    public GameObject[] weaponInfoGroup;
    public Transform affixIconDisplay;

    public Image blackFade;
    EnemySpawner enemySpawner;

    TimeManager timeManager;
    bool fadingFromBlack = true;
    bool fadingToBlack = false;
    float blackFadeAlpha = 1f;
    int killCount;

    public delegate void OnScreenBlack();
    public static event OnScreenBlack onScreenBlack;

    public delegate void OnFadeCompleted();
    public static event OnFadeCompleted onFadeCompleted;

    int enemiesToSpawnBoss = 200; // BIG TIME TEMP

    private void Start()
    {

        Initialize();
        SceneManager.sceneLoaded += FadeFromBlack;
        SceneManager.sceneLoaded += LoadNewLevel;
        enemySpawner.onEnemyDeath += IncrementKillCounter;
        enemySpawner.onEnemyDeath += UpdateBossBar;
        SceneManager.sceneLoaded += ClearAffixIcons;
    }

	private void LoadNewLevel(Scene scene, LoadSceneMode sceneLoadMode)
    {
        Initialize();
    }

    private void Initialize()
    {
        canvas = GetComponent<Canvas>();
        SetRenderCamera(Camera.main);
        enemySpawner = GameObject.Find("Game Management").GetComponent<EnemySpawner>();
        timeManager = GameObject.Find("Game Management").GetComponent<TimeManager>();
        audioSource = GetComponent<AudioSource>();
        ResetKillCounter();
        ResetBossBar();
        SceneManager.sceneLoaded += FadeFromBlack;
        SceneManager.sceneLoaded += LoadNewLevel;
        SceneManager.sceneLoaded += ClearAffixIcons;
    }

    public void SetRenderCamera(Camera camera)
    {
        canvas.worldCamera = camera;
    }

    // AFFIX PANEL //
    public void UpdatePlayerAffixDisplay(BaseAffix newAffix)
    {
        Transform existingAffixIcon = affixIconDisplay.Find(newAffix.affixName);
        if (existingAffixIcon != null)
        {
            String newQuantity = (newAffix.affixCount + 1).ToString();
            existingAffixIcon.GetComponentInChildren<TextMeshProUGUI>().text = newQuantity;
        }
        else
        {
            Sprite affixIcon = newAffix.affixIcon;
            GameObject affixIconDisplay = Instantiate(affixDisplayObject, playerAffixDisplay.transform);
            Image image = affixIconDisplay.GetComponent<Image>();
            affixIconDisplay.name = newAffix.affixName;
            image.sprite = affixIcon;

            // Outline logic
            Material shaderMaterial = new Material(affixShader);
            shaderMaterial.EnableKeyword("OUTBASE_ON");
            shaderMaterial.EnableKeyword("OUTBASEPIXELPERF_ON");
            if (newAffix.rarity != Rarity.COMMON)
                shaderMaterial.SetFloat("_OutlineAlpha", 1f);
            shaderMaterial.SetColor("_OutlineColor", RarityUtil.GetRarityColor(newAffix.rarity));
            image.material = shaderMaterial;
        }
    }

    public void ClearAffixIcons(Scene scene, LoadSceneMode sceneLoadMode)
    {
        for (int i = 0; i < affixIconDisplay.childCount; i++)
        {
            Transform child = affixIconDisplay.GetChild(i);
            Destroy(child.gameObject);
        }
    }

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

    // KILL COUNTER, BOSS BAR ---------------------------------------------------
    public void IncrementKillCounter(BaseEnemy enemy)
    {
        killCount += 1;
        killCountText.text = killCount.ToString();
    }

    public void ResetKillCounter()
    {
        killCount = 0;
        killCountText.text = killCount.ToString();
    }

	public void UpdateBossBar(BaseEnemy enemy)
	{
        playerBossBar.fillAmount = (float) killCount / enemiesToSpawnBoss;
	}

    public void ResetBossBar()
    {
        playerBossBar.fillAmount = 0f;
    }


    //----------------------------------------------------------------
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
