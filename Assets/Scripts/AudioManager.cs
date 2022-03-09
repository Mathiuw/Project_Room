using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    public static AudioManager instance;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        AudioManager[] instances = FindObjectsOfType<AudioManager>();

        if (instances.Length > 1)
        {
            for (int i = 0; i < instances.Length - 1; i++)
            {
                Destroy(instances[i].gameObject);
            }
        }

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;           
            s.source.loop = s.loop;
            s.source.spatialBlend = s.spatialBlend;
        }
    }

    public void Play(string name,Vector3 location)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        AudioSource.PlayClipAtPoint(s.source.clip, location);
    }
}