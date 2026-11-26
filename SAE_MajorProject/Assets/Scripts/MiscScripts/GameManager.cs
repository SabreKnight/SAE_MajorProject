using UnityEngine;
using Unity.Netcode;

public class GameManager : NetworkBehaviour
{
    public static GameManager Instance { get; private set; }
    public InputManager_Cont inputController;
    public UIManager UIManagerSingleton;
    public AudioManager audioManagerSingleton;
    public AnimationCurve DroppedItemAnimation;
    public NetworkObject Playerobj;
    public SceneChanger sceneChanger;
    public PlayerRespawn playerRespawn;

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

#region Spawning_Player

    [Rpc(SendTo.ClientsAndHost)]
    public void SetupPlayerSpawn_Rpc(NetworkObjectReference playerObjectref)
    {
        NetworkObject playerObject = playerObjectref;
        if(playerObject.IsOwner)
        {
            Playerobj = playerObject;
            inputController.SetUpPlayerObject(playerObject);
            Camera.main.gameObject.GetComponent<CameraScript>().SetPlayerTrack(playerObject);
            playerRespawn.playerhealth = playerObject.GetComponent<PlayerHealth>();
        }
        else
        {
            Debug.Log("player object is attempting to link to game manager, but is not owned by this client");
        }
    }

#endregion

}
