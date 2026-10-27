using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Data.Common;

public class InventoryManager : MonoBehaviour
{
    // allows quick lookup of items based on name to get their index in the list
    private Dictionary<Item, int[]> InventoryLookup = new Dictionary<Item, int[]>();

    //list of items that make up the inventory
    [HideInInspector] public int inventorySize = 10;
    public List<InstantiatedItem> InventoryList = new List<InstantiatedItem>();
    public List<InstantiatedItem> HotInventoryList = new List<InstantiatedItem>();
    private int hotListsize = 3;
    public int itemIndexOffset = 0;
    public InstantiatedItem helditem;

    private void Awake()
    {
        UpdateLookup();
    }

    #region Updating
    public void UpdateInventorySize()
    {
        if(InventoryList.Count < inventorySize)
        {
            for(int I = 0 ; I < inventorySize; I++) // goes through each item slot in the inventory
            {
                InventoryList.Add(null);
                Debug.Log("Inventory size has been updated to: " + InventoryList.Count);

                if(InventoryList.Count == inventorySize)
                {
                    Debug.Log("Inventory has reach max size of: " + InventoryList.Count + " which should be equal to: " + inventorySize);
                    return;
                }
                
            }

            Debug.Log("max items have been added to the inventory without reaching max value");
        }
    }
    
    private void UpdatingHotlist()
    {
        if(HotInventoryList.Count < hotListsize)
        {
            for(int I = 0 ; I < hotListsize; I++) // goes through each item slot in the inventory
            {
                HotInventoryList.Add(null);
                Debug.Log("Hot Inventory size has been updated to: " + HotInventoryList.Count);

                if(HotInventoryList.Count == hotListsize)
                {
                    Debug.Log("Hot Inventory has reach max size of: " + HotInventoryList.Count + " which should be equal to: " + hotListsize );
                    return;
                }
                
            }

            Debug.Log("max items have been added to the inventory without reaching max value");
        }
    }

    public void UpdateLookup()
    {
        //UpdateHotPanels();
        InventoryLookup.Clear();
        for(int I = 0 ; I < InventoryList.Count; I++) // goes through each item slot in the inventory
        {
            Item item = InventoryList[I].itemType;
            if(item != null)
            {
                if(InventoryLookup.ContainsKey(item))
                {
                    bool confirmed = false;
                    foreach(int index in InventoryLookup[item])
                    {
                        if(I == index)
                        {
                            confirmed = true;
                            break;
                        }
                    }

                    if(confirmed == false)
                    {
                        int[] newArray = new int[InventoryLookup[item].Length + 1];
                        int[] dictArray = InventoryLookup[item];

                        for(int i = 0 ; i < dictArray.Length; i++)
                        {
                            newArray[i] = dictArray[i];
                        }

                        newArray[InventoryLookup[item].Length] = I;
                        InventoryLookup[item] = newArray;
                        Debug.Log("item ( " + item.ItemName + ") has been updated in lookup to add index: " + I );
                    }
                }
                else
                {
                    int[] newArray = new int[1];
                    newArray[0] = I;
                    InventoryLookup.Add(item, newArray);
                    Debug.Log("New item (" + item.ItemName + ") added to lookup at: " + I);
                }       
            }
        }

        foreach(KeyValuePair<Item, int[]> item in InventoryLookup)
        {
            string text = item.Key.ItemName + ": all indexes include: ";
            foreach(int index in item.Value)
            {
                text += index + ", ";
            }

            Debug.Log(text);
        }
    }

    public void UpdateHotPanels()
    {
        for (int i = 0; i < HotInventoryList.Count; i++)
        {
            HotInventoryList[i] = new InstantiatedItem(i);
        }
    }

    #endregion
    #region Manipulate

    // attempts to add an item to a stack, or create a new stack in inventory. returns how many DID NOT enter the inventory
    public int AddItemToInventory(InstantiatedItem newItem) 
    {
        UpdateLookup();
        int newItemCount = newItem.currentStackSize;
        if(InventoryLookup.ContainsKey(newItem.itemType))    // checks if there is a reference to the item already in the inventory
        {
            foreach(int index in InventoryLookup[newItem.itemType])  // goes through each stack of the item
            {
                InstantiatedItem InventoryItem = InventoryList[index];
                if(InventoryItem.currentStackSize != InventoryItem.itemType.stackSizeLimit)  // checks if the stack is full
                {
                    int combinedSize = newItemCount + InventoryItem.currentStackSize;

                    if(combinedSize > InventoryItem.itemType.stackSizeLimit) // checks if the combined size is greater than the stack size
                    {
                        InventoryItem.currentStackSize = InventoryItem.itemType.stackSizeLimit;
                        newItemCount = combinedSize - InventoryItem.currentStackSize ;
                        Debug.Log("filled a stack with " + newItemCount + " left over");
                    }
                    else    // combined size fits in the stack
                    {
                        InventoryItem.currentStackSize = combinedSize;  // adds the items to the list 
                        Debug.Log("all items added successfully");
                        return 0;   // returns 0 as all items were collected and added to inventory
                    }
                }
            }
        }

        while(newItemCount > 0) // makes sure to sort out all the new items
        {

            for(int I = 0 ; I < inventorySize; I++) // goes through each item slot in the inventory
            {
                if(InventoryList[I].itemType != null)    // skips if there is already an item in that slot
                {
                    Debug.Log("Stack " + I + " has non-applicable items: " );
                    continue;
                }

                InventoryList[I].itemType = newItem.itemType;                     // adds the item to the inventory
                InventoryList[I].currentStackSize = newItemCount;        // sets the item current for the new stack

                if(newItemCount > InventoryList[I].itemType.stackSizeLimit)       // checks if the additional items surpass the stack size limit
                {
                    Debug.Log("items filled a new stack");
                    // gets the value of how much it goes over the stack size limit
                    newItemCount = InventoryList[I].currentStackSize - newItem.itemType.stackSizeLimit;     
                    InventoryList[I].currentStackSize = newItem.itemType.stackSizeLimit;  // sets the stack to full
                }
                else
                {
                    newItemCount = 0;
                    break;
                }
            }

        }
        Debug.Log("All items added");
        return 0;
    }

