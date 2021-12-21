public class RicochetAffix : BaseAffix
{
    void Start()
    {
        PlayerShoot.onShoot += OnShoot;
    }

    public void OnShoot(PlayerShoot playerShoot, Projectile projectile)
    {
        projectile.isRicochet = true;
        projectile.ricochetCount = 5;
    }
    void OnDestroy()
    {
        PlayerShoot.onShoot -= OnShoot;
    }
}
