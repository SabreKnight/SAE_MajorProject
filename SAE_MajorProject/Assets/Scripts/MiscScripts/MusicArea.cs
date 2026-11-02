using UnityEngine;

public class MusicArea : MonoBehaviour
{
    [SerializeField] private string EnterTrackName;
    [SerializeField] private bool TrackOnEnter;
    [SerializeField] private float enterFadetime = 5f;
    [SerializeField] private string ExitTrackName;
    [SerializeField] private bool TrackOnExit;
    [SerializeField] private float exitFadetime = 5f;

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("trigger entered the area:" + collision.gameObject);
        if(collision.gameObject.CompareTag("Player") && TrackOnEnter == true)
        {
            if(GameManager.Instance.audioManagerSingleton.audioSource.isPlaying)
            {
                StartCoroutine(GameManager.Instance.audioManagerSingleton.OverrideAudio(EnterTrackName, false, enterFadetime));
            }
            else
            {
                StartCoroutine(GameManager.Instance.audioManagerSingleton.OverrideAudio(EnterTrackName, false, 0f));
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("trigger exited the area:" + collision.gameObject);
        if(collision.gameObject.CompareTag("Player") && TrackOnExit == true)
        {
            if(GameManager.Instance.audioManagerSingleton.audioSource.isPlaying)
            {
                StartCoroutine(GameManager.Instance.audioManagerSingleton.OverrideAudio(ExitTrackName, false, exitFadetime));
            }
            else
            {
                StartCoroutine(GameManager.Instance.audioManagerSingleton.OverrideAudio(ExitTrackName, false, 0f));
            }
        }
    }
}
