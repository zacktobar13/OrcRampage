using UnityEngine;
using UnityEngine.Pool;

public class SoundEffect : MonoBehaviour
{
    ObjectPool<GameObject> myPool;
    AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();   
    }

    public void SetMyPool(ObjectPool<GameObject> pool)
    {
        myPool = pool;
    }

    void Update()
    {
        if (!audioSource.isPlaying)
        {
            myPool.Release(gameObject);
        }
    }
}
