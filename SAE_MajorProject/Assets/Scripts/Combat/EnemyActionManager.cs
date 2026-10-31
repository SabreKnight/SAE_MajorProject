using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyActionManager : MonoBehaviour
{
    protected Health_BaseClass CharacterHealth;
    protected GameObject playerObject;
    protected BehaviourState_BaseClass CurrentState;
    protected List<BehaviourState_BaseClass> behaviourList;

    void OnEnable()
    {
        ChangeState(0);
        
        if(CharacterHealth == null)
        {
            CharacterHealth = this.gameObject.GetComponent<Health_BaseClass>();
        }
    }

    protected void Update()
    {
        
    }

#region Change State
    protected void ChangeState(BehaviourState_BaseClass newState)
    {
        if(CurrentState != null)
        {
            CurrentState.ExitBehaviourState();
        }
        CurrentState = newState;
        CurrentState.EnterBehaviourState(this);
    }

    protected void ChangeState(int behaviourIndex)
    {
        if(CurrentState != null)
        {
            CurrentState.ExitBehaviourState();
        }

        CurrentState = behaviourList[behaviourIndex];
        CurrentState.EnterBehaviourState(this);

    }
#endregion

}
