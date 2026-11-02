using UnityEngine;
using System.Collections;

public interface I_HitObject
{
    public void HitObjectMethod(Item tool, Vector2 HitDirection)
    {
        
    }

    public void HitObjectMethod(Item tool, float altmult, Vector2 HitDirection)
    {
        
    }
}

public interface I_Dammageable : I_HitObject
{
    public void Damaged(AttackStats attackStats, Vector2 HitDirection)
    {
        
    }
}

public interface I_DeathManager
{
    public void CharacterDied(Health_BaseClass characterHealth)
    {
        
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

public abstract class BehaviourState_BaseClass: MonoBehaviour
{
    protected EnemyActionManager enemyActionManagerRef;
    public virtual void EnterBehaviourState(EnemyActionManager actionManager)
    {
        enemyActionManagerRef = actionManager;
    }

    public virtual bool ActiveBehaviourState()
    {
        return true;
    }

    public virtual void ExitBehaviourState()
    {
        
    }
}
