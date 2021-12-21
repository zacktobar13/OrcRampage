using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    //[Header("Weapon Attributes")]
    //public WeaponIdentifier weaponID;
    //public bool isAutomatic;
    //public int attackPower;
    //public int critChance;
    //public int critPower;
    //public int clipSize;
    //public int currentClip;
    //public float reloadTime;
    //public float shotsPerSecond;
    //public float projectileSpeed;
    //public float projectileSpreadAmount;
    //public float knockbackSpeed;
    //public float knockbackTime;

    //public float criticalReloadBegin;
    //public float criticalReloadEnd;

    //[Header("Visual Effects")]
    //public GameObject bulletCasing;
    //public GameObject muzzleFlash;
    //public Sprite notFiringSprite;
    //public Sprite firingSprite;
    //public SpriteRenderer sprite;

    //public float visualsCooldown;

    //public GameObject projectile;
    //public GameObject projectileSpawn;

    //[Header("Audio Effects")]
    //public AudioSource audioSource;
    //public AudioClip shootSound;
    //public AudioClip emptySound;
    //public AudioClip reloadSound;

    //[HideInInspector] public GameObject projectileSpawned;
    //Projectile projectileInfo;

    //private bool criticalHit;
    //public GameObject reloadBarGameObject;
    //ReloadBar reloadBar;

    //[HideInInspector] public float lastShotTime;

    //[HideInInspector] public bool isReloading;


    //private void OnEnable()
    //{
    //    muzzleFlash.SetActive(false);
    //    sprite.sprite = notFiringSprite;

    //  //  ReloadBar.onReload += OnReload;
    //    //PlayerShootOld.onFailedReload += OnFailedReload;
    //    //PlayerShootOld.onCriticalReload += OnCriticalReload;
    //}

    //private void Awake()
    //{  
    //    currentClip = clipSize;
    //}

    //private void Start()
    //{
    //    reloadBar = reloadBarGameObject.GetComponent<ReloadBar>();
    //}

    //public virtual IEnumerator Shoot()
    //{
    //    StartCoroutine( VisualEffects() );
    //    SpawnProjectile(true);
    //    currentClip -= 1;
    //    yield return new WaitForSeconds( 1f / shotsPerSecond );
    //}

    //public virtual IEnumerator FireEmpty()
    //{
    //    audioSource.PlayOneShot(emptySound);
    //    yield return new WaitForSeconds(1f / shotsPerSecond);
    //}

    //public virtual void SpawnProjectile(bool makeNoise)
    //{
    //    Instantiate(bulletCasing, new Vector2(transform.position.x, transform.position.y - 3.5f), Quaternion.Euler(0f, 0f, 0f));
    //    projectileSpawned = Instantiate(projectile, projectileSpawn.transform.position, Quaternion.identity);
    //    projectileInfo = projectileSpawned.GetComponent<Projectile>();
    //    projectileInfo.projectileDamage = CalculateBulletDamage();
    //    projectileInfo.criticalHit = criticalHit;
    //    projectileInfo.movementSpeed = projectileSpeed;
    //    projectileInfo.spread = projectileSpreadAmount;
    //    if (makeNoise)
    //    {
    //        audioSource.pitch = Random.Range(.9f, 1.2f);
    //        audioSource.PlayOneShot(shootSound);
    //    }
    //}

    //public virtual IEnumerator VisualEffects()
    //{
    //    sprite.sprite = firingSprite;
    //    muzzleFlash.SetActive( true );
    //    yield return new WaitForSeconds( visualsCooldown );
    //    muzzleFlash.SetActive( false );
    //    sprite.sprite = notFiringSprite;
    //}

    //public void DropWeapon()
    //{
    //    StopAllCoroutines();
    //}

    //// Checks to see if a shot will be a crit
    //public virtual bool RollCrit()
    //{
    //    return ( critChance >= Random.Range( 0, 100 ) );
    //}

    //// All attacks roll a random range to create a spread of damage.
    //public virtual int CalculateBulletDamage()
    //{
    //    if ( RollCrit() )
    //    {
    //        criticalHit = true;
    //        return (int)Random.Range((attackPower * critPower) * 1f, (attackPower * critPower) * 1.25f);
    //    }
    //    else
    //    {
    //        criticalHit = false;
    //        return (int)Random.Range(attackPower * .8f, attackPower * 1.25f);
    //    }
    //}

    //public bool CanShoot()
    //{
    //    return currentClip > 0 && !isReloading;
    //}

    //public void StartReload() // Initiates our reload bar.
    //{
    //    reloadBarGameObject.SetActive(true);
    //  //  reloadBar.StartReload(reloadTime);
    //    isReloading = true;
    //}
    //public bool CanReload()
    //{
    //    return currentClip < clipSize;
    //}

    //public void OnReload(Weapon weapon) // Delegate starts in ReloadBar because that's where logic for slider reaching the end is.
    //{
    //    audioSource.pitch = Random.Range(.9f, 1.2f);
    //    audioSource.PlayOneShot(reloadSound);
    //    currentClip = clipSize;
    //    isReloading = false;
    //}

    //public void OnFailedReload(Weapon weapon)
    //{
    //    // Play fail sfx.
    //}

    //public void OnCriticalReload(Weapon weapon)
    //{
    //    audioSource.pitch = Random.Range(1f, 1.2f);
    //    audioSource.PlayOneShot(reloadSound, 1f);
    //    currentClip = clipSize;
    //    isReloading = false;
    //}
}
