using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_itemPanel : MonoBehaviour
{
    public InstantiatedItem instantiatedItem;
    public TextMeshProUGUI StackSizeText; 
    public Image iconimage;
    public Image outlinePanel;

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
    
}
