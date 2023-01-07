using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Pool;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] GameObject weapon;
    [SerializeField] GameObject projectile;
    [SerializeField] float baseAttacksPerSecond;
    [SerializeField] AudioClip attackSound;

    public delegate void OnShoot(PlayerAttack playerAttack);
    public event OnShoot onPlayerShoot;

    public delegate void OnProjectileSpawned(PlayerAttack playerAttack, Projectile projectileSpawned);
    public event OnProjectileSpawned onProjectileSpawned;

    protected ObjectPool<GameObject> projectilePool;
 
    PlayerStats playerStats;
    GameplayUI gameplayUI;
    TimeManager timeManager;
    PoolManager poolManager;
    
    Transform projectilePoolParent;
	Transform projectileSpawnPoint;

	float lastAttackTime;

    bool isAttacking;
    bool isAutomatic = true;

    void Start()
    {
        GameObject gameManagement = GameObject.Find("Game Management");

        gameplayUI = gameManagement.GetComponent<GameplayUI>();
        timeManager = gameManagement.GetComponent<TimeManager>();
        poolManager = gameManagement.GetComponent<PoolManager>();
        projectilePool = poolManager.GetObjectPool(projectile);
        projectileSpawnPoint = transform.Find("Projectile Spawn Point");

        playerStats = GetComponent<PlayerStats>();
        string hierarchyName = "Game Management/" + projectile.name + " Pool";
        projectilePoolParent = GameObject.Find(hierarchyName).transform;
    }

    void Update()
    {
        if (timeManager.IsGamePaused())
            return;

        if ((PlayerInput.attack || (isAutomatic && PlayerInput.holdingAttack)) && CanAttack())
        {
            Attack();
            lastAttackTime = Time.time;
        }
        else
        {
            isAttacking = false;
        }
    }

    public bool IsAttacking()
    {
        return isAttacking;
    }

    // Forces an attack that ignores cooldown, doesn't play a sound, and doesn't trigger OnShoot event
    public void ForceAttack(float offset)
    {
        Attack(offset, false, true);
    }

    private void Attack()
    {
        isAttacking = true;

        if (onPlayerShoot != null)
            onPlayerShoot(this);

        Attack(0, true);
    }

    private void Attack(float offset, bool playSound, bool ignoreCooldown=false)
    {
        bool isCritical = playerStats.RollCritical();
        int damage = playerStats.CalculateDamage(isCritical);
        Projectile projectileSpawned = SpawnProjectile(damage, isCritical, offset);

        if (projectileSpawned && onProjectileSpawned != null)
            onProjectileSpawned(this, projectileSpawned);

        SoundManager.PlayOneShot(transform.position, attackSound, new SoundManagerArgs("MageShoot"));
    }

    public virtual Projectile SpawnProjectile(int damage, bool isCritical, float offset)
    {
        if (timeManager.IsGamePaused())
            return null;

        if (!projectile)
            return null;

        Vector2 directionToMouse = (PlayerInput.mousePosition - (Vector2)transform.position).normalized;
        Vector2 projectileSpawnOffset = directionToMouse * 2;
        Vector2 projectileDirection = Utility.Rotate(directionToMouse, offset);

        GameObject projectileSpawned = projectilePool.Get();
        Projectile projectileInfo = projectileSpawned.GetComponent<Projectile>();
        projectileInfo.movementDirection = projectileDirection;
        projectileInfo.shotByPlayer = true;
        projectileSpawned.transform.position = (Vector2)projectileSpawnPoint.position + projectileSpawnOffset;
        projectileSpawned.transform.parent = projectilePoolParent;

        projectileInfo.SetMyPool(projectilePool);
        projectileInfo.projectileDamage = damage;
        projectileInfo.isCriticalHit = isCritical;
        projectileInfo.SetProjectileRotation(Mathf.Rad2Deg * Mathf.Atan2(projectileDirection.y, projectileDirection.x));
        return projectileInfo;
    }

    public bool CanAttack()
    {
        if (!isActiveAndEnabled)
            return false;

        float attacksPerSecond = playerStats.CalculateAttackSpeed(baseAttacksPerSecond);
        return Time.time > lastAttackTime + (1 / attacksPerSecond);
    }

    public void DisableWeapon()
    {
        weapon.SetActive(false);
    }

    public void EnableWeapon()
    {
        weapon.SetActive(true);
    }

    private void Awake()
    {
        SceneManager.sceneLoaded += ReInitialize;
    }

    public void ReInitialize(Scene scene, LoadSceneMode lsm)
    {
        Start();
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= ReInitialize;
    }
}
