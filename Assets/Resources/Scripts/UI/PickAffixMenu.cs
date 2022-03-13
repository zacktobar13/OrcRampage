using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PickAffixMenu : MonoBehaviour
{
    public AffixObject[] commonAffixChoices;
    public AffixObject[] uncommonAffixChoices;
    public AffixObject[] magicAffixChoices;
    public AffixObject[] epicAffixChoices;
    public AffixObject[] legendaryAffixChoices;
    public AffixObject[] ancientAffixChoices;
    AffixObject[] currentRunCommonAffixChoices;
    AffixObject[] currentRunUncommonAffixChoices;
    AffixObject[] currentRunMagicAffixChoices;
    AffixObject[] currentRunEpicAffixChoices;
    AffixObject[] currentRunLegendaryAffixChoices;
    AffixObject[] currentRunAncientAffixChoices;

    public float commonAffixChance;
    public float uncommonAffixChance;
    public float magicAffixChance;
    public float epicAffixChance;
    public float legendaryAffixChance;
    public float ancientAffixChance;
    float totalAffixChance;

    public TextMeshProUGUI playerLevelText;
    public TextMeshProUGUI rewardText;
    public GameObject buttonGameObject;
    public RectTransform menuBackground;

    int affixChoicesRemaining;
    int choicesAvailable = 3;
    GameObject player;
    GameplayUI gameplayUI;
    PlayerExperience playerExperience;
    GameObject[] spawnedButtons;

    private void Awake()
    {
        InitializeAffixChoices(SceneManager.GetActiveScene(), LoadSceneMode.Single);
        SceneManager.sceneLoaded += InitializeAffixChoices;
    }

    private void OnEnable()
	{
        totalAffixChance = commonAffixChance + uncommonAffixChance + magicAffixChance + epicAffixChance + legendaryAffixChance + ancientAffixChance;

        SpawnAffixButtons();
        gameplayUI = transform.parent.GetComponent<GameplayUI>();
        player = PlayerManagement.player;
        playerExperience = player.GetComponent<PlayerExperience>();
        UpdateRewardText();
        UpdatePlayerLevelText();
        menuBackground.sizeDelta = new Vector2(300f * choicesAvailable, menuBackground.rect.height);        
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= InitializeAffixChoices;
    }

    void SpawnAffixButtons()
    {
        int numberOfChoices = 3;
        spawnedButtons = new GameObject[numberOfChoices];
        for (int i = 0; i < numberOfChoices; i++)
        {
            Rarity rarity = RollAffixRarity();
            AffixObject[] affixChoices = getAffixChoices(rarity);
            AffixObject affixChoice = affixChoices[Random.Range(0, affixChoices.Length)];
            SpawnAffixButton(affixChoice, i);
        }
    }

    void InitializeAffixChoices(Scene scene, LoadSceneMode mode)
    {
        currentRunCommonAffixChoices = new AffixObject[commonAffixChoices.Length];
        commonAffixChoices.CopyTo(currentRunCommonAffixChoices, 0);

        currentRunUncommonAffixChoices = new AffixObject[uncommonAffixChoices.Length];
        uncommonAffixChoices.CopyTo(currentRunUncommonAffixChoices, 0);

        currentRunMagicAffixChoices = new AffixObject[magicAffixChoices.Length];
        magicAffixChoices.CopyTo(currentRunMagicAffixChoices, 0);

        currentRunEpicAffixChoices = new AffixObject[epicAffixChoices.Length];
        epicAffixChoices.CopyTo(currentRunEpicAffixChoices, 0);

        currentRunLegendaryAffixChoices = new AffixObject[legendaryAffixChoices.Length];
        legendaryAffixChoices.CopyTo(currentRunLegendaryAffixChoices, 0);

        currentRunAncientAffixChoices = new AffixObject[ancientAffixChoices.Length];
        ancientAffixChoices.CopyTo(currentRunAncientAffixChoices, 0);
    }

	public void SetQuantityToChoose(int val)
    {
        affixChoicesRemaining = val;
    }

    public void SpawnAffixButton(AffixObject affix, int buttonIndex)
    {
        AffixButton affixButton;

        GameObject button = Instantiate(buttonGameObject);
        spawnedButtons[buttonIndex] = button;
        affixButton = button.GetComponent<AffixButton>();
        affixButton.affixMenu = this;
        affixButton.SetMyAffix(affix);
        button.transform.SetParent(transform.Find("Affix Button Panel").transform, false);
    }

    public void AddAffixToPlayer(BaseAffix affix, AffixObject affixData)
    {
        BaseAffix alreadyOnPlayer = (BaseAffix)player.GetComponent(affix.GetType());
        if (alreadyOnPlayer != null)
        {
            alreadyOnPlayer.AddAffixCount(1);
        }
        else
        {
            alreadyOnPlayer = player.AddComponent(affix.GetType()) as BaseAffix;
            alreadyOnPlayer.IntializeFromScriptableObject(affixData);
        }

        gameplayUI.UpdatePlayerAffixDisplay(alreadyOnPlayer);

        // Remove affix from options if it's unique
        if (affixData.isUniqueAffix)
        {
            AffixObject[] affixChoices = getAffixChoices(affixData.affixRarity);

            for (int i = 0; i < affixChoices.Length; i++)
            {
                AffixObject affixChoice = affixChoices[i];
                if (affixChoice.affixName == affixData.affixName)
                {
                    affixChoices = Utility.RemoveAt<AffixObject>(affixChoices, i);
                    UpdateCurrentRunAffixChoices(affixData.affixRarity, affixChoices);
                    break;
                }
            }
        }

        foreach (GameObject a in spawnedButtons)
        {
            Destroy(a);
        }

        affixChoicesRemaining -= 1;
        UpdateRewardText();

        if (affixChoicesRemaining <= 0)
        {
            gameplayUI.HideAffixPanel();
            gameplayUI.ShowPlayerInfoPanel();
        }
        else
        {
            SpawnAffixButtons();
        }
    }

    public Rarity RollAffixRarity()
    {
        float roll = Random.Range(0, totalAffixChance);
        if (roll <= commonAffixChance)
        {
            return Rarity.COMMON;
        }
        roll -= commonAffixChance;

        if (roll <= uncommonAffixChance)
        {
            return Rarity.UNCOMMON;
        }
        roll -= uncommonAffixChance;

        if (roll <= magicAffixChance)
        {
            return Rarity.MAGIC;
        }
        roll -= magicAffixChance;

        if (roll <= epicAffixChance)
        {
            return Rarity.EPIC;
        }
        roll -= epicAffixChance;

        if (roll <= legendaryAffixChance)
        {
            return Rarity.LEGENDARY;
        }

        return Rarity.ANCIENT;
    }

    void UpdateCurrentRunAffixChoices(Rarity rarity, AffixObject[] newData)
    {
        switch (rarity)
        {
            case Rarity.COMMON:
                currentRunCommonAffixChoices = newData;
                return;
            case Rarity.UNCOMMON:
                currentRunUncommonAffixChoices = newData;
                return;
            case Rarity.MAGIC:
                currentRunMagicAffixChoices = newData;
                return;
            case Rarity.LEGENDARY:
                currentRunLegendaryAffixChoices = newData;
                return;
            case Rarity.EPIC:
                currentRunEpicAffixChoices = newData;
                return;
            case Rarity.ANCIENT:
                currentRunAncientAffixChoices = newData;
                return;
        }

        Debug.Assert(false, "UpdateCurrentRunAffixChoices is incomplete");
        return;
    }

    void UpdatePlayerLevelText()
    {
        playerLevelText.text = "You've reached\nlevel " + playerExperience.playerLevel.ToString() + "!";
    }

    void UpdateRewardText()
    {
        if (affixChoicesRemaining > 1)
        {
            rewardText.text = "Choose " + affixChoicesRemaining.ToString() + " rewards";
        }
        else
        {
            rewardText.text = "Choose " + affixChoicesRemaining.ToString() + " reward";
        }
    }

    AffixObject[] getAffixChoices(Rarity rarity)
    {
        switch (rarity)
        {
            case Rarity.COMMON:
                return currentRunCommonAffixChoices;
            case Rarity.UNCOMMON:
                return currentRunUncommonAffixChoices;
            case Rarity.MAGIC:
                return currentRunMagicAffixChoices;
            case Rarity.LEGENDARY:
                return currentRunLegendaryAffixChoices;
            case Rarity.EPIC:
                return currentRunEpicAffixChoices;
            case Rarity.ANCIENT:
                return currentRunAncientAffixChoices;
        }

        Debug.Assert(false, "Get affix choices is incomplete");
        return currentRunCommonAffixChoices;
    }
}
