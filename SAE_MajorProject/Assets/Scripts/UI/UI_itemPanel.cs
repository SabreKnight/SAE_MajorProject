using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public class UI_itemPanel : MonoBehaviour, 

IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler, IDropHandler
{

#region References

    [SerializeField] private PanelType panelType = PanelType.OwnedPanel;
    public int inventoryIndexInt;
    private UIManager UIManagerReference;
    private GameObject tempDragObject;
    public InstantiatedItem instantiatedItem;
    public TextMeshProUGUI StackSizeText; 
    [SerializeField] private TextMeshProUGUI HotkeyStackSize; 
    public Image iconimage;
    public Image outlinePanel;
    [SerializeField] private bool SelectedBool;
    
    

#endregion
#region Updating
    void Start()
    {
        if(UIManagerReference == null)
        {
            UIManagerReference = GameManager.Instance.UIManagerSingleton;
        }
    }

    public void UpdatePanel(InstantiatedItem item)
    {
        //Debug.Log("item panel: " + inventoryIndexInt + " has been updated of type: " + panelType);
        instantiatedItem = item;
        iconimage.sprite = instantiatedItem.itemType.inventorySprite;
        iconimage.color = Color.white;

        if(panelType == PanelType.OwnedPanel)
        {
            BoolgetSet = instantiatedItem.selectedItemBool;
        }

        if(panelType == PanelType.ProgressPanel)
        {
            StackSizeText.text = instantiatedItem.currentStackSize.ToString() + "/" + instantiatedItem.quotaCount;

            if(instantiatedItem.currentStackSize >= instantiatedItem.quotaCount)
            {
                outlinePanel.color = Color.blue;
            }
        }
        else
        {
            StackSizeText.text = instantiatedItem.currentStackSize.ToString();
        }
        
    }

    public void EmptyPanel()
    {
        //Debug.Log("item panel: " + inventoryIndexInt + " has been NOT updated of type: " + panelType);
        instantiatedItem = new InstantiatedItem(instantiatedItem.HotInventoryIndex);
        StackSizeText.text = "";
        iconimage.sprite = null;

        iconimage.color = Color.clear;
        if(panelType != PanelType.GamePanel)
        {
            BoolgetSet = false;
        }
        
    }
    
    // need to create delegates and the like to notify UI manager of panel selection.

    public bool BoolgetSet
    {
        get
        {
            return SelectedBool;
        }

        set
        {
            if(value)
            {
                outlinePanel.color = Color.red;
                UIManagerReference.UI_HotInventoryPanels[instantiatedItem.HotInventoryIndex].instantiatedItem = instantiatedItem;
            }
            else
            {
                outlinePanel.color = Color.black;
            }
        }
    }   

#endregion
#region Dragging

    public void OnBeginDrag(PointerEventData eventData) // this begins the UI dragging data
    {
        if(panelType != PanelType.OwnedPanel)
        {
            Debug.Log("not a dragable panel");
            return;
        }

        tempDragObject = new GameObject("Dragged_UI_Object");   // creates a temporary UI object to be dragged
        if(instantiatedItem.itemType != null)   // checks if there is an object type
        {
            tempDragObject.AddComponent<Image>();   // creates an image component
            tempDragObject.GetComponent<Image>().raycastTarget = false;
            tempDragObject.GetComponent<Image>().sprite = instantiatedItem.itemType.inventorySprite; // adds a sprite to the image
        }
        tempDragObject.transform.SetParent(this.gameObject.transform.parent.parent, true);  // sets parent so it appears abpve UI panel
        //Debug.Log("Begin drag");
    }
    public void OnDrag(PointerEventData eventData)   // this is the active dragging data
    {
        if(tempDragObject == null)
        {
            return;
        }
        Debug.Log("Dragging " + eventData.pointerDrag);
        tempDragObject.transform.position = Input.mousePosition;    // temp UI object follows mouse position
    }
    public void OnEndDrag(PointerEventData eventData)   // this is the end of dragging data
    {
        if(tempDragObject != null)
        {
            DestroyImmediate(tempDragObject);   // destroys the temp UI object
        }

        tempDragObject = null;
        //Debug.Log("End drag");  
    }
    public void OnDrop(PointerEventData eventData)  // when an object has been dropped ontop of it
    {
        if(eventData.pointerDrag.GetComponent<UI_itemPanel>().instantiatedItem == null || eventData.pointerDrag.GetComponent<UI_itemPanel>() == this) // cancels drop if there is no item being dragged
        {
            Debug.Log("Attempting to drop null item");
            return;
        }

        if(eventData.pointerDrag.GetComponent<UI_itemPanel>().panelType != PanelType.OwnedPanel)
        {
            Debug.Log("Attempting to move object from panel that does not hold a moveable item");
            return;
        }

        UI_itemPanel droppedItemPanel = eventData.pointerDrag.GetComponent<UI_itemPanel>(); // gets reference to object being dropped

        switch(panelType)
        {
            case PanelType.OwnedPanel:
            {
                Debug.Log(eventData.pointerDrag + " Has been dropped ontop of " + this.gameObject + " in the inventory");

                if(instantiatedItem == null || droppedItemPanel.instantiatedItem.itemType != instantiatedItem.itemType) // items are not the same
                {
                    UIManagerReference.inventory.SwapItems(droppedItemPanel.inventoryIndexInt, inventoryIndexInt);
                }
                
                else // items are the same and attempts to merge the two
                {
                    UIManagerReference.inventory.CombineStacks(droppedItemPanel.inventoryIndexInt, inventoryIndexInt);
                    
                    if(droppedItemPanel.instantiatedItem.selectedItemBool == true)
                    {
                        UIManagerReference.inventory.SetHotItem(droppedItemPanel.inventoryIndexInt, instantiatedItem.HotInventoryIndex);
                    }
                }

                break;
            }
            case PanelType.HotPanel:
            {
                Debug.Log("Dragging onto a hot-panel in the inventory");

                if(droppedItemPanel.instantiatedItem.itemType != null)
                {
                    SetHotitem(droppedItemPanel, this);
                }

                break;
            }
            case PanelType.GamePanel:
            {
                Debug.Log("attempting to drag onto a game panel, which is invalid");
                break;
            }
            case PanelType.ProgressPanel:
            {
                if(instantiatedItem.currentStackSize >= instantiatedItem.quotaCount || instantiatedItem == null)
                {
                    Debug.Log("Progress quota for this item has been filled");
                    return;
                }

                int leftover = UIManagerReference.currentInteraction.DepositItem(droppedItemPanel.instantiatedItem, inventoryIndexInt);

                Debug.Log(leftover + " are left over from " + droppedItemPanel.instantiatedItem.currentStackSize);
                UIManagerReference.inventory.RemoveItemFromInventory(droppedItemPanel.instantiatedItem, droppedItemPanel.instantiatedItem.currentStackSize - leftover);

                break;
            }
        }
        
    }

    private void SetHotitem(UI_itemPanel droppedItemPanel, UI_itemPanel HotPanel)
    {
        if(droppedItemPanel.instantiatedItem.itemType != null)
        {
            if (HotPanel.instantiatedItem == null)
            {
                Debug.LogError("instantiated item reference on hot panel:");
                HotPanel.instantiatedItem = UIManagerReference.FindHotPanelItem(this);
            }

            droppedItemPanel.instantiatedItem.HotInventoryIndex = HotPanel.instantiatedItem.HotInventoryIndex;   // tells the dropped item which hot panel it is
            UIManagerReference.UI_InventoryPanels[inventoryIndexInt].instantiatedItem.selectedItemBool = false; // deactivates selection on old item
            droppedItemPanel.instantiatedItem.selectedItemBool = true; // activates the new panel
            HotPanel.instantiatedItem = droppedItemPanel.instantiatedItem;   // sets item in hotpanel
            HotPanel.inventoryIndexInt = droppedItemPanel.inventoryIndexInt;
            HotPanel.UIManagerReference.inventory.SetHotItem(droppedItemPanel.inventoryIndexInt, instantiatedItem.HotInventoryIndex);
            UpdatePanel(instantiatedItem);
        }
    }

#endregion

    public void OnPointerClick(PointerEventData eventData)  // this is when somethign has been clicked
    {
        Debug.Log("Clicked UI interface");
    }
}

public enum PanelType
{
    OwnedPanel = 0,
    HotPanel = 1,
    GamePanel = 2,
    ProgressPanel = 3
};