using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

public class InteractionManager : MonoBehaviour    // attach to player object
{
    public GameObject hitMarker;
    public Vector2 hitMarkerPosition;
    [SerializeField] private Grid grid;
    [SerializeField] public InventoryManager inventory;

    [Header("Tool behaviours")]
    public Item helditem;
    public GameObject ToolBehaviourManagerObject;   // this is a prefab which holds the tool behaviour components
    [SerializeField] private ToolBehaviours_BaseClass leftToolbehaviour;
    [SerializeField] private ToolBehaviours_BaseClass rightToolbehaviour;
    private List<I_HitObject> hitMarkerOutputArray;

    private Vector3 markerOffset = new Vector3(0.5f, 0.5f, 0);

    public void LeftClick()
    {
        if(helditem == null || ToolBehaviourManagerObject == null)  // checks if there is an object and tool behaviour to use
        {
            return;
        }

        switch(helditem.type)   // checks what type of object is beign used 
        {
            case UseType.Tool:  // currently held object is a tool
            {
                //Debug.Log("Player attempted to use the Tool: " + helditem.ItemName);
                hitMarkerOutputArray = leftToolbehaviour.ToolClickMethod(hitMarkerPosition);
                UseTools(hitMarkerOutputArray);
                return;
            }
            default:
            {
                Debug.Log("No specified item has been selected");
                return;
            }
        }
    }

    public void UseTools(List<I_HitObject> hitarray)
    {
        if(hitarray == null )
        {
            Debug.Log("No hittable objects found in array");    
        }

        for (int i = 0; i < hitarray.Count; i++)
        {
            //Debug.Log();
            I_HitObject hit = hitarray[i];
            if(hit != null)
            {
                Debug.Log("using: " + helditem + " to hit: "  + hit);
                hit.HitObjectMethod(helditem, transform.position);
            }
            else
            {
                Debug.Log("No hittable object reference to use tool on: " + hitarray[i]);
            }
        }
    }

    public void MoveHitmarker(Vector2 LookDirection)
    {
        Vector3Int LookVectInt = (Vector3Int)Vector2Int.RoundToInt(LookDirection);
        Vector3Int hitVector =  grid.WorldToCell(this.gameObject.transform.position) + LookVectInt;
        hitMarkerPosition = (Vector3)hitVector + markerOffset;
        hitMarker.transform.position = hitMarkerPosition;
    }

    public bool InteractkeyMethod()
    {
        Collider2D[] collArray = new Collider2D[10];
        collArray = Physics2D.OverlapBoxAll((Vector2)hitMarkerPosition, Vector2.one, 0f);

        Debug.Log("Attempting to interact with object");
        foreach(Collider2D coll in collArray)
        {
            if(coll.gameObject.TryGetComponent<I_InteractObject>(out I_InteractObject hitObj) && coll != null)
            {
                return hitObj.InteractObjectMethod();
            }
        }
        
        Debug.Log("No object to interact with");
        return false;
    }

    public void DropheldObject()
    {
        if(inventory.helditem == null || inventory.helditem.itemType == null)
        {
            Debug.Log("Attempting to drop no held item");
            return;
        }

        InstantiatedItem dropitem = new InstantiatedItem(helditem, inventory.helditem.currentStackSize);

        if(inventory.RemoveItemFromInventory(inventory.helditem, dropitem.currentStackSize))
        {
            //Debug.Log(dropitem.currentStackSize + " number of " + helditem.ItemName + " has been dropped by player");
            GameObject DropObject = new GameObject("DroppedObject", typeof(PickUpItem));
            DropObject.GetComponent<PickUpItem>().SetupObject(dropitem, Mathf.Sign(hitMarkerPosition.x - transform.position.x), transform.position);
            Debug.Log(dropitem.currentStackSize + " number of " + helditem.ItemName + " has been dropped by player");
        }
        
    }

    public void SetheldObject(Item newHeld)
    {
        Debug.Log("attempting to set new held object in interaction window");
        if(newHeld == helditem)
        {
            Debug.Log("new held object is the same as old held object");
            return;
        }

        if(ToolBehaviourManagerObject != null)
        {
            Debug.Log("removing tool behaviour: " + ToolBehaviourManagerObject);
            DestroyImmediate(ToolBehaviourManagerObject);

            ToolBehaviourManagerObject = null;
            leftToolbehaviour = null;
            rightToolbehaviour = null;
        }

        if(newHeld == null)
        {
            Debug.Log("new held object being set is a null object");
            helditem = null;
            return; 
        }

        helditem = newHeld;
        Debug.Log("new held object in interaction window is: " + helditem);

        if(newHeld.ToolbehaviourManagerObject == null)
        {
            Debug.Log("New object does not have tool behaviours");
            return;
        }

        ToolBehaviours_BaseClass[] toolbehaviour = newHeld.ToolbehaviourManagerObject.GetComponents<ToolBehaviours_BaseClass>();
        Debug.Log("new tool behaviours are: " + toolbehaviour[0]);

        if(toolbehaviour[0] != leftToolbehaviour && toolbehaviour[0] != rightToolbehaviour)
        {
            ToolBehaviourManagerObject = Instantiate(newHeld.ToolbehaviourManagerObject, transform.position, Quaternion.identity, this.transform);

            foreach(ToolBehaviours_BaseClass behaviour in toolbehaviour)
            {
                if(behaviour.PrimaryAction)
                {
                    leftToolbehaviour = behaviour;
                    Debug.Log("swapped primary tool behaviour to: " + behaviour);
                }
                else
                {
                    rightToolbehaviour = behaviour;
                    Debug.Log("swapped Secondary tool behaviour to: " + behaviour);
                }
            }
        }
    }


}
