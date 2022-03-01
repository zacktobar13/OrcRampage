using UnityEngine.UI;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerExperience : MonoBehaviour
{
	public int playerLevel;
	public int currentXp;
	public int xpToNextLevel;
	public GameplayUI gameplayUI;
	PlayerStats playerStats;

	private void Start()
	{
		gameplayUI = GameObject.Find("Gameplay UI").GetComponent<GameplayUI>();
		playerStats = GetComponent<PlayerStats>();
		playerLevel = 1;
		xpToNextLevel = CalculateXpToNextLevel(playerLevel);
		gameplayUI.UpdatePlayerLevel(playerLevel);
		gameplayUI.UpdatePlayerExperienceBar(currentXp, xpToNextLevel);
	}

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
			Debug.Log("Added 1000 XP");
			AddXp(1000);
        }
    }

    public int CalculateXpToNextLevel(int level)
	{
		return 100 * level;
	}

	public void AddXp(int amount)
	{
		currentXp += playerStats.CalculateExperienceGained(amount);

		// TODO: This will be the number of times the affix menu needs to pop-up
		int numberOfLevelsGained = 0;

		while (ReachedNextLevel())
		{
			numberOfLevelsGained++;
			currentXp -= xpToNextLevel;
			playerLevel += 1;
			xpToNextLevel = CalculateXpToNextLevel(playerLevel);
		}

		if (numberOfLevelsGained > 0)
		{
			ToggleAffixPanel(numberOfLevelsGained);
		}

		gameplayUI.UpdatePlayerLevel(playerLevel);
		gameplayUI.UpdatePlayerExperienceBar(currentXp, xpToNextLevel);
	}

	public bool ReachedNextLevel()
	{
		return currentXp >= xpToNextLevel;
	}

	void ToggleAffixPanel(int numberToChoose)
	{
		gameplayUI.ShowAffixPanel(numberToChoose);
	}
	
}
