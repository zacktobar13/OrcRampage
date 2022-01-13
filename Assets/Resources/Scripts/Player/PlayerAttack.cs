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
    int currentWeaponIndex = 0;

    void Start()
    {
        currentWeapon = GetComponentInChildren<Weapon>();
        weapons[0] = currentWeapon;
    }

    void Update()
    {
        if (PlayerInput.attack)
        {
            currentWeapon.Shoot();
        }

        if (PlayerInput.changeToFirstWeapon)
        {
            ChangeToWeapon(0);
        }

        if (PlayerInput.changeToSecondWeapon)
        {
            ChangeToWeapon(1);
        }

        if (PlayerInput.interact)
        {
            pressedInteractThisFrame = true;
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

    public void ChangeToWeapon(int index)
    {
        // Already on the requested index
        if (currentWeaponIndex == index)
            return;

        // No weapon at the requested index
        if (!weapons[index])
            return;

        // Disable current weapon, switch to other
        currentWeapon.gameObject.SetActive(false);
        weapons[index].gameObject.SetActive(true);
        currentWeaponIndex = index;
        currentWeapon = weapons[currentWeaponIndex];
    }

    public void PickupWeapon(GameObject weaponObject)
    {
        Weapon weapon = weaponObject.GetComponent<Weapon>();
        Debug.Assert(weapon);

        // If we already have the weapon we're picking up, just add ammo
        if (weapons[0] && weapons[0] == weapon)
        {
            weapons[0].ammoCount += weapon.ammoCount;
            Destroy(weapon.gameObject);
            return;
        }
        else if (weapons[1] && weapons[1] == weapon)
        {
            weapons[1].ammoCount += weapon.ammoCount;
            Destroy(weapon.gameObject);
            return;
        }

        // We don't already have the weapon, so either add it to an empty slot or swap it with current
        if (!weapons[0])
        {
            weapon.PickupWeapon(gameObject);
            weapons[0] = weapon;
            weapon.gameObject.SetActive(false);
            return;
        }
        else if (!weapons[1])
        {
            weapon.PickupWeapon(gameObject);
            weapons[1] = weapon;
            weapon.gameObject.SetActive(false);
            return;
        }
        else
        {
            int currentWeaponIndex = currentWeapon == weapons[0] ? 0 : 1;
            weapons[currentWeaponIndex].DropWeapon();
            weapon.PickupWeapon(gameObject);
            weapons[currentWeaponIndex] = weapon;
            currentWeapon = weapon;
            return;
        }
    }

    // OnTriggerStay is run on the physics update, this variable helps it feel more responsive
    bool pressedInteractThisFrame = false;
    bool interactedThisFrame = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        pressedInteractThisFrame = false;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        pressedInteractThisFrame = false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (interactedThisFrame)
            return;

        if (pressedInteractThisFrame)
        {
            if (collision.gameObject)
            {
                GameObject potentialWeapon = collision.gameObject;
                interactedThisFrame = true;
                Weapon weapon = potentialWeapon.GetComponent<Weapon>();
                if (weapon)
                {
                    PickupWeapon(collision.gameObject);
                }
            }
        }

        pressedInteractThisFrame = false;
    }

    private void LateUpdate()
    {
        interactedThisFrame = false;
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
