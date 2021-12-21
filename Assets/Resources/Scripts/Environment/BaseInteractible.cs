using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseInteractible : MonoBehaviour
{
    public virtual void OnPlayerInteract()
    {
        Debug.LogWarning("Called base OnPlayerInteract with " + gameObject.name + ". Override failed.");
    }
}
