using UnityEngine;

public class MultishotAffix : BaseAffix
{
    public int extraShots;
    public float bulletPadding;

    PlayerAttack playerAttack;

    public void Start()
    {
        playerAttack = GetComponent<PlayerAttack>();
        playerAttack.onPlayerShoot += OnShoot;
    }

    public void OnShoot(PlayerAttack playerShoot)
    {
        int directionFlip = 1;
        float distance = 0;
        for (int i = 0; i < extraShots; i++)
        {
            if (directionFlip == 1)
            {
                distance += bulletPadding;
            }
            directionFlip *= -1;

            float weaponRotation = playerShoot.GetWeaponRotation();
            Vector2 weaponOffsetDirection = Utility.Rotate(Vector2.right, weaponRotation + 90);

            playerShoot.ForceAttack(weaponOffsetDirection * distance * directionFlip);
        }
    }

    private void OnDestroy()
    {
        playerAttack.onPlayerShoot -= OnShoot;
    }
}
