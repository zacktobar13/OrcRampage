using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PlayerSingleton : MonoBehaviour
{
    public static PlayerSingleton instance = null;
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
            UnityEditor.SceneVisibilityManager.instance.Show(gameObject, false);
#endif

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);

        PlayerManagement.SetPlayer(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (instance == this)
        {
            PlayerManagement.SetPlayer(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (this == instance)
            instance = null;
    }
}
