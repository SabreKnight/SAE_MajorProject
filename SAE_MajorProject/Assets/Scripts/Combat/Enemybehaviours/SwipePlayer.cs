using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SwipePlayer : BehaviourState_BaseClass
{

    private LayerMask layerMask;
    private GameObject playerobject;
    private bool completedSwipe = false;
    private float totalWindupTime = 2;
    private float currentWindUpTime;
    private Vector2 AttackSize = Vector2.one;
    private Vector3 playerlocation;

    [SerializeField] private float hitRange = 1f;

    public override void EnterBehaviourState(EnemyActionManager actionManager)
    {
        layerMask = LayerMask.GetMask("Interactable");

        enemyActionManagerRef = actionManager;
        enemyActionManagerRef.isFixedUpdateState = false;
        playerobject = enemyActionManagerRef.playerObject;
        enemyActionManagerRef.CurrentStateEnum = BehaviourState.SwipeAttack;
        completedSwipe = true;
        StartCoroutine(SwipeMethod());
    }

    public override bool ActiveBehaviourState()
    {
        return completedSwipe;
    }

    private IEnumerator SwipeMethod()
    {
        playerlocation = playerobject.transform.position;
        Debug.Log("Starting swipe attack method");
        currentWindUpTime = 0f;
        while(currentWindUpTime < totalWindupTime)
        {
            yield return new WaitForSeconds(0.1f);
            currentWindUpTime += 0.1f;    
        }

        AttackEnemy();
        yield return null;
    }

    private void AttackEnemy()
    {
        enemyActionManagerRef.animator.SetTrigger("slashing");
        Vector3 hitvector = (playerlocation - transform.position).normalized * hitRange;
        Collider2D[] attackColliders = Physics2D.OverlapBoxAll((Vector2)(transform.position + hitvector), enemyActionManagerRef.attackStats.AttackSize, 0f);

        foreach(Collider2D hit in attackColliders)
        {
            Debug.Log("attempting to hit: " + hit.gameObject + " at : " + (transform.position + hitvector));

            Health_BaseClass hitObj = hit.gameObject.GetComponent<Health_BaseClass>();
            Debug.Log(" hit gives " + hitObj);

            if(hitObj != null && hit != null && hit.gameObject.CompareTag("Player"))
            {
                Debug.Log("Found dammageable to attack" + hitObj);
                hitObj.Damaged(enemyActionManagerRef.attackStats, transform.position);
            }
            else
            {
                Debug.Log("hit object could not be found:" + hitObj);
            }

        }

        //Debug.Log("completed swipe attack method");
        completedSwipe = false;
    }

    public override void ExitBehaviourState()
    {
        StartCoroutine(enemyActionManagerRef.WaitMethod());
    }
}
