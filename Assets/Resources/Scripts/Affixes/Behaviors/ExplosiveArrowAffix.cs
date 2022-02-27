using UnityEngine;

public class ExplosiveArrowAffix : BaseAffix
{
    void Start()
    {
        //PlayerAttack.onShoot += OnShoot;
    }

    public void OnShoot(PlayerAttack playerShoot, Projectile projectile)
    { 
        projectile.hitEffect = StaticResources.explosiveArrowExplosion;
    }
    void OnDestroy()
    {
        //PlayerAttack.onShoot -= OnShoot;
    }
}
