using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
//using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    [SerializeField] private InventoryManager inventory;
    [SerializeField] private GameObject InventoryPanelObject;

    [SerializeField] private List<UI_itemPanel> UI_InventoryPanels;
    [SerializeField] private List<UI_itemPanel> UI_HotInventoryPanels;

    private bool OpenInventoryBool = true;

    void Start()
    {
        OpenInventoryBool = false;
        InventoryPanelObject.SetActive(false);
    }

    public bool InventoryinputButton()
    {
        if(!OpenInventoryBool)
        {
            Debug.Log("Player opened the inventory panel");
            OpenInventoryBool = true;
            OpenInventoryPanel();
            return true;
        }
        else
        {
            Debug.Log("Player closed the inventory panel");
            OpenInventoryBool = false;
            InventoryPanelObject.SetActive(false);
            return false;
        }
    }

    private void OpenInventoryPanel()
    {
        InventoryPanelObject.SetActive(true);
        inventory.UpdateLookup();

        for(int i = 0 ; i < UI_InventoryPanels.Count; i++)
        {
            if(inventory.InventoryList[i].itemType != null)
            {
                UI_InventoryPanels[i].UpdatePanel(inventory.InventoryList[i]);
            }
            else
            {
                UI_InventoryPanels[i].EmptyPanel();
            }
        }
    }

}
