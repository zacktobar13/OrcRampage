public class PiercingAffix : BaseAffix
{
    void Start()
    {
        PlayerAttack.onShoot += OnShoot;
    }

    public void OnShoot(PlayerAttack playerShoot, Projectile projectile)
    {
        //projectile.isPiercing = true;
    }

    void OnDestroy()
    {
        PlayerAttack.onShoot -= OnShoot;
    }
}
