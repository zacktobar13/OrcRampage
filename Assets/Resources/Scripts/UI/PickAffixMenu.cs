using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PickAffixMenu : MonoBehaviour
{
    public AffixObject[] affixChoices;
    public TextMeshProUGUI playerLevelText;
    public TextMeshProUGUI rewardText;
    public GameObject buttonGameObject;

    int affixChoiceQuantity;
    int choicesAvailable = 3;
    GameObject player;
    GameplayUI gameplayUI;
    PlayerExperience playerExperience;
    GameObject[] spawnedButtons;

	private void OnEnable()
	{
        Time.timeScale = 0;
        SpawnAffixButtons();
        gameplayUI = transform.parent.GetComponent<GameplayUI>();
        player = PlayerManagement.player;
        playerExperience = player.GetComponent<PlayerExperience>();
        UpdateRewardText();
        UpdatePlayerLevelText();
    }

	public void SetQuantityToChoose(int val)
    {
        affixChoiceQuantity = val;
    }

    public void SpawnAffixButtons()
    {
        AffixButton affixButton;
        spawnedButtons = new GameObject[choicesAvailable];

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

    public void AddAffixToPlayer(BaseAffix affix)
    {
        BaseAffix alreadyOnPlayer = (BaseAffix)player.GetComponent(affix.GetType());
        if (alreadyOnPlayer != null)
        {
            alreadyOnPlayer.AddAffixCount(1);
        }
        else
        {
            player.AddComponent(affix.GetType());
        }

        foreach (GameObject a in spawnedButtons)
        {
            Destroy(a);
        }

        affixChoiceQuantity -= 1;
        UpdateRewardText();

        if (affixChoiceQuantity <= 0)
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
        if (affixChoiceQuantity > 1)
        {
            rewardText.text = "Choose " + affixChoiceQuantity.ToString() + " rewards";
        }
        else
        {
            rewardText.text = "Choose " + affixChoiceQuantity.ToString() + " reward";
        }
    }
}
