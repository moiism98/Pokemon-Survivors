using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;
using Unity.VisualScripting;
using System;

[System.Serializable]
public class Sound
{
    [HideInInspector]
    public AudioSource source;
    public string name;
    public AudioClip clip;
    [Range(0f, 1f)]
    public float volume = 1f;
    [Range(.1f, 3f)]
    public float pitch = 1f;
    public bool loop;
}
public class AudioManager : MonoBehaviour
{
    public AudioMixer audioMixer;
    public AudioMixerGroup mixerGroup;
    public Sound[] sounds;
    private void Awake()
    {
        foreach(Sound sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();

            sound.source.outputAudioMixerGroup = mixerGroup;

            sound.source.clip = sound.clip;

            sound.source.volume = sound.volume;

            sound.source.pitch = sound.pitch;

            sound.source.loop = sound.loop;
        }
    }

    void Start()
    {
        audioMixer.SetFloat("volume", PlayerPrefs.GetFloat("Volume"));
    }

    public void PlaySound(string name)
    {
        Sound sound = Array.Find(sounds, sound => sound.name == name);

        if(sound != null)
            sound.source.Play();
        else
            Debug.LogWarning("Sound " + name + " was not found!");
    }

    public void PlaySoundUntilEnd(string name)
    {
        Sound sound = Array.Find(sounds, sound => sound.name == name);

        if(sound != null)
        {
            if(!sound.source.isPlaying) // we do not play the sound again if it's still playing!
                sound.source.Play();
        }
        else
            Debug.LogWarning("Sound " + name + " was not found!");
    }

    public void StopSound(string name)
    {
        Sound sound = Array.Find(sounds, sound => sound.name == name);

        if(sound != null)
            sound.source.Stop();
        else
            Debug.Log("Sound " + name + " was not found!");

    }
}
