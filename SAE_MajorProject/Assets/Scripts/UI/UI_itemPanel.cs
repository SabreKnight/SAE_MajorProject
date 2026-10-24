using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public class UI_itemPanel : MonoBehaviour, 

IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler, IDropHandler
{

#region References

    public int inventoryIndexInt;
    private UIManager UIManagerReference;
    private GameObject tempDragObject;
    public InstantiatedItem instantiatedItem;
    public TextMeshProUGUI StackSizeText; 
    [SerializeField] private TextMeshProUGUI HotkeyStackSize; 
    public Image iconimage;
    public Image outlinePanel;
    private bool SelectedBool;

#endregion

    void OnEnable()
    {
        if(UIManagerReference == null)
        {
            UIManagerReference = GameManager.Instance.UIManagerSingleton;
        }
    }

    public void UpdatePanel(InstantiatedItem item)
    {
        instantiatedItem = item;
        StackSizeText.text = instantiatedItem.currentStackSize.ToString();
        iconimage.sprite = instantiatedItem.itemType.inventorySprite;
        iconimage.color = Color.white;
    }

    public void EmptyPanel()
    {
        instantiatedItem = null;
        StackSizeText.text = "";
        iconimage.sprite = null;

        iconimage.color = Color.clear;
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
            }
            else
            {
                outlinePanel.color = Color.black;
            }
        }
    }   

#region Dragging

    public void OnBeginDrag(PointerEventData eventData) // this begins the UI dragging data
    {
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
        Debug.Log("Dragging " + eventData.pointerDrag);
        tempDragObject.transform.position = Input.mousePosition;    // temp UI object follows mouse position
    }
    public void OnEndDrag(PointerEventData eventData)   // this is the end of dragging data
    {
        DestroyImmediate(tempDragObject);   // destroys the temp UI object
        //Debug.Log("End drag");  
    }
    public void OnDrop(PointerEventData eventData)  // when an object has been dropped ontop of it
    {
        if(eventData.pointerDrag.GetComponent<UI_itemPanel>().instantiatedItem == null || eventData.pointerDrag.GetComponent<UI_itemPanel>() == this) // cancels drop if there is no item being dragged
        {
            Debug.Log("Attempting to drop null item");
            return;
        }

        Debug.Log(eventData.pointerDrag + " Has been dropped ontop of " + this.gameObject);
        UI_itemPanel droppedItemPanel = eventData.pointerDrag.GetComponent<UI_itemPanel>(); // gets reference to object being dropped

        if(instantiatedItem == null || droppedItemPanel.instantiatedItem.itemType != instantiatedItem.itemType) // items are not the same
        {
            UIManagerReference.inventory.SwapItems(droppedItemPanel.inventoryIndexInt, inventoryIndexInt);
            return;
        }
        else // items are the same and attempts to merge the two
        {
            UIManagerReference.inventory.CombineStacks(droppedItemPanel.inventoryIndexInt, inventoryIndexInt);
        }
        
    }

#endregion

    public void OnPointerClick(PointerEventData eventData)  // this is when somethign has been clicked
    {
        Debug.Log("Clicked UI interface");
    }
}
