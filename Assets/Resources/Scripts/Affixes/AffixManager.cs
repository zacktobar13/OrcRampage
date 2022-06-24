using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AffixManager : MonoBehaviour
{
    List<GameObject> commonAffixes = new List<GameObject>();
    List<GameObject> uncommonAffixes = new List<GameObject>();
    List<GameObject> magicAffixes = new List<GameObject>();
    List<GameObject> epicAffixes = new List<GameObject>();
    List<GameObject> legendaryAffixes = new List<GameObject>();
    List<GameObject> ancientAffixes = new List<GameObject>();

    private void Start()
    {
        string pathToAffixDrops = "Prefabs/Interactibles/Affix Drops";
        GameObject[] affixDrops = Resources.LoadAll<GameObject>(pathToAffixDrops);
        foreach (GameObject affixDrop in affixDrops)
        {
            AffixObject affix = affixDrop.GetComponent<AffixDrop>().affix;
            switch (affix.affixRarity)
            {
                case Rarity.COMMON:
                    commonAffixes.Add(affixDrop);
                    continue;
                case Rarity.UNCOMMON:
                    uncommonAffixes.Add(affixDrop);
                    continue;
                case Rarity.MAGIC:
                    magicAffixes.Add(affixDrop);
                    continue;
                case Rarity.EPIC:
                    epicAffixes.Add(affixDrop);
                    continue;
                case Rarity.LEGENDARY:
                    legendaryAffixes.Add(affixDrop);
                    continue;
                case Rarity.ANCIENT:
                    ancientAffixes.Add(affixDrop);
                    continue;
            }
        }
    }

    public GameObject GetRandomAffixDrop(Rarity rarity)
    {
        switch (rarity)
        {
            case Rarity.COMMON:
                return commonAffixes[Random.Range(0, commonAffixes.Count)];
            case Rarity.UNCOMMON:
                return uncommonAffixes[Random.Range(0, uncommonAffixes.Count)];
            case Rarity.MAGIC:
                return magicAffixes[Random.Range(0, magicAffixes.Count)];
            case Rarity.EPIC:
                return epicAffixes[Random.Range(0, epicAffixes.Count)];
            case Rarity.LEGENDARY:
                return legendaryAffixes[Random.Range(0, legendaryAffixes.Count)];
            case Rarity.ANCIENT:
                return ancientAffixes[Random.Range(0, ancientAffixes.Count)];
            default:
                return null;
        }
    }
}
