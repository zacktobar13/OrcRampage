using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticResources : MonoBehaviour
{

    public static Material whiteMaterial;
    public static GameObject floatingDamageNumber;
    public static GameObject critDamageNumber;
    public static GameObject damageCollider;
    public static GameObject floatingExperienceNumber;
    public static GameObject arrow;
    public static GameObject blood;
    public static GameObject explosiveArrowExplosion;
    public static GameObject hitEffect1;
    public static GameObject localCurrency;
    public static GameObject globalCurrency;
    public static Material pixelSnap;
    public static Material shadowMaterial;
    public static Material decorationShadowMaterial;
    public static RuntimeAnimatorController runtimeAnimatorController;

    void Awake()
    {
        hitEffect1 = Resources.Load("Prefabs/Particle Effects/Hit Effect Small 1") as GameObject;
        arrow = Resources.Load("Prefabs/Projectiles/Arrow") as GameObject;
        explosiveArrowExplosion = Resources.Load("Prefabs/Explosions/Explosive Arrow Explosion") as GameObject;
        arrow = Resources.Load("Prefabs/Projectiles/Arrow") as GameObject;
        blood = Resources.Load("Prefabs/Particle Effects/Blood Particle 1") as GameObject;
        whiteMaterial = Resources.Load("Materials/WhiteFlash", typeof(Material)) as Material;
        floatingDamageNumber = Resources.Load("Prefabs/UI/Damage Number", typeof(GameObject)) as GameObject;
        critDamageNumber = Resources.Load("Prefabs/UI/Crit Damage Number", typeof(GameObject)) as GameObject;
        damageCollider = Resources.Load("Prefabs/Damage/Damage Collider Small 1", typeof(GameObject)) as GameObject;
        floatingExperienceNumber = Resources.Load("Prefabs/UI/XP Number", typeof(GameObject)) as GameObject;
        pixelSnap = Resources.Load("Materials/Pixel Snap") as Material;
        shadowMaterial = Resources.Load("Materials/ShadowMaterial") as Material;
        decorationShadowMaterial = Resources.Load("Materials/DecorationShadowMaterial") as Material;
        localCurrency = Resources.Load("Prefabs/Currency/Local Currency Drop") as GameObject;
        globalCurrency = Resources.Load("Prefabs/Currency/Local Currency Drop") as GameObject;
        runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("SpriteAnimController");
    }
}
