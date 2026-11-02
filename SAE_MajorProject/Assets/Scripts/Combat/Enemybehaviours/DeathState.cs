using UnityEngine;

public class DeathState : BehaviourState_BaseClass
{

    public override void EnterBehaviourState(EnemyActionManager actionManager)
    {
        
        GetComponent<SpriteRenderer>().enabled = false;
        enemyActionManagerRef = actionManager;
        enemyActionManagerRef.CurrentStateEnum = BehaviourState.DeathState;
    }

    public override bool ActiveBehaviourState()
    {
        return true;
    }

    public override void ExitBehaviourState()
    {
        GetComponent<SpriteRenderer>().enabled = true;
        enemyActionManagerRef.animator.SetBool("Active", false);
        enemyActionManagerRef.animator.SetTrigger("Death");
        enemyActionManagerRef.playerObject = null;
    }
}
