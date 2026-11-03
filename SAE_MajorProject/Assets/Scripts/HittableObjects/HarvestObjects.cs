using System.Collections;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class HarvestObjects : MonoBehaviour, I_HitObject
{
    
    [SerializeField] private float hitPoints = 10;
    [SerializeField] private float maxhitPoints = 10;
    [SerializeField] private Item effectorTool;
    [SerializeField] private Item itemDrop;
    [SerializeField] private int baseDropAmount = 15;
    [SerializeField] private int randDropVariation = 5;
    [SerializeField] private float RegenTime = 30f;
    private bool regenerating = false;
    
    public void HitObjectMethod(Item tool, Vector3 HitDirection)
    {
        Debug.Log("using: " + tool + " in harvest method");

        if(tool != effectorTool || regenerating == true)
        {
            Debug.Log("object hit with wrong tool");
            return;
        }

        float damage = tool.attackStats.damageStat;

        if(Random.Range(0f, 1f) <= tool.attackStats.CritChance)
        {
            damage *= tool.attackStats.critMultiplier;
        }
        
        Debug.Log(this.gameObject + " took: " + damage + " damage from: " + tool.ItemName);
        hitPoints -= damage;

        if(hitPoints <= 0)
        {
            Harvest();
        }
    }

    private void Harvest()
    {
        int dropAmount = Random.Range(baseDropAmount - randDropVariation, baseDropAmount + randDropVariation);
        InstantiatedItem dropItem = new InstantiatedItem(itemDrop, dropAmount);
        
        GameObject DropObject = new GameObject("HarvestDrops", typeof(PickUpItem));
        int dropDirection = ((int)Random.Range(0,2) * 2) - 1;
        DropObject.GetComponent<PickUpItem>().SetupObject(dropItem, dropDirection, transform.position);

        Debug.Log(this.gameObject + " was harvested and dropped: " + dropAmount + " of " + itemDrop.ItemName);

        AnalyticsManager.Instance.harvestCount ++;
        StartCoroutine(RegenerationMethod());
    }

    private IEnumerator RegenerationMethod()
    {
        Collider2D coll = GetComponent<Collider2D>();
        coll.enabled = false;

        SpriteRenderer SR = GetComponent<SpriteRenderer>();
        SR.enabled = false;

        yield return new WaitForSeconds(RegenTime);

        coll.enabled = true;
        SR.enabled = true;
        hitPoints = maxhitPoints;
    }

}
