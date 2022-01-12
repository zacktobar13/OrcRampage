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

    void Start()
    {
        currentWeapon = GetComponentInChildren<Weapon>();
        weapons[0] = currentWeapon;
    }

    private void OnDisable()
    {
        currentlyDrawing = false;
    }

    void Update()
    {
        if (PlayerInput.attack)
        {
            currentWeapon.Shoot();
        }
    }

    public void DisableWeapon()
    {
        currentWeapon.gameObject.SetActive(false);
    }

    public void EnableWeapon()
    {
        currentWeapon.gameObject.SetActive(true);
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
