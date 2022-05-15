using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AffixDrop : DroppedItem
{
    public AffixObject affix;
    BaseAffix affixReference;

    GameObject player;
    GameplayUI gameplayUI;
    SpriteRenderer spriteRenderer;
    protected Material shaderMaterial; 

    private new void Start()
    {
        base.Start();
        shaderMaterial = transform.Find("Sprite").GetComponent<SpriteRenderer>().material;
        spriteRenderer = transform.Find("Sprite").GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = affix.icon;
        gameplayUI = GameObject.Find("Gameplay UI").GetComponent<GameplayUI>();
        player = PlayerManagement.player;

        Color rarityColor = RarityUtil.GetRarityColor(affix.affixRarity);
        shaderMaterial.SetColor("_OutlineColor", rarityColor);
    }

	protected override void Consume()
	{
        base.Consume();
        AddAffixToPlayer();
	}

	public void AddAffixToPlayer()
    {
        affixReference = affix.affixPrefab.GetComponent<BaseAffix>();
        BaseAffix alreadyOnPlayer = (BaseAffix)player.GetComponent(affixReference.GetType());
        if (alreadyOnPlayer != null)
        {
            alreadyOnPlayer.AddAffixCount(1);
        }
        else
        {
            alreadyOnPlayer = player.AddComponent(affixReference.GetType()) as BaseAffix;
            alreadyOnPlayer.IntializeFromScriptableObject(affix);
        }

        gameplayUI.UpdatePlayerAffixDisplay(alreadyOnPlayer);
    }
}