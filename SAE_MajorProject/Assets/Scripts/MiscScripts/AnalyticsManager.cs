using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Unity.Services.Analytics;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.UnityConsent;
public class AnalyticsManager : MonoBehaviour
{
    public static AnalyticsManager Instance;
    public int DeathCount = 0;
    public int KillCount = 0;
    public int harvestCount = 0;

    private void Awake()
    {
        if (Instance == null) // standard singleton 
        { 
            Instance = this; 
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
        EndUserConsent.SetConsentState(new ConsentState {
        AnalyticsIntent = ConsentStatus.Granted,
        AdsIntent = ConsentStatus.Denied
        });    // this starts data collection


        Debug.Log("Analytics data has started recording");
    }

    public void StopDataCollectRequest()   // request to stop analytcis data collection
    {
        EndUserConsent.SetConsentState(new ConsentState {
        AnalyticsIntent = ConsentStatus.Denied,
        AdsIntent = ConsentStatus.Denied
        });
        
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
            {"ProgressionLevel", progressionlevel},    //name of milestone
            {"EventTriggerTime", Time.time}        // seconds of playtime this session
        };

        AnalyticsService.Instance.RecordEvent(ProgressionCompletion); 
    }

    public void DeathEvent(Vector2 position) 
    {
        string deathpos = ("X: " + position.x + " Y: " + position.y);
        DeathCount ++;
        CustomEvent DeathEvent = new CustomEvent("DeathEvent")
        {
            {"DeathLocation", deathpos},
            {"EventTriggerTime", Time.time}        // seconds of playtime this session
        };

        AnalyticsService.Instance.RecordEvent(DeathEvent); 
    }

    public void FinalStats(bool quit)
    {
        CustomEvent FinalStatsEvent = new CustomEvent("FinalStatsEvent")
        {
            {"DeathCount", DeathCount},
            {"KillCount", KillCount},
            {"HarvestCount", harvestCount},
            {"EventTriggerTime", Time.time}        // seconds of playtime this session
        };

        AnalyticsService.Instance.RecordEvent(FinalStatsEvent);
        
        if(quit == true)
        {
            Application.Quit();   
        }
    }
}


