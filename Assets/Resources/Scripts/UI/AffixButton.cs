using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AffixButton : MonoBehaviour
{
	public GameplayUI gameplayUI;

	public void Select()
	{
		Time.timeScale = 1f;
		gameplayUI.affixPanel.SetActive(false);
		gameplayUI.playerInfoPanel.SetActive(true);
	}
}
