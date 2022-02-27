using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AffixButton : MonoBehaviour
{
	public GameplayUI gameplayUI;
	public PickAffixMenu affixMenu;
	BaseAffix affixReference;

	public void Select()
	{
		affixMenu.AddAffixToPlayer(affixReference);
	}

	public void SetMyAffix(BaseAffix affix)
	{
		affixReference = affix;
	}
}
