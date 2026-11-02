using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public List<TrackClass> Tracklist;
    public Dictionary<string, AudioClip> trackDictionary = new Dictionary<string, AudioClip>();
    public AudioSource audioSource;
    [SerializeField] private float pitchVariance = 0.15f;

    public void OnEnable()
    {
        if(audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        foreach(TrackClass track in Tracklist)
        {
            trackDictionary.Add(track.name, track.audioClip);
        }
    }

    public void PlayAudio(string trackName, bool altpitch)
    {
        Debug.Log("attempting to player: " + trackName);
        if(trackDictionary.ContainsKey(trackName))
        {
            AlterSoundPitch(altpitch);
            Debug.Log(trackName + " was played");
            audioSource.PlayOneShot(trackDictionary[trackName]);
            audioSource.pitch = 1f;
        }
        else
        {
            Debug.Log(trackName + " was not found");
        }
    }

    public IEnumerator OverrideAudio(string trackName, bool altpitch, float fadeTime)
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / fadeTime;
            yield return null; // Wait for the next frame
        }

        audioSource.Stop();
        PlayAudio(trackName, altpitch);
        audioSource.volume = startVolume; // Reset volume for next time if needed
    }

    private void AlterSoundPitch(bool altpitch)
    {
        if(altpitch == true)
        {
            float randomPitch = Random.Range(1f - pitchVariance, 1f + pitchVariance);
            audioSource.pitch = randomPitch;
        }
        else
        {
            audioSource.pitch = 1f;
        }
        
    }
}

[System.Serializable]
public class TrackClass
{
    public string name;
    public AudioClip audioClip;
}
