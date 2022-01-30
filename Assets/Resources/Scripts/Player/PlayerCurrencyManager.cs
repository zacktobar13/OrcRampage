using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCurrencyManager : MonoBehaviour
{
    public int localCurrency;
    GameplayUI gameplayUI;

    private void Start()
    {
        gameplayUI = GameObject.Find("Gameplay UI").GetComponent<GameplayUI>();
        gameplayUI.UpdateCurrencyInfo(localCurrency);
    }

    public void AddCurrency(int amountToAdd)
    {
        localCurrency += amountToAdd;
        gameplayUI.UpdateCurrencyInfo(localCurrency);
    }

    public bool RemoveCurrency(int amountToRemove)
    {
        if (amountToRemove > localCurrency)
            return false;

        localCurrency -= amountToRemove;
        gameplayUI.UpdateCurrencyInfo(localCurrency);
        return true;
    }
}
