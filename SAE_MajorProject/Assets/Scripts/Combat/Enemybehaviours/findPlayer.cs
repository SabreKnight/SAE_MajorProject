using UnityEngine;

public class FindPlayer_EB : BehaviourState_BaseClass
{
    [SerializeField] private float CheckRadius = 3; 
    [SerializeField] private LayerMask layerMask;

    [SerializeField] private int totalCount = 100;
    [SerializeField] private int currentCount;

    public override void EnterBehaviourState(EnemyActionManager actionManager)
    {
        layerMask = LayerMask.GetMask("Interactable");

        enemyActionManagerRef = actionManager;
        enemyActionManagerRef.isFixedUpdateState = false;
        enemyActionManagerRef.CurrentStateEnum = BehaviourState.FindPlayer;
    }

    public override bool ActiveBehaviourState()
    {
        if(totalCount <= currentCount)
        {
            currentCount = 0;
            return Checkcolliders();
        }
        currentCount ++;
        return true;
    }
    
    private bool Checkcolliders()
    {
        Debug.Log("Checking Colliders");
        Collider2D[] checkArray = new Collider2D[20];
        checkArray = Physics2D.OverlapCircleAll(this.gameObject.transform.position, CheckRadius, layerMask);

        foreach(Collider2D check in checkArray)
        {
            if(check.gameObject.CompareTag("Player"))
            {
                enemyActionManagerRef.playerObject = check.gameObject;
                Debug.Log("Player has been found");
                return false;
            }
        }
        return true;
    }

    public override void ExitBehaviourState()
    {
        
    }
}
