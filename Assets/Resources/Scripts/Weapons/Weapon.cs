using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("Weapon Attributes")]
    public WeaponIdentifier weaponID;
    public bool isAutomatic;
    public int attackPower;
    public int critChance;
    public int critPower;
    public float attacksPerSecond;

    [Header("Ranged Weapon")]
  //  public float projectileSpeed;
    public float projectileSpreadAmount;
    public int ammoCount;
    public GameObject projectile;
    public GameObject bulletCasing;

    [Header("Visual Effects")]
    public Sprite notFiringSprite;
    public Sprite firingSprite;
    public SpriteRenderer sprite;
    public GameObject visualEffect;
    public Transform visualEffectsSpawnPoint;
    public float visualsCooldown;
    public GameObject shadow;

    [Header("Audio Effects")]
    public AudioSource audioSource;
    public AudioClip shootSound;
    public AudioClip emptySound;

    private Rigidbody2D rigidBody;

    protected Transform projectileSpawn;
    protected float lastAttackTime;
    protected bool isOnPlayer;

    private Vector2 randomDropDir;
    private float droppedWeaponMovementSpeed = .01f;
	private Coroutine moveCoroutine;
    private Animator anim;

	private void Awake()
	{
        anim = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody2D>();
    }

	private void OnEnable()
    {
        sprite.sprite = notFiringSprite;
        projectileSpawn = transform.Find("BulletSpawn");
        isOnPlayer = transform.parent != null && transform.parent.CompareTag("Player");
    }

	public virtual void Attack()
    {
        if (!CanAttack())
            return;

        if (!HasAmmo())
        {
            FireEmpty();
            return;
        }

        StartCoroutine(VisualEffects());
        SpawnProjectile();
        PlayAttackSound();
        ammoCount -= 1;
        lastAttackTime = Time.time;
        return;
    }

    public virtual void FireEmpty()
    {
        audioSource.PlayOneShot(emptySound);
    }

    public virtual void SpawnProjectile()
    {
        if (bulletCasing)
            Instantiate(bulletCasing, new Vector2(transform.position.x, transform.position.y - 3.5f), Quaternion.Euler(0f, 0f, 0f));

        GameObject projectileSpawned = Instantiate(projectile, projectileSpawn.position, Quaternion.identity);
        Projectile projectileInfo = projectileSpawned.GetComponent<Projectile>();
        bool isCritical = RollCrit();
        projectileInfo.projectileDamage = CalculateDamage(isCritical);
        projectileInfo.isCriticalHit = isCritical;
        //projectileInfo.movementSpeed = projectileSpeed;
        projectileInfo.shotByPlayer = isOnPlayer;

        projectileInfo.SetProjectileRotation(transform.eulerAngles.z);
    }

    public virtual void PlayAttackSound()
    {
        audioSource.pitch = Random.Range(.9f, 1.2f);
        audioSource.PlayOneShot(shootSound);
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
        anim.Play("DropWeapon", -1, 0);
        transform.localScale = Vector3.one;
        moveCoroutine = StartCoroutine(MoveOnDrop());
        transform.parent = null;
        GetComponent<BoxCollider2D>().enabled = true;
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
        transform.parent = newOwner.transform;
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<RotateWeapon>().enabled = true;
        isOnPlayer = transform.parent != null && transform.parent.CompareTag("Player");

        // Reset weapon's sprite position that gets changed from the idle floating animation.
        sprite.gameObject.transform.localPosition = Vector3.zero;
    }

    //Checks to see if a shot will be a crit
    public virtual bool RollCrit()
    {
        return ( critChance >= Random.Range( 0, 100 ) );
    }

    //All attacks roll a random range to create a spread of damage.
    public virtual int CalculateDamage(bool isCrit)
    {
        if (isCrit)
        {
            return (int)Random.Range((attackPower * critPower) * 1f, (attackPower * critPower) * 1.25f);
        }
        else
        {
            return (int)Random.Range(attackPower * .8f, attackPower * 1.25f);
        }
    }

    public virtual bool HasAmmo()
    {
        return ammoCount > 0;
    }

    public virtual bool CanAttack()
    {
        return Time.time > lastAttackTime + (1 / attacksPerSecond);
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
