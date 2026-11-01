using UnityEngine;
using System.Collections;

public class CooldownState : BehaviourState_BaseClass
{
    private bool completedWait = false;
    private float CurrentWaitTime;
    [SerializeField] private float TotalWaitTime = 3f;

    private IEnumerator WaitMethod()
    {
        Debug.Log("Starting swipe attack method");
        CurrentWaitTime = 0f;
        while(CurrentWaitTime < TotalWaitTime)
        {
            yield return new WaitForSeconds(0.1f);
            CurrentWaitTime += 0.1f;    
        }
        completedWait = false;
        yield return null;
    }

    public override void EnterBehaviourState(EnemyActionManager actionManager)
    {
        enemyActionManagerRef = actionManager;
        enemyActionManagerRef.isFixedUpdateState = false;
        enemyActionManagerRef.CurrentStateEnum = BehaviourState.SwipeAttack;
        completedWait = true;
        StartCoroutine(WaitMethod());
    }

    public override bool ActiveBehaviourState()
    {
        return completedWait;
    }

    public override void ExitBehaviourState()
    {
        
    }
}
