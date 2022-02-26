using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReeScript : MonoBehaviour
{
    Animator animator;
    TimeManager timeManager;

    void Start()
    {
        animator = GetComponent<Animator>();
        timeManager = GameObject.Find("Game Management").GetComponent<TimeManager>();
    }

    public void Ree()
    {
        animator.SetBool("Animating", true);
    }

	private void Update()
	{
        if (Input.GetKeyDown(KeyCode.P))
        {
            timeManager.TogglePause();            
        }
    }
}
