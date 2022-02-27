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

	[HideInInspector]
	public GameplayUI gameplayUI;
	[HideInInspector]
	public PickAffixMenu affixMenu;

	BaseAffix affixReference;

	public void Select()
	{
		affixMenu.AddAffixToPlayer(affixReference);
	}

	public void SetMyAffix(AffixObject affix)
	{
		affixReference = affix.affixPrefab.GetComponent<BaseAffix>();
		affixReference.IntializeFromScriptableObject(affix);
		affixTitle.text = affix.affixName;
		affixDescription.text = affix.affixDescription;
		affixIcon.sprite = affix.icon;
	}
}
