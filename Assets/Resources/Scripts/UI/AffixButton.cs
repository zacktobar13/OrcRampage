using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AffixButton : MonoBehaviour
{
	public TextMeshProUGUI affixTitle;
	public TextMeshProUGUI affixDescription;
	public Image affixIcon;
	public Image affixIconShadow;

	[HideInInspector]
	public GameplayUI gameplayUI;
	[HideInInspector]
	public PickAffixMenu affixMenu;

	BaseAffix affixReference;
	AffixObject affixData;

	public void Select()
	{
		affixMenu.AddAffixToPlayer(affixReference, affixData);
	}

	public void SetMyAffix(AffixObject affix)
	{
		affixReference = affix.affixPrefab.GetComponent<BaseAffix>();
		affixData = affix;
		affixTitle.text = affix.affixName;
		affixDescription.text = affix.affixDescription;
		affixIcon.sprite = affix.icon;
		affixIconShadow.sprite = affix.icon;
	}
}
