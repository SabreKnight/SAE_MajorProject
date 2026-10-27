using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
//using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{

    public InventoryManager inventory;
    [SerializeField] private GameObject InventoryPanelObject;

    [SerializeField] public List<UI_itemPanel> UI_InventoryPanels;
    [SerializeField] public List<UI_itemPanel> UI_HotInventoryPanels;
    [SerializeField] private List<UI_itemPanel> UI_GameInventoryPanels;

    private bool OpenInventoryBool = true;

    public void OnEnable()
    {
        SetUpPanel();
    }
    
    public void SetUpPanel()
    {
        OpenInventoryBool = false;
        InventoryPanelObject.SetActive(false);

        for(int i = 0; i < UI_InventoryPanels.Count; i++)
        {
            UI_InventoryPanels[i].inventoryIndexInt = i;
        }
        for (int i = 0; i < UI_HotInventoryPanels.Count; i++)
        {
            UI_HotInventoryPanels[i].instantiatedItem = inventory.HotInventoryList[i];
        }
        
        for (int i = 0; i < UI_GameInventoryPanels.Count; i++)
        {
            UI_GameInventoryPanels[i].instantiatedItem = inventory.HotInventoryList[i];

            if(inventory.HotInventoryList[i].itemType != null)
            {
                UI_GameInventoryPanels[CalculateOffset(i)].UpdatePanel(inventory.HotInventoryList[i]);   // updates game UI to match hot panels
            }
            else
            {
                UI_GameInventoryPanels[CalculateOffset(i)].EmptyPanel();
            }
        }
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
        //inventory.UpdateHotPanels();
        UpdatePanels();
    }

    public InstantiatedItem FindHotPanelItem(UI_itemPanel panelref)
    {
        for (int i = 0; i < UI_HotInventoryPanels.Count; i++)
        {
            if(UI_HotInventoryPanels[i] == panelref)
            {
                return inventory.HotInventoryList[i];
            }
        }

        return null;
    }

    public void UpdatePanels()
    {
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

        for(int i = 0 ; i < UI_HotInventoryPanels.Count; i++)
        {
            if(inventory.HotInventoryList[i].itemType != null)
            {
                UI_HotInventoryPanels[i].UpdatePanel(inventory.HotInventoryList[i]);
                UI_GameInventoryPanels[CalculateOffset(i + 1)].UpdatePanel(inventory.HotInventoryList[i]);   // updates game UI to match hot panels
            }
            else
            {
                UI_HotInventoryPanels[i].EmptyPanel();
                UI_GameInventoryPanels[CalculateOffset(i + 1)].EmptyPanel();
            }
        }
    }

    private int CalculateOffset(int baseValue)
    {
        int HotOffset = baseValue - inventory.itemIndexOffset;
        if(HotOffset >= UI_GameInventoryPanels.Count)    // wraps the value so it doesn't go outside list index range
        {
            HotOffset -= UI_GameInventoryPanels.Count;
        }
        if(HotOffset < 0)
        {
            HotOffset += UI_GameInventoryPanels.Count;
        }

        return HotOffset;
    }

    public void SwapSelected(int firstIndex, int secondIndex)
    {
        bool firstSelected = UI_InventoryPanels[firstIndex].BoolgetSet;
        bool secondSelected = UI_InventoryPanels[secondIndex].BoolgetSet;

        UI_InventoryPanels[firstIndex].BoolgetSet = secondSelected;
        UI_InventoryPanels[secondIndex].BoolgetSet = firstSelected;

        UpdatePanels();
    }

    
    public void ScrollHotInventory(float scrollValue)
    {
        inventory.ScrollHotItems(scrollValue);
        UpdatePanels();

    }



}
