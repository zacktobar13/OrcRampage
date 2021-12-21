using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static Transform playerSpawnLocation;
    LevelInfo levelInfo;

    void Start()
    {
        levelInfo = GameObject.Find("Level Info").GetComponent<LevelInfo>();
        playerSpawnLocation = GameObject.Find("Level Entry Point").transform;
        string nextLevel = levelInfo.possibleLevels[1];
    }

    public static void ChooseNextLevel()
    {
     //   SceneManager.LoadSceneAsync(levelInfo.possibleLevels[1]);
    }
}
