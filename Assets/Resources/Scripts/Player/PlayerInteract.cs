using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    //public //PlayerShootOld playerWeapons;
    public AudioSource audioSource;
    public AudioClip pickUpSFX;

    // may not actually be nearest since it's updated on OnTriggerEnter
    GameObject nearestInteractable;

    private void Update()
    {
        if (nearestInteractable && Input.GetButtonDown("Interact"))
        {
            BaseInteractible interact = nearestInteractable.GetComponent<BaseInteractible>();
            if (interact)
            {
                interact.OnPlayerInteract();
                return;
            }
        }
    }

    private void OnTriggerEnter2D( Collider2D collision )
    {
        if ( collision.gameObject.CompareTag( "Interactable" ) || collision.gameObject.CompareTag( "GatherableResource" ) )
            nearestInteractable = collision.gameObject;
    }

    private void OnTriggerExit2D( Collider2D collision )
    {
        if ( nearestInteractable == collision.gameObject )
            nearestInteractable = null;
    }
}
