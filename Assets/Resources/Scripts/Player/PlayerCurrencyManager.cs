using UnityEngine;

public class PlayerCurrencyManager : MonoBehaviour
{
    public int localCurrency;
    GameplayUI gameplayUI;
    PlayerStats playerStats;

    private void Start()
    {
        GameObject player = PlayerManagement.player;
        playerStats = player.GetComponent<PlayerStats>();
        gameplayUI = GameObject.Find("Gameplay UI").GetComponent<GameplayUI>();
        gameplayUI.UpdateCurrencyInfo(localCurrency);
    }

    public void AddCurrency(int amountToAdd)
    {
        localCurrency += playerStats.CalculateGoldGained(amountToAdd);
        gameplayUI.UpdateCurrencyInfo(localCurrency);
    }

    public bool RemoveCurrency(int amountToRemove)
    {
        if (amountToRemove > localCurrency)
        {
            Debug.Log("Not enough money, need " + amountToRemove + ", have: " + localCurrency);
            return false;
        }

        Debug.Log("Spending money: " + amountToRemove);
        localCurrency -= amountToRemove;
        gameplayUI.UpdateCurrencyInfo(localCurrency);
        return true;
    }
}
