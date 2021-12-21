using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AffixButton : MonoBehaviour
{
    public GameObject affixGameObject;
    BaseAffix affix;
    Image image;

    public TextMeshProUGUI affixNameText;
    public TextMeshProUGUI affixDescriptionText;

    public delegate void OnAffixChosen(BaseAffix affix);
    public static event OnAffixChosen onAffixChosen;

    public void OnEnable()
    {
        affix = affixGameObject.GetComponent<BaseAffix>();
        image = GetComponent<Image>();
        image.sprite = affix.affixIcon;
    }

    public void AddAffixToPlayer()
    {
        BaseAffix myAffix = affixGameObject.GetComponent<BaseAffix>();
        PlayerManagement.player.AddComponent(myAffix.GetType());
        onAffixChosen(affix);
    }

    public void UpdateAffixNameText()
    {
        affixNameText.SetText(affix.affixName);
    }

    public void UpdateAffixDescriptionText()
    {
        affixDescriptionText.SetText(affix.affixDescription);
    }

    public void ClearNameAndDescriptionText()
    {
        affixNameText.SetText("");
        affixDescriptionText.SetText("");
    }
}
