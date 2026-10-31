using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class HarvestObjects : MonoBehaviour, I_HitObject
{
    
    [SerializeField] private float hitPoints = 10;
    [SerializeField] private Item effectorTool;
    [SerializeField] private Item itemDrop;
    [SerializeField] private int baseDropAmount = 15;
    [SerializeField] private int randDropVariation = 5;
    

    public void HitObjectMethod(Item tool, Vector3 HitDirection)
    {
        Debug.Log("using: " + tool + " in harvest method");

        if(tool != effectorTool)
        {
            Debug.Log("object hit with wrong tool");
            return;
        }

        float damage = tool.damageStat;

        if(Random.Range(0f, 1f) <= tool.CritChance)
        {
            damage *= tool.critMultiplier;
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

        this.gameObject.SetActive(false);
    }

}
