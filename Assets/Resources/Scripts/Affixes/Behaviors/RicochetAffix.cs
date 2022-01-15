public class RicochetAffix : BaseAffix
{
    void Start()
    {
        PlayerAttack.onShoot += OnShoot;
    }

    public void OnShoot(PlayerAttack playerShoot, Projectile projectile)
    {
        //projectile.isRicochet = true;
        //projectile.ricochetCount = 5;
    }
    void OnDestroy()
    {
        PlayerAttack.onShoot -= OnShoot;
    }
}
