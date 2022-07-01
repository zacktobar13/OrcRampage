using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleBehavior : MonoBehaviour
{
    float gravity = .9f;
    float movementResistance = .999f;
    public GameObject particle;
    public Sprite[] restingSprites;
    public Sprite[] flyingSprites;
    public SpriteRenderer spriteRenderer;
    float verticalParticleSpeed;
    float verticalSpeed;
    float horizontalSpeed;
    AudioSource audioSource;
    public AudioClip hitGroundSFX;

    private void Start()
    {
        verticalParticleSpeed = Random.Range(10f, 15f);
        horizontalSpeed = Random.Range(-5f, 5f);
        verticalSpeed = Random.Range(-5f, 5f);
        particle.GetComponent<SpriteRenderer>().sprite = flyingSprites[(int)Random.Range(0f, flyingSprites.Length)];
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (particle)
        {
            // Particle moving towards ground.
            particle.transform.Translate((Vector2.up * verticalParticleSpeed) * Time.deltaTime);
            verticalParticleSpeed -= gravity;

            if (particle.transform.position.y <= gameObject.transform.position.y)
            {
                Destroy(particle);
                spriteRenderer.material = StaticResources.pixelSnap;
                spriteRenderer.sprite = restingSprites[(int)Random.Range(0f, restingSprites.Length)];
                SoundManager.PlayOneShot(transform.position, hitGroundSFX);
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
