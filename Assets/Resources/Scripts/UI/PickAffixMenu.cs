using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PickAffixMenu : MonoBehaviour
{
    public AffixObject[] affixChoices;
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
        SpawnAffixButtons();
        gameplayUI = transform.parent.GetComponent<GameplayUI>();
        player = PlayerManagement.player;
        playerExperience = player.GetComponent<PlayerExperience>();
        UpdateRewardText();
        UpdatePlayerLevelText();
        menuBackground.sizeDelta = new Vector2(300f * choicesAvailable, menuBackground.rect.height);
    }

	public void SetQuantityToChoose(int val)
    {
        affixChoicesRemaining = val;
    }

    public void SpawnAffixButtons()
    {
        AffixButton affixButton;
        spawnedButtons = new GameObject[choicesAvailable];

        choicesAvailable = Mathf.Min(3, affixChoices.Length);
        for (int i = 0; i < choicesAvailable; i++)
        {
            GameObject button = Instantiate(buttonGameObject);
            spawnedButtons[i] = button;
            AffixObject affixChosen = affixChoices[Random.Range(0, affixChoices.Length)];
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
            BaseAffix addedAffix = player.AddComponent(affix.GetType()) as BaseAffix;
            addedAffix.IntializeFromScriptableObject(affixData);
        }

        // Remove affix from options if it's unique
        if (affixData.isUniqueAffix)
        {
            for (int i = 0; i < affixChoices.Length; i++)
            {
                AffixObject affixChoice = affixChoices[i];
                if (affixChoice.affixName == affixData.affixName)
                {
                    affixChoices = Utility.RemoveAt<AffixObject>(affixChoices, i);
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
