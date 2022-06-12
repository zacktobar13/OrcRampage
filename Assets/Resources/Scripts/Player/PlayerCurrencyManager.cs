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
        gameplayUI.UpdateLocalCurrencyInfo(localCurrency);
        gameplayUI.UpdateGlobalCurrencyInfo(PlayerSerializedStats.GetGlobalCurrency());
    }

    public void AddLocalCurrency(int amountToAdd)
    {
        localCurrency += playerStats.CalculateGoldGained(amountToAdd);
        gameplayUI.UpdateLocalCurrencyInfo(localCurrency);
    }

    public bool RemoveLocalCurrency(int amountToRemove)
    {
        if (amountToRemove > localCurrency)
        {
            Debug.Log("Not enough money, need " + amountToRemove + ", have: " + localCurrency);
            return false;
        }

        localCurrency -= amountToRemove;
        gameplayUI.UpdateLocalCurrencyInfo(localCurrency);
        return true;
    }

    public bool CanAffordLocal(int price)
    {
        return localCurrency >= price;
    }

    public void AddGlobalCurrency(int amountToAdd)
    {
        int newAmount = PlayerSerializedStats.AddGlobalCurrency(amountToAdd);
        gameplayUI.UpdateGlobalCurrencyInfo(newAmount);
    }

    public bool RemoveGlobalCurrency(int amountToRemove)
    {
        int currentGlobalCurrency = PlayerSerializedStats.GetGlobalCurrency();
        if (amountToRemove > currentGlobalCurrency)
        {
            return false;
        }

        int newAmount = PlayerSerializedStats.RemoveGlobalCurrency(amountToRemove);
        gameplayUI.UpdateGlobalCurrencyInfo(newAmount);
        return true;
    }

    public bool CanAffordGlobal(int price)
    {
        return PlayerSerializedStats.GetGlobalCurrency() >= price;
    }

    private void OnApplicationQuit()
    {
        PlayerSerializedStats.SerializeAllStats();
    }
}
