using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameplayUI : MonoBehaviour
{
    public TextMeshProUGUI ammoText;
    public Image weaponImage;

    public GameObject[] experienceInfoGroup;
    public GameObject[] weaponInfoGroup;

    public Image blackFade;
    bool fadingFromBlack = true;
    bool fadingToBlack = false;
    float blackFadeAlpha = 1f;

    public delegate void OnScreenBlack();
    public static event OnScreenBlack onScreenBlack;

    public delegate void OnFadeCompleted();
    public static event OnFadeCompleted onFadeCompleted;

    private void Start()
    {
        SceneManager.sceneLoaded += FadeFromBlack;
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player)
        {
            Weapon playerWeapon = player.GetComponentInChildren<PlayerAttack>().GetCurrentWeapon();
            ammoText.text = playerWeapon.currentAmmo + " / " + playerWeapon.maxAmmo;
            weaponImage.sprite = playerWeapon.notFiringSprite;
        }
    }

    private void FixedUpdate()
    {
        if (fadingFromBlack)
        {
            blackFadeAlpha -= .005f;
            var tempColor = blackFade.color;
            tempColor.a = blackFadeAlpha;
            blackFade.color = tempColor;

            if (blackFadeAlpha <= 0f)
            {
                fadingFromBlack = false;
                blackFade.enabled = false;

                if (onFadeCompleted != null)
                {
                    onFadeCompleted();
                }
            }
        }

        if (fadingToBlack)
        {
            blackFadeAlpha += .005f;
            var tempColor = blackFade.color;
            tempColor.a = blackFadeAlpha;
            blackFade.color = tempColor;

            if (blackFadeAlpha >= 1f)
            {
                fadingToBlack = false;
                if (onScreenBlack != null)
                {
                    onScreenBlack();
                }
            }
        }
    }

    // Toggles wave info group UI
    public void ToggleExperienceInfoGroup(bool toggle)
    {
        foreach (GameObject member in experienceInfoGroup)
        {
            member.SetActive(toggle);
        }
    }

    // Toggles weapon info group UI
    public void ToggleWeaponInfoGroup(bool toggle)
    {
        foreach (GameObject member in weaponInfoGroup)
        {
            member.SetActive(toggle);
        }
    }

    public void FadeFromBlack(Scene scene, LoadSceneMode sceneLoadMode)
    {
        blackFade.enabled = true;
        fadingFromBlack = true;
    }

    public void FadeToBlack()
    { 
        blackFade.enabled = true;
        fadingToBlack = true;
    }
}
