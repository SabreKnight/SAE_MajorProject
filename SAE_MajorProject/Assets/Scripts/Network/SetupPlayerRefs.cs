using Unity.Netcode;
using UnityEngine;

public class SetupPlayerRefs : NetworkBehaviour
{
    [SerializeField] private PlayerHealth playerHealthRef;
    [SerializeField] private InteractionManager playerInteractionManager;

    private void OnEnable()
    {
        if(playerHealthRef == null)
        {
            playerHealthRef = GetComponent<PlayerHealth>();
        }

        if(playerInteractionManager == null)
        {
            playerInteractionManager = GetComponent<InteractionManager>();
        }
    }

    [Rpc(SendTo.Owner)]
    public void AssignOwnerClientReferences_Rpc()
    {
        if(!IsOwner)
        {
            Debug.Log("attempting to assign owner references on object that isn't owned by client");
            return;
        }

        playerHealthRef.SetupHealthUI();
        playerInteractionManager.SetGridref();

    }

    public void AssignAllClientReferences()
    {
        
    }
}
