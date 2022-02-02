using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedManager : MonoBehaviour
{
    public string seedInput;
    int seed;

    void Awake()
    {
        // No string was input, generate one.
        if (seedInput.Length == 0)
        {
            GenerateString();
        }
        else
        {
            Debug.Log("User input seed " + seedInput + " used.");
        }

        Random.InitState(BuildSeed(seedInput));
    }

    void GenerateString()
    {
        for (int i = 0; i < 12; i++)
        {
            char c = (char)('!' + Random.Range(0,94));
            seedInput += c;
        }

        Debug.Log("Randomly generated seed " + seedInput + " used.");
    }

    int BuildSeed(string input)
    {
        foreach (char c in seedInput)
        {
            seed += c;
        }
        return seed;
    }
}
