using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_itemPanel : MonoBehaviour
{
    public InstantiatedItem instantiatedItem;
    public TextMeshProUGUI StackSizeText; 
    [SerializeField] private TextMeshProUGUI HotkeyStackSize; 
    public Image iconimage;
    public Image outlinePanel;
    private bool SelectedBool;

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
    
}
