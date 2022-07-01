using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    static float minimumTimeBetweenSounds = .05f;
    static float lastSoundTime = Mathf.NegativeInfinity;
    static AudioSource globalAudioSource;
    static AudioMixer mixer;

    private void Start() 
    {
        mixer = Resources.Load("Audio/MasterMixer") as AudioMixer;
    }

    public static void PlayOneShot(Vector3 spawnLocation, AudioClip audioClip)
    {
        PlayOneShot(spawnLocation, audioClip, new SoundManagerArgs(false));
    }

    public static void PlayOneShot(Vector3 spawnLocation, AudioClip audioClip, SoundManagerArgs args)
    {
        if (!args.alwaysPlay && Time.time < minimumTimeBetweenSounds + lastSoundTime)
        {
            return;
        }

        GameObject soundEffect = Instantiate(StaticResources.soundEffect, spawnLocation, Quaternion.identity);
        AudioSource audioSource = soundEffect.GetComponent<AudioSource>();

        audioSource.outputAudioMixerGroup = mixer.FindMatchingGroups(args.mixerName)[0];   
        float oldPitch = audioSource.pitch;
        audioSource.pitch = Random.Range(args.customPitchRange.x, args.customPitchRange.y);

        audioSource.PlayOneShot(audioClip, 1f);
        lastSoundTime = Time.time;

        audioSource.pitch = oldPitch;
    }

    public static void SetGlobalAudioSource(AudioSource audioSource)
    {
        globalAudioSource = audioSource;
    }

    //public static void PlayGlobalAudio(AudioClip audioClip, SoundManagerArgs args)
   // {
   //     PlayOneShot(globalAudioSource, audioClip, args);
   // }
}

public struct SoundManagerArgs
{
    public Vector2 customPitchRange;

    /// <summary>
    /// Should this sound always play, even if there are a lot of other sounds playing
    /// </summary>
    public bool alwaysPlay;
    /// <summary>
    /// The name of the audio mixer this sound effect routes to
    /// </summary>
    public string mixerName;


    // TODO: Make the other constructors as they're needed
    public SoundManagerArgs(bool alwaysPlay)
    {
        customPitchRange = new Vector2(.8f, 1.2f);
        this.alwaysPlay = alwaysPlay;
        this.mixerName = "Master";
    }

    public SoundManagerArgs(string mixer)
    {
        customPitchRange = new Vector2(.8f, 1.2f);
        this.alwaysPlay = false;
        this.mixerName = mixer;
    }

    public SoundManagerArgs(bool alwaysPlay, string mixer)
    {
        customPitchRange = new Vector2(.8f, 1.2f);
        this.alwaysPlay = alwaysPlay;
        this.mixerName = mixer;
    }

    public SoundManagerArgs(Vector2 customPitchRange)
    {
        this.customPitchRange = customPitchRange;
        alwaysPlay = false;
        this.mixerName = "Master";
    }
}
