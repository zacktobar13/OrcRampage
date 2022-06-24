using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Portal : MonoBehaviour
{
    public float triggerDistance;
    string nextScene;
    
    private void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, PlayerManagement.player.transform.position);
        if (PlayerInput.interact && distanceToPlayer <= triggerDistance)
        {
            LoadNextScene();
        }
    }

    public void SetNextScene(string scene)
    {
        nextScene = scene;
    }

    public void LoadNextScene()
    {
        SceneManager.LoadScene(nextScene);
    }


}
