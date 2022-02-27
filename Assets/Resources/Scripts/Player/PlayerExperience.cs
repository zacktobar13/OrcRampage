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
		return 1000;
	}

	public void AddXp(int amount)
	{
		Debug.Log("Adding " + amount + " experience");
		currentXp += amount;

		gameplayUI.UpdatePlayerExperienceBar(currentXp, xpToNextLevel);
		
		if (ReachedNextLevel())
		{
			currentXp -= xpToNextLevel;
			playerLevel += 1;
			xpToNextLevel = CalculateXpToNextLevel(playerLevel);
			gameplayUI.UpdatePlayerLevel(playerLevel);
		}
	}

	public bool ReachedNextLevel()
	{
		return currentXp >= xpToNextLevel;
	}

}
