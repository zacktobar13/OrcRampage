using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("Weapon Attributes")]
    public WeaponIdentifier weaponID;
    public bool isAutomatic;
    public int attackDamage;
    public float baseAttacksPerSecond;

    [Header("Ranged Weapon")]
  //  public float projectileSpeed;
    public float projectileSpreadAmount;
    public int maxAmmo;
    public int currentAmmo;
    public GameObject projectile;

    [Header("Visual Effects")]
    public Sprite notFiringSprite;
    public Sprite firingSprite;
    public SpriteRenderer sprite;
    public GameObject visualEffect;
    public Transform visualEffectsSpawnPoint;
    public float visualsCooldown;
    public GameObject shadow;

    [Header("Audio Effects")]
    public AudioClip shootSound;
    public AudioClip emptySound;

    private Rigidbody2D rigidBody;

    protected Transform projectileSpawn;
    protected float lastAttackTime;
    
    protected bool isOnPlayer;
    protected PlayerStats playerStats;

    private Vector2 randomDropDir;
    private float droppedWeaponMovementSpeed = .01f;
	private Coroutine moveCoroutine;
    private Animator anim;

    TimeManager timeManager;

	private void Awake()
	{
        anim = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody2D>();
    }

    public void SetTimeManager(TimeManager tm)
    {
        timeManager = tm;
    }

    private void OnEnable()
    {
        sprite.sprite = notFiringSprite;
        projectileSpawn = transform.Find("Projectile Spawn");
        isOnPlayer = transform.parent != null && transform.parent.CompareTag("Player");
        if (isOnPlayer)
            playerStats = GetComponentInParent<PlayerStats>();
    }

	public virtual Projectile Attack(bool isCritical, float offset, bool playSound, bool ignoreCooldown=false)
    {
        if (timeManager.IsGamePaused())
            return null;

        if (!ignoreCooldown && !CanAttack())
            return null;

        StartCoroutine(VisualEffects());
        Projectile projectileSpawned = SpawnProjectile(isCritical, offset);

        if (playSound)
            PlayAttackSound();

        if (!ignoreCooldown)
            lastAttackTime = Time.time;

        return projectileSpawned;
    }

    public virtual Projectile Attack(bool isCritical)
    {
        if (timeManager.IsGamePaused())
            return null;
        return Attack(isCritical, 0, true);
    }

    public virtual void FireEmpty()
    {
        SoundManager.PlayOneShot(transform.position, emptySound);
    }

    public virtual Projectile SpawnProjectile(bool isCritical, float offset)
    {
        if (timeManager.IsGamePaused())
            return null;

        if (!projectile)
            return null;

        GameObject projectileSpawned = Instantiate(projectile, projectileSpawn.position, Quaternion.identity);
        Projectile projectileInfo = projectileSpawned.GetComponent<Projectile>();
        projectileInfo.projectileDamage = attackDamage;
        projectileInfo.isCriticalHit = isCritical;
        //projectileInfo.movementSpeed = projectileSpeed;
        projectileInfo.shotByPlayer = isOnPlayer;
        projectileInfo.rotationOffset = offset;

        projectileInfo.SetProjectileRotation(transform.eulerAngles.z);
        return projectileInfo;
    }

    public virtual void PlayAttackSound()
    {
        SoundManager.PlayOneShot(transform.position, shootSound, new SoundManagerArgs(true, shootSound.name));
    }

    public virtual IEnumerator VisualEffects()
    {
        if (visualEffect)
        {
            GameObject visuals = Instantiate(visualEffect, visualEffectsSpawnPoint.position, visualEffectsSpawnPoint.rotation);
            visuals.GetComponent<PowerTools.SpriteAnim>().SetSpeed(1/visualsCooldown);
        }

        sprite.sprite = firingSprite;
        yield return new WaitForSeconds(visualsCooldown);
        sprite.sprite = notFiringSprite;
    }

    IEnumerator MoveOnDrop()
    {
        randomDropDir = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        float targetTime = Time.time + 1f;
        while (Time.time <= targetTime)
        {
            yield return new WaitForFixedUpdate();
            rigidBody.MovePosition(rigidBody.position + randomDropDir * droppedWeaponMovementSpeed);
        }
    }


    public void DropWeapon()
    {
        StopAllCoroutines();
        shadow.SetActive(true);
        anim.enabled = true;
        sprite.sprite = notFiringSprite;
        anim.Play("DropWeapon", -1, 0);
        transform.localScale = Vector3.one;
        moveCoroutine = StartCoroutine(MoveOnDrop());
        transform.parent = null;
        //GetComponent<BoxCollider2D>().enabled = true;
        GetComponent<RotateWeapon>().enabled = false;
        transform.rotation = Quaternion.identity;
        isOnPlayer = false;
    }

    public void PickupWeapon(GameObject newOwner)
    {
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }

        shadow.SetActive(false);
        anim.enabled = false;
       // transform.parent = newOwner.Gametransform;
        //GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<RotateWeapon>().enabled = true;
        isOnPlayer = transform.parent != null && transform.parent.CompareTag("Player");

        // Reset weapon's sprite position that gets changed from the idle floating animation.
        sprite.gameObject.transform.localPosition = Vector3.zero;
    }

    public virtual bool HasAmmo()
    {
        return true;
        //return currentAmmo > 0;
    }

    public bool GiveAmmo(int amount)
    {
        if (currentAmmo == maxAmmo)
            return false;

        currentAmmo = Mathf.Min(maxAmmo, currentAmmo + amount);
        return true;
    }

    public virtual bool CanAttack()
    {
        float attacksPerSecond = baseAttacksPerSecond;
        if (isOnPlayer)
            attacksPerSecond = playerStats.CalculateAttackSpeed(baseAttacksPerSecond);

        return isActiveAndEnabled && Time.time > lastAttackTime + (1 / attacksPerSecond);
    }

    public static bool operator ==(Weapon a, Weapon b)
    {
        return a.weaponID == b.weaponID;
    }

    public static bool operator !=(Weapon a, Weapon b)
    {
        return a.weaponID != b.weaponID;
    }

    public override bool Equals(object other)
    {
        return base.Equals(other);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}
