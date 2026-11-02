using UnityEngine;
using System.Collections;

public interface I_HitObject
{
    public virtual void HitObjectMethod(Item tool, Vector2 HitDirection)
    {
       
    }
}

public interface I_Dammageable : I_HitObject
{
    public virtual void Damaged(AttackStats attackStats, Vector2 HitDirection)
    {
        
    }
}

public interface I_DeathManager
{
    public virtual void CharacterDied(Health_BaseClass characterHealth)
    {
        
    }
}

public interface I_InteractObject
{
    public virtual bool InteractObjectMethod()
    {
        return false;
    }

    public virtual int DepositItem(InstantiatedItem item, int panelIndex)
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
