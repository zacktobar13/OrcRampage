using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public delegate void OnBowDraw(float time);
    public static event OnBowDraw onBowDraw;

    public delegate void OnBowReady();
    public static event OnBowReady onBowReady;

    public delegate void OnShoot(PlayerShoot playerShoot, Projectile projectileSpawned);
    public static event OnShoot onShoot;

    public delegate void OnFire(PlayerShoot playerShoot);
    public static event OnFire onFire;

    public delegate void OnDrawEnd(PlayerShoot playerShoot);
    public static event OnDrawEnd onDrawEnd;

    public AudioSource audioSource;
    public GameObject arrowReleaseBarGameObject;
    public ArrowReleaseBar arrowReleaseBar;
    public float criticalFieldStart;
    public float criticalFieldEnd;

    public float shotCooldown;
    float lastShotTime;

    public AudioClip drawBowSound;
    public bool currentlyDrawing = false;

    public float maxDrawTime;
    bool bowReady;

    private void OnDisable()
    {
        // If we were drawing before we're disabled, make sure we signal we're stopping
        if (currentlyDrawing && onDrawEnd != null)
            onDrawEnd(this);

        arrowReleaseBarGameObject.SetActive(false);
        currentlyDrawing = false;

    }

    void Update()
    {
        if (Time.time < lastShotTime + shotCooldown)
        {
            return;
            // TODO fail sound effect.
        }

        if (!bowReady)
        {
            bowReady = true;

            if (onBowReady != null)
                onBowReady();
        }

        if (PlayerInput.shoot)
        {
            ShootBow(0f, arrowReleaseBar.canCriticalFire);
            lastShotTime = Time.time;

            if (onFire != null)
                onFire(this);
        }
    }

    public void ShootBow(float offsetScalar, bool isCritical)
    {
        Vector3 offsetDirection = Vector3.Cross(gameObject.transform.right, gameObject.transform.forward).normalized;
        Vector3 arrowPosition = transform.position + offsetDirection * offsetScalar;
        GameObject arrow = Instantiate(StaticResources.arrow, arrowPosition, transform.rotation);
        Projectile projectile = arrow.GetComponent<Projectile>();
        projectile.projectileDamage = Mathf.CeilToInt(projectile.projectileDamage);

        if (isCritical)
        {
            projectile.projectileDamage *= 2;
            projectile.isCriticalHit = true;
        }

        if (onShoot != null)
            onShoot(this, projectile);
    }

    float DamageScalarFromDrawDuration()
    {
        float drawProgress = arrowReleaseBar.sliderPercentToFinish;
        return drawProgress;
    }

    float MovementScalarFromDrawDuration()
    {
        float drawProgress = arrowReleaseBar.sliderPercentToFinish;
        return drawProgress;
    }
}
