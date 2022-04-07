using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Rarity
{
    COMMON,
    UNCOMMON,
    MAGIC,
    EPIC,
    LEGENDARY,
    ANCIENT
}

public class RarityUtil : MonoBehaviour
{
    public static Color CommonColor = Color.white;
    public static Color UncommonColor = Color.green;
    public static Color MagicColor = Color.blue;
    public static Color EpicColor = new Color(.75f, 0, 1f, 1f);
    public static Color LegendaryColor = new Color(1f, .5f, 0f, 1f);
    public static Color AncientColor = Color.red;

    public static Color GetRarityColor(Rarity rarity)
    {
        switch(rarity)
        {
            case Rarity.COMMON: // WHITE
                return CommonColor;
            case Rarity.UNCOMMON: // GREEN
                return UncommonColor;
            case Rarity.MAGIC: // BLUE
                return MagicColor;
            case Rarity.EPIC: // PURPLE
                return EpicColor;
            case Rarity.LEGENDARY: // ORANGE
                return LegendaryColor;
            case Rarity.ANCIENT: // RED
                return AncientColor;
            default:
                Debug.Assert(false, "Rarity isn't assigned a color");
                return Color.white;
        }
    }

    public static float GetRaritySizeScalar(Rarity rarity)
    {
        switch (rarity)
        {
            case Rarity.COMMON:
                return 1f;
            case Rarity.UNCOMMON:
                return 1.1f;
            case Rarity.MAGIC:
                return 1.2f;
            case Rarity.EPIC:
                return 1.3f;
            case Rarity.LEGENDARY:
                return 1.4f;
            case Rarity.ANCIENT:
                return 1.5f;
            default:
                return 1f;
        }
    }

    
}
