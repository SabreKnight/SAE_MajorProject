using UnityEngine;

public class InteractionManager : MonoBehaviour    // attach to player object
{
    public GameObject hitMarker;
    public Vector2 hitMarkerPosition;
    [SerializeField] private Grid grid;
    [SerializeField] public InventoryManager inventory;
    public Item helditem;
    public GameObject ToolBehaviourManagerObject;   // this is a prefab which holds the tool behaviour components
    [SerializeField] private ToolBehaviours_BaseClass leftToolbehaviour;
    [SerializeField] private ToolBehaviours_BaseClass rightToolbehaviour;
    
    //private I_HitObject[] hitMarkerOutputArray = new I_HitObject[10];

    private Vector3 markerOffset = new Vector3(0.5f, 0.5f, 0);

    public void LeftClick()
    {
        if(helditem == null || ToolBehaviourManagerObject == null)
        {
            return;
        }

        switch(helditem.type)
        {
            case UseType.Tool:
            {
                Debug.Log("Player attempted to use the Tool: " + helditem.ItemName);
                I_HitObject[] hitMarkerOutputArray = leftToolbehaviour.ToolClickMethod(hitMarkerPosition);
                return;
            }
            default:
            {
                Debug.Log("No specified item has been selected");
                return;
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

    public void InteractkeyMethod()
    {

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

        if(ToolBehaviourManagerObject != null)
        {
            DestroyImmediate(ToolBehaviourManagerObject);
        }

        if(newHeld == null)
        {
            return; 
        }

        helditem = newHeld;

        if(newHeld.ToolbehaviourManagerObject == null )
        {
            Debug.Log("New object does not have tool behaviours");
            return;
        }

        ToolBehaviours_BaseClass[] toolbehaviour = newHeld.ToolbehaviourManagerObject.GetComponents<ToolBehaviours_BaseClass>();

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
