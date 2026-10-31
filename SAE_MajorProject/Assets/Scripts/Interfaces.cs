using UnityEngine;
using System.Collections;

public interface I_HitObject
{
    public void HitObjectMethod(Item tool, Vector3 HitDirection)
    {
        
    }

    public void HitObjectMethod(Item tool, float altmult, Vector2 HitDirection)
    {
        
    }
}

public interface I_DeathManager
{
    public IEnumerator CharacterDied(Health_BaseClass characterHealth)
    {
        yield return null;
    }
}

public interface I_InteractObject
{
    public bool InteractObjectMethod()
    {
        return false;
    }

    public int DepositItem(InstantiatedItem item, int panelIndex)
    {

        return 0;
    }
}

public abstract class BehaviourState_BaseClass
{
    protected EnemyActionManager enemyActionManagerRef;
    public void EnterBehaviourState(EnemyActionManager actionManager)
    {
        enemyActionManagerRef = actionManager;
    }

    public bool ActiveBehaviourState()
    {
        return true;
    }

    public void ExitBehaviourState()
    {
        
    }
}
