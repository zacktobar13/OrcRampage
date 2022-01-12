using UnityEngine;

public class MultishotAffix : BaseAffix
{
    [HideInInspector]
    public int extraShots;

    public void Start()
    {
        PlayerAttack.onShoot += OnShoot;
        extraShots = 5;
    }

    public void OnShoot(PlayerAttack playerShoot, Projectile projectile)
    {
        int directionFlip = 1;
        int distance = 0;
        for (int i = 0; i < extraShots; i++)
        {
            if (directionFlip == 1)
            {
                distance += 2;
            }
            playerShoot.ShootBow(distance * directionFlip, true);
            directionFlip *= -1;
        }
    }

    private void OnDestroy()
    {
        PlayerAttack.onShoot -= OnShoot;
    }
}
