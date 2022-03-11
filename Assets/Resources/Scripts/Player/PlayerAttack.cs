using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public int baseDamage;
    PlayerStats playerStats;

    public delegate void OnShoot(PlayerAttack playerAttack);
    public event OnShoot onPlayerShoot;

    public delegate void OnProjectileSpawned(PlayerAttack playerAttack, Projectile projectileSpawned);
    public event OnProjectileSpawned onProjectileSpawned;

    GameplayUI gameplayUI;

    Weapon[] weapons = new Weapon[2];
    [SerializeField] Weapon currentWeapon;
    int currentWeaponIndex = 0;

    TimeManager timeManager;

    void Start()
    {
        gameplayUI = GameObject.Find("Gameplay UI").GetComponent<GameplayUI>();
        timeManager = GameObject.Find("Game Management").GetComponent<TimeManager>();
        
        playerStats = GetComponent<PlayerStats>();

        currentWeapon = GetComponentInChildren<Weapon>();
        weapons[0] = currentWeapon;
        currentWeapon.PickupWeapon(gameObject);
    }

    void Update()
    {
        if (timeManager.IsGamePaused())
            return;

        if (PlayerInput.attack || (currentWeapon.isAutomatic && PlayerInput.holdingAttack))
        {
            Attack();
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

    // Forces an attack that ignores cooldown, doesn't play a sound, and doesn't trigger OnShoot event
    public void ForceAttack(float offset)
    {
        Attack(offset, false, true);
    }

    private void Attack()
    {
        if (onPlayerShoot != null && CanWeaponAttack())
            onPlayerShoot(this);

        Attack(0, true);
    }

    private void Attack(float offset, bool playSound, bool ignoreCooldown=false)
    {
        bool isCritical = playerStats.RollCritical();
        currentWeapon.attackDamage = playerStats.CalculateDamage(baseDamage, isCritical);
        Projectile projectileSpawned = currentWeapon.Attack(isCritical, offset, playSound, ignoreCooldown);

        if (projectileSpawned && onProjectileSpawned != null)
            onProjectileSpawned(this, projectileSpawned);
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
            bool receivedAmmo = weapons[0].GiveAmmo(weapon.currentAmmo);
            if (receivedAmmo)
            {
                Destroy(weapon.gameObject);
            }
            return;
        }
        else if (weapons[1] && weapons[1] == weapon)
        {
            bool receivedAmmo = weapons[1].GiveAmmo(weapon.currentAmmo);
            if (receivedAmmo)
            {
                Destroy(weapon.gameObject);
            }
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

    bool attemptedInteracts = false;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (interactedThisFrame)
            return;

        if (pressedInteractThisFrame)
        {
            if (collision.gameObject)
            {
                GameObject potentialWeapon = collision.gameObject;
                Weapon weapon = potentialWeapon.GetComponent<Weapon>();
                if (weapon)
                {
                    interactedThisFrame = true;
                    PickupWeapon(collision.gameObject);
                }
            }
            attemptedInteracts = true;
        }
    }

    private void LateUpdate()
    {
        interactedThisFrame = false;
        if (attemptedInteracts)
        {
            attemptedInteracts = false;
            pressedInteractThisFrame = false;
        }
    }

    public Weapon GetCurrentWeapon()
    {
        return currentWeapon;
    }

    public float GetWeaponRotation()
    {
        return currentWeapon.transform.rotation.eulerAngles.z;
    }

    public bool CanWeaponAttack()
    {
        return currentWeapon.CanAttack();
    }

    public void DisableWeapon()
    {
        currentWeapon.gameObject.SetActive(false);
    }

    public void EnableWeapon()
    {
        currentWeapon.gameObject.SetActive(true);
    }
}
