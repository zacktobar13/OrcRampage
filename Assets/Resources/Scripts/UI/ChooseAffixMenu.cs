using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChooseAffixMenu : MonoBehaviour
{
    public TextMeshProUGUI affixNameText;
    public TextMeshProUGUI affixDescriptionText;

    public GameObject[] menuElements;

    public GameObject[] affixButtons;
    public GameObject[] affixes;

    public delegate void OnShowAffixMenu();
    public static event OnShowAffixMenu onShowAffixMenu;


    void Awake()
    {
        affixNameText.SetText("");
        affixDescriptionText.SetText("");

        AffixButton.onAffixChosen += OnAffixChosen;
        PlayerHealth.onRespawn += ToggleAffixMenu;
    }

    private void OnEnable()
    {
        if (onShowAffixMenu != null)
            onShowAffixMenu();

        PlayerManagement.TogglePlayerControl(false);

        // Shuffle affixes
        for (int i = 0; i < affixButtons.Length; i++)
        {
            GameObject temp = affixes[i];
            int roll = Random.Range(0, affixes.Length);
            affixes[i] = affixes[roll];
            affixes[roll] = temp;
        }

        // Set affix buttons to corresponding affix
        for (int i = 0; i < affixButtons.Length; i++)
        {
            AffixButton affixButton = affixButtons[i].GetComponent<AffixButton>();
            if (i > affixes.Length)
            {
                // TODO: Set to default empty affix instead of null
                affixButton.affixGameObject = null;
                continue;
            }

            affixButton.affixGameObject = affixes[i];

            // Force an update to refresh UI
            affixButton.OnEnable();

        }

    }

    public void HideAffixMenu()
    {
        foreach (GameObject menuElement in menuElements)
        {
            menuElement.SetActive(false);
        }


        //gameObject.SetActive(!playerHealth.isCurrentlyDead);
    }

    public void ToggleAffixMenu(PlayerHealth playerHealth)
    {
        foreach(GameObject menuElement in menuElements)
        {
            menuElement.SetActive(!playerHealth.isCurrentlyDead);
        }

        if (!playerHealth.isCurrentlyDead)
        {
            if (onShowAffixMenu != null)
                onShowAffixMenu();

        }
    }

    public void OnAffixChosen(BaseAffix affix)
    {
        HideAffixMenu();
        PlayerManagement.TogglePlayerControl(true);
    }

    private void OnDestroy()
    {
        AffixButton.onAffixChosen -= OnAffixChosen;
        PlayerHealth.onRespawn -= ToggleAffixMenu;
    }
}
