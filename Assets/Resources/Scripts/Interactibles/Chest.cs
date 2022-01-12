using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Chest : MonoBehaviour
{
    public int openCost;
    
    Animator animator;
    bool isOpen;

    public TextMeshProUGUI costText;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

	private void Update()
	{
        if (!isOpen && costText.isActiveAndEnabled && PlayerInput.interact)
        {
            AttemptOpen(0);
        }
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
        if (!isOpen)
        {
            costText.enabled = collision.tag == "Player"; 
        }
    }

	private void OnTriggerExit2D(Collider2D collision)
	{
        if (!isOpen)
        {
            costText.enabled = false;
        }
    }

	public void AttemptOpen(int playerCurrency)
    {
        if (playerCurrency >= openCost)
        {
            animator.SetBool("isOpen", true);
            isOpen = true;
            costText.enabled = false;
        }
    }
}
