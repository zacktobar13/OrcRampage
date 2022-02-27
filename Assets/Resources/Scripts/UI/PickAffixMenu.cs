using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PickAffixMenu : MonoBehaviour
{
    int affixChoiceQuantity;
    int choicesAvailable = 3;
    GameObject player;
    GameplayUI gameplayUI;
    public TextMeshProUGUI playerLevelText;
    public TextMeshProUGUI rewardText;
    PlayerExperience playerExperience;
    public GameObject buttonGameObject;
    AffixButton[] affixButtons;

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
        affixButtons = new AffixButton[choicesAvailable];

        // TODO Randomly select what affix each button will have.
        for (int i = 0; i < choicesAvailable; i++)
        {
            GameObject button = Instantiate(buttonGameObject);
            affixButtons[i] = button.GetComponent<AffixButton>();
            affixButtons[i].affixMenu = this;
            button.transform.SetParent(transform.Find("Affix Button Panel").transform, false);
        }
    }

    public void AddAffixToPlayer(BaseAffix affix)
    {
        // TODO Logic to add affix component to player here

        foreach (AffixButton a in affixButtons)
        {
            Destroy(a.gameObject);
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
