﻿using UnityEngine;

public class GameplayUISingleton : MonoBehaviour
{
    public static GameplayUISingleton instance = null;
    void Awake()
    {
        //Check if instance already exists
        if (instance == null)
        {
            //if not, set instance to this
            instance = this;
        }

        //If instance already exists and it's not this:
        else if (instance != this)
        {
            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);
        }

#if UNITY_EDITOR
        if (Application.isPlaying)
            UnityEditor.SceneVisibilityManager.instance.ShowAll();
#endif

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);
    }

    private void OnDestroy()
    {
        if (this == instance)
            instance = null;
    }
}
