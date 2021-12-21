using UnityEngine;

public class ExplosiveArrowAffix : BaseAffix
{
    void Start()
    {
        PlayerShoot.onShoot += OnShoot;
    }

    public void OnShoot(PlayerShoot playerShoot, Projectile projectile)
    { 
        projectile.hitEffect = StaticResources.explosiveArrowExplosion;
    }
    void OnDestroy()
    {
        PlayerShoot.onShoot -= OnShoot;
    }
}
