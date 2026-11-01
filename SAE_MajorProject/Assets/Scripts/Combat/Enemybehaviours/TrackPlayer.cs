using UnityEngine;

public class TrackPlayer_EB : BehaviourState_BaseClass
{
    private GameObject playerobject;
    [SerializeField] private float playerTrackThreshold;
    public override void EnterBehaviourState(EnemyActionManager actionManager)
    {
        enemyActionManagerRef = actionManager;

        playerobject = enemyActionManagerRef.playerObject;
        enemyActionManagerRef.isFixedUpdateState = true;
        enemyActionManagerRef.CurrentStateEnum = BehaviourState.TrackingPlayer;
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
            return false;
        }
        else
        {

            Vector3 EnemyApproach = (playerobject.transform.position - transform.position).normalized * enemyActionManagerRef.EnemyMovementSpeed;
            transform.position += EnemyApproach;
            Debug.Log("Enemy has moved towards: " + EnemyApproach);
            return true;
        }
        
    }

    public override void ExitBehaviourState()
    {
        
    }
}
