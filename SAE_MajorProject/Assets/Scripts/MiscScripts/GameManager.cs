using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public InputManager_Cont inputController;
    public UIManager UIManagerSingleton;
    public AudioManager audioManagerSingleton;
    public AnimationCurve DroppedItemAnimation;
    public GameObject Playerobj;
    public SceneChanger sceneChanger;

    private void Awake()
    {
        // Enforce the Singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Delete duplicate instances
            return;
        }

        Instance = this;
        // Optional: Keep this object alive when changing scenes
        DontDestroyOnLoad(gameObject); 

        FindReferences();
    }

    private void FindReferences()
    {
        if(inputController == null)
        {
            inputController = InputManager_Cont.FindFirstObjectByType<InputManager_Cont>();
        }

        if(UIManagerSingleton == null)
        {
            UIManagerSingleton = UIManager.FindFirstObjectByType<UIManager>();
        }

        if(audioManagerSingleton == null)
        {
            audioManagerSingleton = AudioManager.FindFirstObjectByType<AudioManager>();
        }

        if(sceneChanger == null)
        {
            sceneChanger = SceneChanger.FindFirstObjectByType<SceneChanger>();
        }
    }
}
