using UnityEngine;

[CreateAssetMenu(fileName = "New Affix", menuName = "Affix")]
public class AffixObject : ScriptableObject
{
    public Sprite icon;
    public string affixName;
    public string affixDescription;
    public Rarity affixRarity;
    public bool isUniqueAffix;
    public GameObject affixPrefab;

    [Header("Scalar Increasing Affixes")]
    public float scalarIncreasePerAffix;

    [Header("Damage Dealing Affixes")]
    public int baseDamage;
    public float baseProcChance;
}
