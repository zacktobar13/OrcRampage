using UnityEngine;
using UnityEngine.SceneManagement;

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

    private void OnApplicationQuit()
    {
        PlayerSerializedStats.SerializeAllStats();
    }

    private void Awake()
    {
        SceneManager.sceneLoaded += ReInitialize;
    }

    public void ReInitialize(Scene scene, LoadSceneMode lsm)
    {
        Start();
    }
}
