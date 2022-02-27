public class PiercingAffix : BaseAffix
{
    PlayerAttack playerAttack;

    void Start()
    {
        playerAttack = GetComponent<PlayerAttack>();
        playerAttack.onProjectileSpawned += OnProjectileSpawned;
    }

    public void OnProjectileSpawned(PlayerAttack playerShoot, Projectile projectile)
    {
        projectile.isPiercing = true;
    }

    void OnDestroy()
    {
        playerAttack.onProjectileSpawned -= OnProjectileSpawned;
    }
}
