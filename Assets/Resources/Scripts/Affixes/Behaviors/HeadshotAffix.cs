using UnityEngine;

public class HeadshotAffix : BaseAffix
{
    float headshotChance = 10;

    void Start()
    {
        PlayerAttack.onShoot += OnShoot;
    }

    public void OnShoot(PlayerAttack playerShoot, Projectile projectile)
    {
        if (projectile.isCriticalHit)
        {
            if (RollHeadshot())
                projectile.isHeadshot = true;
        }
    }

    bool RollHeadshot()
    {
        return (headshotChance >= Random.Range(0, 100));
    }

    void OnDestroy()
    {
        PlayerAttack.onShoot -= OnShoot;
    }
}

