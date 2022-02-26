using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCasing : MonoBehaviour
{
    float gravity = .9f;
    float movementResistance = .999f;
    public GameObject casing;
    public GameObject casingSprite;
    public GameObject shadowSprite;
    public SpriteRenderer spriteRenderer;
    float verticalCasingSpeed;
    float casingRotation;
    float rotationSpeed;
    float verticalSpeed;
    float horizontalSpeed;
    AudioSource audioSource;
    public AudioClip hitGroundSFX;

    private void Start()
    {
        verticalCasingSpeed = Random.Range(15f, 25f);
        horizontalSpeed = Random.Range(-10f, 10f);
        verticalSpeed = Random.Range(-5f, 5f);
        audioSource = GetComponent<AudioSource>();
        rotationSpeed = Random.Range(.05f, .1f);
    }

    void FixedUpdate()
    {
        if (casing)
        {
            // Particle moving towards ground.
            casingSprite.transform.Rotate(0f, 0f, casingRotation);
            shadowSprite.transform.Rotate(0f, 0f, casingRotation);
            casingRotation += rotationSpeed;
            casing.transform.Translate((Vector2.up * verticalCasingSpeed) * Time.deltaTime);
            verticalCasingSpeed -= gravity;

            if (casing.transform.position.y <= gameObject.transform.position.y)
            {
                Destroy(casing);

                spriteRenderer.material = StaticResources.pixelSnap;

                if (hitGroundSFX)
                {
                    audioSource.pitch = Random.Range(.7f, 1.3f);
                    audioSource.PlayOneShot(hitGroundSFX);
                }

                return;
            }

            // Horizontal Movement
            transform.Translate((Vector2.right * horizontalSpeed) * Time.deltaTime);
            horizontalSpeed *= movementResistance;

            // Vertical Movement
            transform.Translate((Vector2.up * verticalSpeed) * Time.deltaTime);
            verticalSpeed *= movementResistance;
        }
    }
}
