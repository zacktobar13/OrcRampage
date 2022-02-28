using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    static float minimumTimeBetweenSounds = .05f;
    static float lastSoundTime = Mathf.NegativeInfinity;

    public static void PlayOneShot(AudioSource audioSource, AudioClip audioClip)
    {
        PlayOneShot(audioSource, audioClip, new SoundManagerArgs(1));
    }

    public static void PlayOneShot(AudioSource audioSource, AudioClip audioClip, SoundManagerArgs args)
    {
        if (!args.alwaysPlay && Time.time < minimumTimeBetweenSounds + lastSoundTime)
        {
            Debug.Log("Skipped playing sound from: " + audioSource.gameObject);
            return;
        }

        float oldPitch = audioSource.pitch;
        audioSource.pitch = Random.Range(args.customPitchRange.x, args.customPitchRange.y);

        audioSource.PlayOneShot(audioClip, args.volumeScalar);
        lastSoundTime = Time.time;

        audioSource.pitch = oldPitch;
    }
}

public struct SoundManagerArgs
{
    public float volumeScalar;
    public Vector2 customPitchRange;

    /// <summary>
    /// Should this sound always play, even if there are a lot of other sounds playing
    /// </summary>
    public bool alwaysPlay;


    // TODO: Make the other constructors as they're needed
    public SoundManagerArgs(float volume)
    {
        volumeScalar = volume;
        customPitchRange = new Vector2(.8f, 1.2f);
        alwaysPlay = false;
    }

    public SoundManagerArgs(Vector2 customPitchRange)
    {
        volumeScalar = 1;
        this.customPitchRange = customPitchRange;
        alwaysPlay = false;
    }

    public SoundManagerArgs(bool alwaysPlay)
    {
        volumeScalar = 1;
        customPitchRange = new Vector2(.8f, 1.2f);
        this.alwaysPlay = alwaysPlay;
    }

    public SoundManagerArgs(float volume, Vector2 customPitchRange)
    {
        volumeScalar = volume;
        this.customPitchRange = customPitchRange;
        alwaysPlay = false;
    }
}
