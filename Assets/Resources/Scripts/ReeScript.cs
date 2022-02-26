using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReeScript : MonoBehaviour
{
    Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Ree()
    {
        animator.SetBool("Animating", true);
    }

	private void FixedUpdate()
	{
        Debug.Log("Fixed Update");
	}

	private void Update()
	{
        if (Input.GetKeyDown(KeyCode.P))
        {
            Time.timeScale = (Time.timeScale + 1) % 2;            
        }

        Debug.Log("Update");
    }
}
