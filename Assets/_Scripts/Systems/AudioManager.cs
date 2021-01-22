﻿using System;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    public static AudioManager Instance;

    private void Awake() 
    {
        if(Instance == null)
        {
            Instance = this;
        } else Destroy(this);
        
        foreach(Sound s in sounds)
        {
           s.source = gameObject.AddComponent<AudioSource>();
           s.source.clip = s.clip;
           s.source.volume = s.volume;
           s.source.pitch = s.pitch;
        }
    }
    void Start()
    {
        
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Play();
    }
}
