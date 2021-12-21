using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalCurrencyDrop : MonoBehaviour
{

    public int amount = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            PlayerCurrencyManager.localCurrency += amount;
            Debug.Log("New Local Currency Amount: " + PlayerCurrencyManager.localCurrency);
            Destroy(gameObject);
        }
    }
}
