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
    public int ammoCount;
    public float shotsPerSecond;
    public float projectileSpeed;
    public float projectileSpreadAmount;
    public float knockbackSpeed;
    public float knockbackTime;

    [Header("Visual Effects")]
    public GameObject bulletCasing;
    public Sprite notFiringSprite;
    public Sprite firingSprite;
    public SpriteRenderer sprite;

    public float visualsCooldown;

    public GameObject projectile;
    Transform projectileSpawn;

    [Header("Audio Effects")]
    public AudioSource audioSource;
    public AudioClip shootSound;
    public AudioClip emptySound;

    [HideInInspector] public GameObject projectileSpawned;
    Projectile projectileInfo;

    private bool criticalHit;

    [HideInInspector] public float lastShotTime;

    [HideInInspector] public bool isReloading;

    Vector2 randomDropDir;
    float droppedWeaponMovementSpeed = 5f;
	Coroutine moveCoroutine;
    Animator anim;
    public GameObject shadow;

	private void Awake()
	{
        anim = GetComponent<Animator>();
    }

	private void OnEnable()
    {
        sprite.sprite = notFiringSprite;
        projectileSpawn = transform.Find("BulletSpawn");
    }

	public virtual void Shoot()
    {
        if (!CanShoot())
            return;

        if (!HasAmmo())
        {
            FireEmpty();
            return;
        }

        StartCoroutine(VisualEffects());
        SpawnProjectile(true);
        ammoCount -= 1;
        return;
    }

    public virtual void FireEmpty()
    {
        audioSource.PlayOneShot(emptySound);
    }

    IEnumerator MoveOnDrop()
    {
        randomDropDir = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        float targetTime = Time.time + 1f;
        while (Time.time <= targetTime)
        {
            yield return new WaitForEndOfFrame();
            transform.Translate(randomDropDir * Time.deltaTime * droppedWeaponMovementSpeed);
        }
    }

    public virtual void SpawnProjectile(bool makeNoise)
    {
        Instantiate(bulletCasing, new Vector2(transform.position.x, transform.position.y - 3.5f), Quaternion.Euler(0f, 0f, 0f));
        projectileSpawned = Instantiate(projectile, projectileSpawn.position, Quaternion.identity);
        projectileInfo = projectileSpawned.GetComponent<Projectile>();
        projectileInfo.projectileDamage = CalculateBulletDamage();
        projectileInfo.isCriticalHit = criticalHit;
        projectileInfo.movementSpeed = projectileSpeed;
        projectileInfo.spread = projectileSpreadAmount;

        //Vector2 weaponToMouse = PlayerInput.mousePosition - (Vector2)projectileSpawn.position;
        //float projectileRotation = Mathf.Atan2(weaponToMouse.y, weaponToMouse.x) * Mathf.Rad2Deg;
        projectileInfo.SetProjectileRotation(transform.eulerAngles.z);

        if (makeNoise)
        {
            audioSource.pitch = Random.Range(.9f, 1.2f);
            audioSource.PlayOneShot(shootSound);
        }
    }

    public virtual IEnumerator VisualEffects()
    {
        sprite.sprite = firingSprite;
        yield return new WaitForSeconds(visualsCooldown);
        sprite.sprite = notFiringSprite;
    }

    public void DropWeapon()
    {
        StopAllCoroutines();
        shadow.SetActive(true);
        anim.enabled = true;
        anim.SetBool("onGround", false);
        transform.localScale = Vector3.one;
        moveCoroutine = StartCoroutine(MoveOnDrop());
        transform.parent = null;
        GetComponent<BoxCollider2D>().enabled = true;
        GetComponent<RotateArm>().enabled = false;
        transform.rotation = Quaternion.identity;
    }

    public void PickupWeapon(GameObject newOwner)
    {
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }

        shadow.SetActive(false);
        anim.enabled = false;
        sprite.gameObject.transform.localPosition = Vector3.zero;
        transform.parent = newOwner.transform;
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<RotateArm>().enabled = true;
    }

    //Checks to see if a shot will be a crit
    public virtual bool RollCrit()
    {
        return ( critChance >= Random.Range( 0, 100 ) );
    }

    //All attacks roll a random range to create a spread of damage.
    public virtual int CalculateBulletDamage()
    {
        if ( RollCrit() )
        {
            criticalHit = true;
            return (int)Random.Range((attackPower * critPower) * 1f, (attackPower * critPower) * 1.25f);
        }
        else
        {
            criticalHit = false;
            return (int)Random.Range(attackPower * .8f, attackPower * 1.25f);
        }
    }

    public void Anim_OnGround()
    {
        anim.SetBool("onGround", true);
    }

    public bool HasAmmo()
    {
        return ammoCount > 0;
    }

    public bool CanShoot()
    {
        return Time.time > lastShotTime + (1 / shotsPerSecond);
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
