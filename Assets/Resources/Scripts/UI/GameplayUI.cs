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
    public Image playerBossBar;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI localCurrencyInfo;
    public TextMeshProUGUI globalCurrencyInfo;
    public TextMeshProUGUI killCountText;
    public Animator currencyAnim;
    public Animator globalCurrencyAnim;
    public GameObject affixPanel;
    public GameObject playerInfoPanel;
    public GameObject deathPanel;
    public GameObject playerAffixDisplay;
    public GameObject affixDisplayObject;

    public GameObject pickedUpAffixPanel;
    public Image pickedUpAffixIconImage;
    public TextMeshProUGUI pickedUpAffixTitleText;
    public TextMeshProUGUI pickedUpAffixDescriptionText;
    public float pickedUpAffixWindowDuration;

    public TextMeshProUGUI currentWaveText;

    public GameObject waveInfoGameObject;
    public TextMeshProUGUI waveInfoText;

    public Shader affixShader;

    public GameObject[] weaponInfoGroup;
    public Transform affixIconDisplay;

    public Image blackFade;
    EnemySpawner enemySpawner;

    TimeManager timeManager;
    bool fadingFromBlack = true;
    bool fadingToBlack = false;
    float blackFadeAlpha = 1f;

    public delegate void OnScreenBlack();
    public static event OnScreenBlack onScreenBlack;

    public delegate void OnFadeCompleted();
    public static event OnFadeCompleted onFadeCompleted;


    private void Awake()
    {
        enemySpawner = GameObject.Find("Game Management").GetComponent<EnemySpawner>();
        timeManager = GameObject.Find("Game Management").GetComponent<TimeManager>();
        audioSource = GetComponent<AudioSource>();
        canvas = GetComponent<Canvas>();
        enemySpawner.UpdateGameplayUIReference(this);
        enemySpawner.onEnemyDeath += IncrementKillCounter;
      //  enemySpawner.onEnemyDeath += UpdateBossBar;
        SceneManager.sceneLoaded += ClearAffixIcons;
        SceneManager.sceneLoaded += FadeFromBlack;
        SetRenderCamera(Camera.main);
        ResetKillCounter();
      //  ResetBossBar();
    }

    private void Start()
    {
        RefreshPlayerAffixUI();
    }

    public void RefreshPlayerAffixUI()
    {
        BaseAffix[] playerAffixes = PlayerManagement.player.GetComponents<BaseAffix>();
        foreach (BaseAffix affix in playerAffixes)
        {
            UpdatePlayerAffixDisplay(affix);
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= FadeFromBlack;
        enemySpawner.onEnemyDeath -= IncrementKillCounter;
       // enemySpawner.onEnemyDeath -= UpdateBossBar;
        SceneManager.sceneLoaded -= ClearAffixIcons;
    }

    public void SetRenderCamera(Camera camera)
    {
        canvas.worldCamera = camera;
    }

    public void EnableCurrentWaveText()
    {
        currentWaveText.enabled = true;
    }

    public void UpdateCurrentWaveText(int waveNumber)
    {
        currentWaveText.text = "Wave: " + waveNumber.ToString();
    }

    // WAVE INFO TEXT //
    public void ToggleWaveInfoText(bool toggle)
    {
        waveInfoGameObject.SetActive(toggle);
    }

    public void UpdateWaveInfoText(string text)
    {
        waveInfoText.text = text;
    }
    // -------------------------------------------------------

    // AFFIX PANEL //
    public void UpdatePlayerAffixDisplay(BaseAffix newAffix)
    {
        Transform existingAffixIconTransform = affixIconDisplay.Find(newAffix.affixName);
        GameObject existingAffixIcon = null;
        if (existingAffixIconTransform)
            existingAffixIcon = existingAffixIconTransform.gameObject;

        if (existingAffixIcon == null)
        {
            Sprite affixIcon = newAffix.affixIcon;
            existingAffixIcon = Instantiate(affixDisplayObject, playerAffixDisplay.transform);
            Image image = existingAffixIcon.GetComponent<Image>();
            existingAffixIcon.name = newAffix.affixName;
            image.sprite = affixIcon;

            Material shaderMaterial = new Material(image.material);
            if (newAffix.rarity != Rarity.COMMON)
                shaderMaterial.SetFloat("_OutlineAlpha", 1f);
            shaderMaterial.SetColor("_OutlineColor", RarityUtil.GetRarityColor(newAffix.rarity));

            image.material = shaderMaterial;
        }

        String newQuantity = (newAffix.affixCount).ToString();
        existingAffixIcon.GetComponentInChildren<TextMeshProUGUI>().text = newQuantity;
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
        //int experience = PlayerManagement.player.GetComponent<PlayerExperience>().xpToNextLevel;
        UpdatePlayerHealth(health, health);
    }

    public void UpdatePlayerHealth(int currentHealth, int maxHealth)
    {
        healthText.text = currentHealth + " / " + maxHealth;
        playerHealthBar.fillAmount = (float) currentHealth / maxHealth;
    }

    public void UpdateLocalCurrencyInfo(int newCurrencyAmount)
    {
        currencyAnim.SetTrigger("AddCurrency");
        localCurrencyInfo.text = newCurrencyAmount.ToString();
    }

    public void UpdateGlobalCurrencyInfo(int newCurrencyAmount)
    {
        globalCurrencyAnim.SetTrigger("AddCurrency");
        globalCurrencyInfo.text = newCurrencyAmount.ToString();
    }
    // ----------------------------------------------------

    // PICKED UP AFFIX PANEL //
    public void ShowPickedUpAffixPanel(AffixObject affix)
    {
        StartCoroutine(PickUpAffixCoroutine(affix));
    }

    IEnumerator PickUpAffixCoroutine(AffixObject affix)
    {
        UpdatePickedUpAffixPanel(affix);
        pickedUpAffixPanel.SetActive(true);
        yield return new WaitForSeconds(pickedUpAffixWindowDuration);
        pickedUpAffixPanel.SetActive(false);
    }

    public void UpdatePickedUpAffixPanel(AffixObject affix)
    {
        pickedUpAffixIconImage.sprite = affix.icon;
        pickedUpAffixTitleText.text = affix.affixName;
        pickedUpAffixDescriptionText.text = affix.affixDescription;
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
    public void IncrementKillCounter(BaseEnemy enemy, EnemySpawner spawner)
    {
        killCountText.text = spawner.enemiesKilled.ToString();
    }

    public void ResetKillCounter()
    {
        killCountText.text = "0";
    }

	//public void UpdateBossBar(BaseEnemy enemy, EnemySpawner spawner)
	//{
 //      // playerBossBar.fillAmount = (float) spawner.enemiesKilled / spawner.enemiesToSpawnBoss;
	//}

 //   public void ResetBossBar()
 //   {
 //       playerBossBar.fillAmount = 0f;
 //   }


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
