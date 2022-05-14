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
		return 50 * level;
	}

	void LevelUp()
    {
		// Boost stats
		playerStats.LevelUp();

		currentXp -= xpToNextLevel;
		playerLevel += 1;
		xpToNextLevel = CalculateXpToNextLevel(playerLevel);
	}

	public void AddXp(int amount)
	{
		currentXp += playerStats.CalculateExperienceGained(amount);

		// TODO: This will be the number of times the affix menu needs to pop-up
		int numberOfLevelsGained = 0;

		while (ReachedNextLevel())
		{
			LevelUp();
			numberOfLevelsGained++;
		}

		if (numberOfLevelsGained > 0)
		{
			ToggleAffixPanel(numberOfLevelsGained);
		}
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
