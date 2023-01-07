using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Pool;

public class PlayerAttack : MonoBehaviour
{
    PlayerStats playerStats;
 
    public delegate void OnShoot(PlayerAttack playerAttack);
    public event OnShoot onPlayerShoot;

    public delegate void OnProjectileSpawned(PlayerAttack playerAttack, Projectile projectileSpawned);
    public event OnProjectileSpawned onProjectileSpawned;

    GameplayUI gameplayUI;

    [SerializeField] GameObject weapon;
    [SerializeField] GameObject projectile;
    [SerializeField] float baseAttacksPerSecond;
    float lastAttackTime;

    TimeManager timeManager;
    bool isAttacking;
    bool isAutomatic = true;

    protected ObjectPool<GameObject> projectilePool;
    PoolManager poolManager;
    Transform projectilePoolParent;

    void Start()
    {
        gameplayUI = GameObject.Find("Gameplay UI").GetComponent<GameplayUI>();
        timeManager = GameObject.Find("Game Management").GetComponent<TimeManager>();
        poolManager = GameObject.Find("Game Management").GetComponent<PoolManager>();
        projectilePool = poolManager.GetObjectPool(projectile);
        
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
        projectileSpawned.transform.right = projectileDirection;
        projectileInfo.movementDirection = projectileSpawned.transform.right;
        projectileInfo.shotByPlayer = true;
        projectileSpawned.transform.position = (Vector2)transform.position + projectileSpawnOffset;
        projectileSpawned.transform.rotation = Quaternion.identity;
        projectileSpawned.transform.parent = projectilePoolParent;

        projectileInfo.SetMyPool(projectilePool);
        projectileInfo.projectileDamage = damage;
        projectileInfo.isCriticalHit = isCritical;
        projectileInfo.SetProjectileRotation(transform.eulerAngles.z);
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
