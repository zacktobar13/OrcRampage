using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiggerProjectileAffix : BaseAffix
{
    PlayerAttack playerAttack;

    public void Start()
    {
        playerAttack = GetComponent<PlayerAttack>();
        playerAttack.onProjectileSpawned += OnProjectileSpawned;
    }

    public override void AddAffixCount(int increment)
    {
        base.AddAffixCount(increment);
    }

    public void OnProjectileSpawned(PlayerAttack playerShoot, Projectile projectile)
    {
        float scaleAmount = 1 + scalarIncreasePerAffix * affixCount;
        projectile.transform.localScale = new Vector3(scaleAmount, scaleAmount, 1);
    }

    private void OnDestroy()
    {
        playerAttack.onProjectileSpawned -= OnProjectileSpawned;
    }
}
