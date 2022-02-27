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
		gameplayUI.UpdatePlayerExperienceBar(currentXp, xpToNextLevel);
	}

	public int CalculateXpToNextLevel(int level)
	{
		return 1000;
	}

	public void AddXp(int amount)
	{
		currentXp += amount;

		int numberOfLevelsGained = 0;

		while (ReachedNextLevel())
		{
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

}
