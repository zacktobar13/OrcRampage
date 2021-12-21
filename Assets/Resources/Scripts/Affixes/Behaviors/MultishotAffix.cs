using UnityEngine;

public class MultishotAffix : BaseAffix
{
    [HideInInspector]
    public int extraShots;

    public void Start()
    {
        PlayerShoot.onFire += OnShoot;
        extraShots = 5;
    }

    public void OnShoot(PlayerShoot playerShoot)
    {
        int directionFlip = 1;
        int distance = 0;
        for (int i = 0; i < extraShots; i++)
        {
            if (directionFlip == 1)
            {
                distance += 2;
            }
            playerShoot.ShootBow(distance * directionFlip, playerShoot.arrowReleaseBar.canCriticalFire);
            directionFlip *= -1;
        }
    }

    private void OnDestroy()
    {
        PlayerShoot.onFire -= OnShoot;
    }
}
