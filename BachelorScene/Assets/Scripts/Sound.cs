using UnityEngine;

[System.Serializable]
public class Sound
{
    public string Name;
    public AudioClip AudioClip;
    public bool Loop;

    [Range(0f,1f)] 
    public float Volume;

    [HideInInspector]
    public AudioSource AudioSource;

}
