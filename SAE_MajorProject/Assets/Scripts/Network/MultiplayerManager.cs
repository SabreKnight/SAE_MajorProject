using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using Unity.VisualScripting;
public class MultiplayerManager : NetworkBehaviour
{

    [SerializeField] private NetworkObject playerNetPrefab;
    [SerializeField] private List<NetworkObject> playerobjectList = new List<NetworkObject>();
    [SerializeField] private Vector3 SpawnLocation;

    // upon the network being created
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (!NetworkManager.IsServer)
        {
            return;
        }
            
        NetworkManager.OnClientConnectedCallback += OnClientConnectedCallback;
    }

    // upon the network being despawned.
    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();
        
        NetworkManager.OnClientConnectedCallback -= OnClientConnectedCallback;
    }

    private void OnClientConnectedCallback(ulong ClientID)
    {
        Debug.Log("Client Connected: " + ClientID);
        SpawnPlayer(ClientID);
    }

    private void SpawnPlayer(ulong ClientID)
    {
        // creates new game object
        NetworkObject newPlayer = Instantiate(playerNetPrefab, SpawnLocation, Quaternion.identity);
        // assigned game object to player
        newPlayer.SpawnAsPlayerObject(ClientID);
        playerobjectList.Add(newPlayer);

        newPlayer.GetComponent<SetupPlayerRefs>().AssignOwnerClientReferences_Rpc();
        GameManager.Instance.SetupPlayerSpawn_Rpc(newPlayer);
    }

}
