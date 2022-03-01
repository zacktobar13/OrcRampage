using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PickAffixMenu : MonoBehaviour
{
    public AffixObject[] affixChoices;
    public AffixObject[] currentRunAffixChoices;
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

	private void OnEnable()
	{
        // Only re initialize if we are of length 0 (i.e. this is a new run)
        if (currentRunAffixChoices.Length == 0)
        {
            InitializeAffixChoices(SceneManager.GetActiveScene(), LoadSceneMode.Single);
        }

        SpawnAffixButtons();
        gameplayUI = transform.parent.GetComponent<GameplayUI>();
        player = PlayerManagement.player;
        playerExperience = player.GetComponent<PlayerExperience>();
        UpdateRewardText();
        UpdatePlayerLevelText();
        menuBackground.sizeDelta = new Vector2(300f * choicesAvailable, menuBackground.rect.height);
        SceneManager.sceneLoaded += InitializeAffixChoices;
    }

    void InitializeAffixChoices(Scene scene, LoadSceneMode mode)
    {
        choicesAvailable = affixChoices.Length;
        currentRunAffixChoices = new AffixObject[affixChoices.Length];
        affixChoices.CopyTo(currentRunAffixChoices, 0);
    }

	public void SetQuantityToChoose(int val)
    {
        affixChoicesRemaining = val;
    }

    public void SpawnAffixButtons()
    {
        AffixButton affixButton;
        spawnedButtons = new GameObject[choicesAvailable];

        choicesAvailable = Mathf.Min(3, currentRunAffixChoices.Length);
        int[] affixChoices = new int[currentRunAffixChoices.Length];
        for (int i = 0; i < affixChoices.Length; i++)
        {
            affixChoices[i] = i;
        }
        Utility.Shuffle(affixChoices);

        for (int i = 0; i < choicesAvailable; i++)
        {
            GameObject button = Instantiate(buttonGameObject);
            spawnedButtons[i] = button;
            AffixObject affixChosen = currentRunAffixChoices[affixChoices[i]];
            affixButton = button.GetComponent<AffixButton>();
            affixButton.affixMenu = this;
            affixButton.SetMyAffix(affixChosen);
            button.transform.SetParent(transform.Find("Affix Button Panel").transform, false);
        }
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
            for (int i = 0; i < affixChoices.Length; i++)
            {
                AffixObject affixChoice = currentRunAffixChoices[i];
                if (affixChoice.affixName == affixData.affixName)
                {
                    currentRunAffixChoices = Utility.RemoveAt<AffixObject>(currentRunAffixChoices, i);
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
}
