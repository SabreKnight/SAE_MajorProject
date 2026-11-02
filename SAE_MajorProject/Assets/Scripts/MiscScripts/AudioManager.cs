using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public List<TrackClass> Tracklist;
    public Dictionary<string, AudioClip> trackDictionary = new Dictionary<string, AudioClip>();
    public AudioSource audioSource;

    public void OnEnable()
    {
        foreach(TrackClass track in Tracklist)
        {
            trackDictionary.Add(track.name, track.audioClip);
        }
    }

    public void PlayAudio(string trackName)
    {
        if(trackDictionary.ContainsKey(trackName))
        {
            
        }
    }

    public void OverrideAudio(string trackName)
    {
        
    }
}

[System.Serializable]
public class TrackClass
{
    public string name;
    public AudioClip audioClip;
}
