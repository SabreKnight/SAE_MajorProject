using System.Collections;
using System.Collections.Generic;
using Unity.Services.Analytics;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.Analytics;
public class AnalyticsManager : MonoBehaviour
{
    public static AnalyticsManager instance;

    private void Awake()
    {
        if (instance == null) // standard singleton 
        { 
            instance = this; 
        }
        else 
        { 
            Destroy(this); 
        }

        DontDestroyOnLoad(gameObject);
    }

    private IEnumerator Start()
    {
        yield return UnityServices.InitializeAsync(); // this initialises unity to recieve data
        StartDataCollectionRequest();
    }

    // These Methods are suppose to be assigned to in-game UI buttons for the player to decide.

    public void RequestUnityPrivacyPolicy()    // opens a URL to unity's personal Privacy policy
    {
        Application.OpenURL(AnalyticsService.Instance.PrivacyUrl);
        Debug.Log("user has opened Unity's Privacy policy");
    }

    public void StartDataCollectionRequest()
    {
        AnalyticsService.Instance.StartDataCollection();    // this starts data collection
        Debug.Log("Analytics data has started recording");
    }

    public void StopDataCollectRequest()   // request to stop analytcis data collection
    {
        AnalyticsService.Instance.StopDataCollection();
        Debug.Log("Request for analytics data to stop recording has been made");
    }

    public void DeleteDataRequest()    // request to delete analytics data.
    {
        AnalyticsService.Instance.RequestDataDeletion();
        Debug.Log("Request for analytics data to be deleted has been made");
    }

    public void MileStoneCompletionEvent(int progressionlevel) 
    {

        CustomEvent ProgressionCompletion = new CustomEvent("ProgressionCompletion")
        {
            {"MilestoneName", progressionlevel},    //name of milestone
            {"EventTriggerTime", Time.time}        // seconds of playtime this session
        };

        AnalyticsService.Instance.RecordEvent(ProgressionCompletion); 
    }
}
