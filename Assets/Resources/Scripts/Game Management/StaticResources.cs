using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticResources : MonoBehaviour
{

    public static GameObject damageNumber;
    public static GameObject healNumber;
    public static GameObject critDamageNumber;
    public static GameObject playerDamageNumber;
    public static GameObject soundEffect;

    public static Material whiteMaterial;
    public static GameObject damageCollider;
    public static GameObject floatingExperienceNumber;
    public static GameObject arrow;
    public static GameObject blood;
    public static GameObject explosionSmall;
    public static GameObject hitEffect1;
    public static GameObject copperCoin;
    public static GameObject xpGlobe;
    public static Material pixelSnap;
    public static Material shadowMaterial;
    public static Material decorationShadowMaterial;
    public static RuntimeAnimatorController runtimeAnimatorController;

    void Awake()
    {
        damageNumber = Resources.Load("Prefabs/UI/Damage Number", typeof(GameObject)) as GameObject;
        healNumber = Resources.Load("Prefabs/UI/Heal Number", typeof(GameObject)) as GameObject;
        critDamageNumber = Resources.Load("Prefabs/UI/Crit Damage Number", typeof(GameObject)) as GameObject;
        playerDamageNumber = Resources.Load("Prefabs/UI/Player Damage Number", typeof(GameObject)) as GameObject;
        soundEffect = Resources.Load("Prefabs/Game Management/Sound Effect", typeof(GameObject)) as GameObject;

        hitEffect1 = Resources.Load("Prefabs/Visual Effects/Hit Effect Small 1") as GameObject;
        arrow = Resources.Load("Prefabs/Projectiles/Arrow") as GameObject;
        explosionSmall = Resources.Load("Prefabs/Explosions/Explosion Small 1") as GameObject;
        arrow = Resources.Load("Prefabs/Projectiles/Arrow") as GameObject;
        blood = Resources.Load("Prefabs/Visual Effects/Blood Particle 1") as GameObject;
        whiteMaterial = Resources.Load("Materials/WhiteFlash", typeof(Material)) as Material;
        damageCollider = Resources.Load("Prefabs/Damage/Damage Collider Small 1", typeof(GameObject)) as GameObject;
        floatingExperienceNumber = Resources.Load("Prefabs/UI/XP Number", typeof(GameObject)) as GameObject;
        pixelSnap = Resources.Load("Materials/Pixel Snap") as Material;
        shadowMaterial = Resources.Load("Materials/ShadowMaterial") as Material;
        decorationShadowMaterial = Resources.Load("Materials/DecorationShadowMaterial") as Material;
        copperCoin = Resources.Load("Prefabs/Interactibles/Coins/Copper Coin") as GameObject;
        xpGlobe = Resources.Load("Prefabs/Interactibles/XP Globe") as GameObject;
        runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("SpriteAnimController");
    }
}
