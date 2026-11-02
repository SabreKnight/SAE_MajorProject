using Unity.VisualScripting;
using UnityEngine;

public class TrackPlayer_EB : BehaviourState_BaseClass
{
    private GameObject playerobject;
    [SerializeField] private float playerTrackThreshold;
    private Rigidbody2D rigidRef;
    public override void EnterBehaviourState(EnemyActionManager actionManager)
    {
        enemyActionManagerRef = actionManager;

        playerobject = enemyActionManagerRef.playerObject;
        enemyActionManagerRef.isFixedUpdateState = true;
        enemyActionManagerRef.CurrentStateEnum = BehaviourState.TrackingPlayer;
        rigidRef = enemyActionManagerRef.rigidbody2DRef;
    }

    public override bool ActiveBehaviourState()
    {
        return TrackPlayermethod();
    
    }

    private bool TrackPlayermethod()
    {
        float playerDistance = Vector2.Distance(playerobject.transform.position, transform.position);
        
        if(playerDistance <= playerTrackThreshold)
        {
            Debug.Log("Player is within range");
            if(enemyActionManagerRef.completedWait == true)
            {
                return false;
            }

            Debug.Log("attack cooldown has not completed yet");
            return true;
        }
        else
        {

            Vector2 EnemyApproach = (Vector2)(playerobject.transform.position - transform.position).normalized * enemyActionManagerRef.EnemyMovementSpeed;
            rigidRef.MovePosition((Vector2)transform.position + EnemyApproach);
            Debug.Log("Enemy has moved towards: " + EnemyApproach);
            return true;
        }
        
    }

    public override void ExitBehaviourState()
    {
        
    }
}
