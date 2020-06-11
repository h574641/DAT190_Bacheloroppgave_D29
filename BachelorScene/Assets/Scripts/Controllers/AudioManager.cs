using System;
using Unity.Audio;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    void Awake()
    {
        foreach(Sound s in sounds)
        {
            s.AudioSource = gameObject.AddComponent(typeof(AudioSource)) as AudioSource;
            s.AudioSource.clip = s.AudioClip;
            s.AudioSource.volume = s.Volume;
            s.AudioSource.loop = s.Loop;
        }
        
    }
    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.Name.Equals(name));
        if(s == null)
        {
            Debug.LogError("AudioManager: Attempted to play audio file: " + name + ", it was not found!");
            return;
        }
        s.AudioSource.Play();
    }
}
