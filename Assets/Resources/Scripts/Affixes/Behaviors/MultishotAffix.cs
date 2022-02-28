using UnityEngine;

public class MultishotAffix : BaseAffix
{
    public int extraShots;
    float angleDeltaIncrement;

    PlayerAttack playerAttack;

    public void Start()
    {
        playerAttack = GetComponent<PlayerAttack>();
        playerAttack.onPlayerShoot += OnShoot;
        extraShots = affixCount;
        angleDeltaIncrement = 15f;
    }

    public override void AddAffixCount(int increment)
    {
        base.AddAffixCount(increment);
        extraShots = affixCount;
    }

    public void OnShoot(PlayerAttack playerShoot)
    {
        int directionFlip = 1;
        float angleDelta = 0;
        for (int i = 0; i < extraShots; i++)
        {
            if (directionFlip == 1)
            {
                angleDelta += angleDeltaIncrement;
            }
            directionFlip *= -1;

            playerShoot.ForceAttack(angleDelta * directionFlip);
        }
    }

    private void OnDestroy()
    {
        playerAttack.onPlayerShoot -= OnShoot;
    }
}
