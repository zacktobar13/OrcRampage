using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PoolManager : MonoBehaviour
{
   // public static PoolManager instance = null;
    public static Dictionary<string, ObjectPool<GameObject>> objectPools;
    /*void Awake()
    {
        //Check if instance already exists
        if (instance == null)
        {
            //if not, set instance to this
            instance = this;
        }

        //If instance already exists and it's not this:
        else if (instance != this)
        {
            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            DestroyImmediate(gameObject);
            return;
        }

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);
    }*/

    public void Awake()
    {
        objectPools = new Dictionary<string, ObjectPool<GameObject>>();
    }

    public void CreateNewPool(string poolName, GameObject pooledPrefab, int defaultCapacity, int maxPoolSize)
    {
        if (!objectPools.ContainsKey(poolName))
        {
            ObjectPool<GameObject> pool = new ObjectPool<GameObject>(() => Instantiate(pooledPrefab), (obj) => obj.SetActive(true),
                                                        (obj) => obj.SetActive(false), (obj) => Destroy(obj), false, defaultCapacity, maxPoolSize);

            objectPools.Add(poolName, pool);
            GameObject poolChild = new GameObject(poolName + " Pool");
            poolChild.transform.SetParent(transform);
          //  Debug.Log("New " + poolName + " pool of size " + defaultCapacity + " created!");
        }
        else
        {
            Debug.LogWarning("Failed to create new pool " + poolName + " as a pool with that name already exists.");
        }
    }

    // Retrieve our product's associated pool or create one if it doesn't exist.
    public ObjectPool<GameObject> GetObjectPool(GameObject poolType)
    {
        string objectName = poolType.name.Replace("(Clone)", "");

        if (objectPools.ContainsKey(objectName))
        {
            ObjectPool<GameObject> pool = PoolManager.objectPools[objectName];
          //  Debug.Log(pool + " found.");
            return pool;
        }
        else
        {
            CreateNewPool(objectName, poolType, 10, 200); // TODO Magic numbers.
            return PoolManager.objectPools[objectName];
        }
    }

   /* private void OnDestroy()
    {
        if (this == instance)
            instance = null;
    }*/
}
