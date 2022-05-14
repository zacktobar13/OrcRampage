using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectSpawner : MonoBehaviour
{
    public ObjectParameters[] availableObjects;
    public int chanceForNothing;
    int totalObjectNumber;

    [System.Serializable]
    public struct ObjectParameters
    {
        public GameObject prefab;
        public int spawnChance;
    }
    void Start()
    {
        totalObjectNumber = GetTotalChanceNumber();
        Debug.Assert(totalObjectNumber == 100f, "Chances at " + gameObject.name + " did not add up to 100!");
        SpawnChest();
    }

    int GetTotalChanceNumber()
    {
        int total = 0;
        for (int i = 0; i < availableObjects.Length; i++)
        {
            total += availableObjects[i].spawnChance;
        }

        return total + chanceForNothing;
    }

    public void SpawnChest()
    {
        int randomRoll = Random.Range(1, 101);

        randomRoll -= chanceForNothing;

        if (randomRoll <= 0)
        {
            return;
        }

        GameObject toSpawn = null;
        foreach (ObjectParameters obj in availableObjects)
        {
            randomRoll -= obj.spawnChance;
            if (randomRoll <= 0)
            {
                toSpawn = obj.prefab;
                break;
            }
        }
        
        Instantiate(toSpawn, transform.position, Quaternion.identity);
    }
}
