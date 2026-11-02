using System.Collections.Generic;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyActionManager : MonoBehaviour
{
    protected Health_BaseClass CharacterHealth;
    public Rigidbody2D rigidbody2DRef;
    public GameObject playerObject;
    public BehaviourState CurrentStateEnum;
    [SerializeField] protected BehaviourState_BaseClass CurrentStateClass;
    [SerializeField] protected List<BehaviourState_BaseClass> behaviourList;
    public bool isFixedUpdateState = false;
    public float EnemyMovementSpeed;
    
    public Animator animator;
    public SpriteRenderer spriteRenderer;

    public AttackStats attackStats;

    void OnEnable()
    {
        if(spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        ChangeState(3);
        
        if(CharacterHealth == null)
        {
            CharacterHealth = this.gameObject.GetComponent<Health_BaseClass>();
        }

        if(rigidbody2DRef == null)
        {
            rigidbody2DRef = GetComponent<Rigidbody2D>();
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
                completedWait = true;
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
                ChangeState(1);
                break;        
            }

            case BehaviourState.DeathState:
            {
                ChangeState(0);
                break;
            }
        }
    }

#region Change State
    public void ChangeState(BehaviourState_BaseClass newState)
    {
        if(CurrentStateClass != null)
        {
            CurrentStateClass.ExitBehaviourState();
        }

        Debug.Log("changing state from " + CurrentStateClass + " to " + newState);

        CurrentStateClass = newState;
        CurrentStateClass.EnterBehaviourState(this);
    }

    public void ChangeState(int behaviourIndex)
    {
        if(CurrentStateClass != null)
        {
            CurrentStateClass.ExitBehaviourState();
        }
        
        //Debug.Log("changing state from " + CurrentStateClass + " to " + behaviourList[behaviourIndex]);

        CurrentStateClass = behaviourList[behaviourIndex];
        CurrentStateClass.EnterBehaviourState(this);

    }
#endregion
    public bool completedWait = true;
    private float CurrentWaitTime = 0f;
    [SerializeField] private float TotalWaitTime = 3f;

    public IEnumerator WaitMethod()
    {
        
        completedWait = false;
        Debug.Log("Starting cooldown attack method");
        CurrentWaitTime = 0f;
        while(CurrentWaitTime < TotalWaitTime)
        {
            yield return new WaitForSeconds(0.1f);
            CurrentWaitTime += 0.1f;    
        }
        completedWait = true;
        yield return null;
    }

}

public enum BehaviourState
{
    TrackingPlayer = 0,
    FindPlayer = 1,
    SwipeAttack = 2,
    DeathState = 3

};
