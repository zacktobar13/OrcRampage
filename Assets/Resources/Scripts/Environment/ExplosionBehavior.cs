using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PowerTools;
using EZCameraShake;
public class ExplosionBehavior : MonoBehaviour
{
    public GameObject creator;
    public GameObject player;
    public Vector2 damageDirection = Vector2.zero;
    public int damageAmount;
    public AudioSource audioSource;
    public CircleCollider2D circleCollider;
    public Light pointLight;
    public bool canHurtPlayer;

    private void Start()
    {
        CameraShaker.Instance.ShakeOnce(2f, 2f, .1f, 1f);
        audioSource.pitch = Random.Range(.5f, 2f);
    }

    private void FixedUpdate()
    {
        if (!audioSource.isPlaying)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !canHurtPlayer)
            return;

        if (collision.gameObject != creator && collision.isTrigger)
        {
            if (damageDirection == Vector2.zero)
            {
                damageDirection = collision.transform.position - transform.position;
            }

            DamageInfo damageInfo = new DamageInfo(damageAmount, false, damageDirection);
            collision.gameObject.SendMessage("ApplyDamage", damageInfo, SendMessageOptions.DontRequireReceiver);
            damageDirection = Vector2.zero;
        }
    }

    public void DisableSelf()
    {
        circleCollider.enabled = false;
    }

    public void DisableLight()
    {
        pointLight.enabled = false;
    }
}
