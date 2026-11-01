using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyActionManager : MonoBehaviour
{
    protected Health_BaseClass CharacterHealth;
    public GameObject playerObject;
    public BehaviourState CurrentStateEnum;
    [SerializeField] protected BehaviourState_BaseClass CurrentStateClass;
    [SerializeField] protected List<BehaviourState_BaseClass> behaviourList;
    public bool isFixedUpdateState = false;
    public float EnemyMovementSpeed;

    public AttackStats attackStats;

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
        if(!isFixedUpdateState)
        {
            ActiveUpdate();
        }
    }

    protected void FixedUpdate()
    {
        if(isFixedUpdateState)
        {
            ActiveUpdate();
        }
    }

    protected void ActiveUpdate()
    {
        if(!CurrentStateClass.ActiveBehaviourState())
        {
            Swapbehaviours();
            //Debug.Log("Attempting to change state");
        }
    }

    protected virtual void Swapbehaviours()
    {
        switch(CurrentStateEnum)
        {
            case BehaviourState.FindPlayer:
            {
                ChangeState(1);
                break;
            }

            case BehaviourState.TrackingPlayer:
            {
                Debug.Log("Enemy has found the player");
                ChangeState(2);
                break;
            }

            case BehaviourState.SwipeAttack:
            {
                Debug.Log("Enemy has completed attack");
                ChangeState(3);
                break;        
            }

            case BehaviourState.Cooldown:
            {
                Debug.Log("Enemy has finished cooldown");
                ChangeState(1);
                break;
            }
        }
    }

#region Change State
    protected void ChangeState(BehaviourState_BaseClass newState)
    {
        if(CurrentStateClass != null)
        {
            CurrentStateClass.ExitBehaviourState();
        }

        Debug.Log("changing state from " + CurrentStateClass + " to " + newState);

        CurrentStateClass = newState;
        CurrentStateClass.EnterBehaviourState(this);
    }

    protected void ChangeState(int behaviourIndex)
    {
        if(CurrentStateClass != null)
        {
            CurrentStateClass.ExitBehaviourState();
        }
        
        Debug.Log("changing state from " + CurrentStateClass + " to " + behaviourList[behaviourIndex]);

        CurrentStateClass = behaviourList[behaviourIndex];
        CurrentStateClass.EnterBehaviourState(this);

    }
#endregion

}

public enum BehaviourState
{
    TrackingPlayer = 0,
    FindPlayer = 1,
    SwipeAttack = 2,
    Cooldown = 3

};
