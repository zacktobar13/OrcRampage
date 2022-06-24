using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class PlayerSerializedStats
{
    private static PlayerStatWrapper statWrapper;

    private static string serializationPath = Application.persistentDataPath + "/playerStats.json";
    private static bool hasBeenDeserialized = false;
    
    public static void SerializeAllStats()
    {
        string serializedStats = JsonUtility.ToJson(statWrapper);
        FileStream file = new FileStream(serializationPath, FileMode.OpenOrCreate);
        StreamWriter streamWriter = new StreamWriter(file);
        Debug.Log("Writing player stats to: " + serializationPath);
        Debug.Log("Serialized Stats: " + serializedStats);
        streamWriter.Write(serializedStats);
        streamWriter.Close();
    }

    static void DeserializeAllStats()
    {
        FileStream fileStream = new FileStream(serializationPath, FileMode.OpenOrCreate);
        StreamReader streamReader = new StreamReader(fileStream);

        string contents = streamReader.ReadToEnd();
        if (contents == "")
        {
            Debug.Log("No existing player stats found at: " + serializationPath + "\n... Creating fresh ones.");
            statWrapper = new PlayerStatWrapper();
        }
        else
        {
            Debug.Log("Loading player stats from: " + serializationPath);
            statWrapper = JsonUtility.FromJson<PlayerStatWrapper>(contents);
        }

        streamReader.Close();
        hasBeenDeserialized = true;
    }

    public static void EnsureDeserialized()
    {
        if (!hasBeenDeserialized)
            DeserializeAllStats();
    }

    public static int AddExperience(int amount)
    {
        EnsureDeserialized();
        statWrapper.experience += amount;
        return statWrapper.experience;
    }

    public static int RemoveExperience(int amount)
    {
        EnsureDeserialized();
        statWrapper.experience -= amount;
        Debug.Assert(amount >= 0);
        return statWrapper.experience;
    }

    public static int GetExperience()
    {
        EnsureDeserialized();
        return statWrapper.experience;
    }

    public static int IncrementLifetimeKills()
    {
        EnsureDeserialized();
        statWrapper.LifetimeKills += 1;
        return statWrapper.LifetimeKills;
    }
}

[System.Serializable]
class PlayerStatWrapper
{
    public int experience;
    public int LifetimeKills;
}
