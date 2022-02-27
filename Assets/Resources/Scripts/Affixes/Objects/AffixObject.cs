using UnityEngine;

[CreateAssetMenu(fileName = "New Affix", menuName = "Affix")]
public class AffixObject : ScriptableObject
{
    public Sprite icon;
    public string affixName;
    public string affixDescription;
    public bool isUniqueAffix;
    public GameObject affixPrefab;
}
