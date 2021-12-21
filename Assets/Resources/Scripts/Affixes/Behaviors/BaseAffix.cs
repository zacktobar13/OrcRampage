using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class adds no behavior but lets us do things like player.GetComponents<BaseAffix>() to get all affixes
/// </summary>

public class BaseAffix : MonoBehaviour
{
    public Sprite affixIcon;
    public string affixName;
    public string affixDescription;
}