    public bool RemoveItemFromInventory(InstantiatedItem item, int removeAmount)
    {  
        int tempSize = item.currentStackSize; // gets the current amount of items in the stack   
        
        if(tempSize < removeAmount) // checks if there is enough to remove in just the single stack
        {
            removeAmount -= item.currentStackSize;   // removes items from the total amount

            UpdateLookup();
            // creates an array to hold the references to the instance items that must be fully removed from inventory
            InstantiatedItem[] cullList = new InstantiatedItem[InventoryLookup[item.itemType].Length];
            int cullcount = 1;  // index for where to put next culled item
            cullList[0] = item; // adds reference for base instance item to cull list

            foreach(int index in InventoryLookup[item.itemType])    // checks for other objects of the same type in the inventory
            {
                InstantiatedItem invItem = InventoryList[index];    // gets the reference to the item
                if(invItem == item) // checks if the chosen index is referencing the original one, and avoids duplicate references
                {
                    continue;
                }

                tempSize += invItem.currentStackSize;   // adds the new items stack size to the current pool to be removed from
                
                if(tempSize < removeAmount) // checks if another stack is needed to remove items from
                {
                    cullList[cullcount] = InventoryList[index];   // adds object to the list to be removed
                    cullcount ++;   // increases index for next culled item
                    removeAmount -= invItem.currentStackSize;   // removes items from the total amount
                    continue;   
                }
                else    // enough stacks item amount's have been found to fully remove items
                {
                    invItem.currentStackSize -= removeAmount;   
                    DeleteItem(cullList);
                    return true;
                }

            }

            Debug.Log("Not enough items in stacks have been found");
            return false;
        }

        if(item.currentStackSize == removeAmount)   // checks if it will remove all items from stack
        {
            item.itemType = null;
            item.currentStackSize = 0;
        }
        else
        {
            item.currentStackSize -= removeAmount;
        }

        Debug.Log("Enough items in single stack");
        return true;

    }  

    private void DeleteItem(InstantiatedItem[] items)   // deletes a collection of stacks
    {
        foreach(InstantiatedItem item in items)
        {
            if(item == null)
            {
                continue;
            }

            item.itemType = null;
            item.currentStackSize = 0;
        }

    }

    public void SwapItems(int firstIndex, int secondIndex)  // swaps the location of 2 different stacks in the inventory
    {
        InstantiatedItem tempItem = InventoryList[firstIndex];
        InventoryList[firstIndex] = InventoryList[secondIndex];
        InventoryList[secondIndex] = tempItem;

        UpdateLookup();
        //GameManager.Instance.UIManagerSingleton.SwapSelected(firstIndex, secondIndex);
        GameManager.Instance.UIManagerSingleton.UpdatePanels();
    }

    public void CombineStacks(int dragStackIndex, int targetStackIndex) // combines the stacks of 2 items of the same type into 1 stack
    {
        InstantiatedItem dragItem = InventoryList[dragStackIndex];
        InstantiatedItem targetItem = InventoryList[targetStackIndex];

        int totalsize = dragItem.currentStackSize + targetItem.currentStackSize;

        if(totalsize > targetItem.itemType.stackSizeLimit)  // checks if items overflow
        {
            dragItem.currentStackSize = totalsize - targetItem.itemType.stackSizeLimit; // gives remainder to original stack
            targetItem.currentStackSize = targetItem.itemType.stackSizeLimit;   // fills the target stack
        }
        else
        {
            targetItem.currentStackSize = totalsize;
            
            dragItem.itemType = null;
            dragItem.currentStackSize = 0;
        }
        
        if(dragItem.selectedItemBool == true)
        {
            dragItem.selectedItemBool = false;
            targetItem.selectedItemBool = true;
        }

        UpdateLookup();
        GameManager.Instance.UIManagerSingleton.UpdatePanels();
    }

    #endregion
    #region HotItems

    public void SetHotItem(int InventoryIndex, int HotIndex)
    {
        HotInventoryList[HotIndex] = InventoryList[InventoryIndex];
        UpdateLookup();
        GameManager.Instance.UIManagerSingleton.UpdatePanels();
    }

    public void ScrollHotItems(float scrollValue)
    {
        itemIndexOffset -= (int)scrollValue;

        if(itemIndexOffset >= HotInventoryList.Count)
        {
            itemIndexOffset = 0;
        }
        if(itemIndexOffset <= -1)
        {
            itemIndexOffset = HotInventoryList.Count - 1;
        }

        helditem = HotInventoryList[itemIndexOffset];
    }

    #endregion

}
