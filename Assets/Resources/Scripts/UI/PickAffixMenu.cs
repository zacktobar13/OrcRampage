using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickAffixMenu : MonoBehaviour
{
    int numAffixesToChoose;
    GameObject player;
    GameplayUI gameplayUI;

    void OnEnable()
    {
        Time.timeScale = 0;
        Debug.Log("Number to choose: " + numAffixesToChoose);
        SpawnAffixButtons();
    }

	private void Start()
	{
        gameplayUI = transform.parent.GetComponent<GameplayUI>();
        player = PlayerManagement.player;
    }

	public void SetNumAffixesToChoose(int val)
    {
        numAffixesToChoose = val;
    }

    public void SpawnAffixButtons()
    {
        // Pick from pool of affixes and spawns the buttons
    }

    public void AddAffixToPlayer(BaseAffix affix)
    {
        // Logic to add affix component to player here

        numAffixesToChoose -= 1;
        Debug.Log("Added affix. Number left: " + numAffixesToChoose);

        if (numAffixesToChoose <= 0)
        {
            gameplayUI.HideAffixPanel();
            gameplayUI.ShowPlayerInfoPanel();
        }
        else
        {
            SpawnAffixButtons();
        }
    }
}
