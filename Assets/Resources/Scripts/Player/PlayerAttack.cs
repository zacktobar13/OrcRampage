using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public delegate void OnShoot(PlayerAttack playerAttack, Projectile projectileSpawned);
    public static event OnShoot onShoot;

    public delegate void OnPlayerAttack(PlayerAttack playerAttack);
    public static event OnPlayerAttack onAttack;

    Weapon[] weapons = new Weapon[2];
    [SerializeField] Weapon currentWeapon;

    public bool currentlyDrawing = false;

    private void OnDisable()
    {
        currentlyDrawing = false;
    }

    void Update()
    {

        if (PlayerInput.attack)
        {
            transform.Find("Pistol").GetComponent<Weapon>().Shoot();
        }
    }

    public void ShootBow(float offsetScalar, bool isCritical)
    {
        Vector2 playerToMouse = PlayerInput.mousePosition - (Vector2)transform.position;
        GameObject arrow = Instantiate(StaticResources.arrow, transform.position, transform.rotation);
        Projectile projectile = arrow.GetComponent<Projectile>();

        float projectileRotation = Mathf.Atan2(playerToMouse.y, playerToMouse.x) * Mathf.Rad2Deg;
        projectile.SetProjectileRotation(projectileRotation);

        projectile.projectileDamage = Mathf.CeilToInt(projectile.projectileDamage);

        if (isCritical)
        {
            projectile.projectileDamage *= 2;
            projectile.isCriticalHit = true;
        }
        
    }

    public void OnAttack(Weapon weapon, Projectile projectile)
    {
        if (projectile)
        {
            if (onShoot != null)
                onShoot(this, projectile);
        }
        else
        {
            if (onAttack != null)
                onAttack(this);
        }
    }
}
