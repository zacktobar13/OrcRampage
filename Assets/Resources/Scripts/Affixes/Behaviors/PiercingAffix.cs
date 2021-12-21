public class PiercingAffix : BaseAffix
{
    void Start()
    {
        PlayerShoot.onShoot += OnShoot;
    }

    public void OnShoot(PlayerShoot playerShoot, Projectile projectile)
    {
        projectile.isPiercing = true;
    }

    void OnDestroy()
    {
        PlayerShoot.onShoot -= OnShoot;
    }
}
