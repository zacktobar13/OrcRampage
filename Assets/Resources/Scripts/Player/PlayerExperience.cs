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

	private void Start()
	{
		playerLevel = 1;
		xpToNextLevel = 20;
	}

	public int CalculateXpToNextLevel(int level)
	{
		return 100 * level;
	}

	public void AddXp(int amount)
	{
		currentXp += amount;

		// TODO: This will be the number of times the affix menu needs to pop-up
		int numberOfLevelsGained = 0;

		while (ReachedNextLevel())
		{
			Time.timeScale = 0;
			ToggleAffixPanel();
			numberOfLevelsGained++;
			currentXp -= xpToNextLevel;
			playerLevel += 1;
			xpToNextLevel = CalculateXpToNextLevel(playerLevel);
		}

		gameplayUI.UpdatePlayerLevel(playerLevel);
		gameplayUI.UpdatePlayerExperienceBar(currentXp, xpToNextLevel);
	}

	public bool ReachedNextLevel()
	{
		return currentXp >= xpToNextLevel;
	}

	void ToggleAffixPanel()
	{
		gameplayUI.playerInfoPanel.SetActive(false);
		gameplayUI.affixPanel.SetActive(true);
	}
	
}
