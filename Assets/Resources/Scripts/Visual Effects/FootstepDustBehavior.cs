using UnityEngine;
using UnityEngine.Pool;

public class FootstepDustBehavior : MonoBehaviour
{
    ObjectPool<GameObject> myPool;

    public void SetMyPool(ObjectPool<GameObject> pool)
    {
        myPool = pool;
    }

    void DestroySelf()
    {
        myPool.Release(gameObject);
    }
}
