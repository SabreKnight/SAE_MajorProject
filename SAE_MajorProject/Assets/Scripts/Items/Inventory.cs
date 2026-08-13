using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class InventoryManager : MonoBehaviour
{
    // allows quick lookup of items based on name to get their index in the list
    private Dictionary<Item, int[]> InventoryLookup = new Dictionary<Item, int[]>();

    //list of items that make up the inventory
    private int inventorySize = 10;
    public List<Item> InventoryList = new List<Item>();

    private List<Item> HotInventoryList = new List<Item>();
    //private int hotListsize = 3;
    //private int itemIndex = 0;

    // attempts to add an item to a stack, or create a new stack in inventory. returns how many DID NOT enter the inventory
    public int AddItemToInventory(Item newItem, int newcount) 
    {
        int newItemCount = newcount;
        if(InventoryLookup.ContainsKey(newItem))    // checks if there is a reference to the item already in the inventory
        {
            foreach(int index in InventoryLookup[newItem])  // goes through each stack of the item
            {
                Item InventoryItem = InventoryList[index];
                if(InventoryItem.CurrentStackSize != InventoryItem.stackSizeLimit)  // checks if the stack is full
                {
                    int combinedSize = newItemCount + InventoryItem.CurrentStackSize;

                    if(combinedSize > InventoryItem.stackSizeLimit) // checks if the combined size is greater than the stack size
                    {
                        InventoryItem.CurrentStackSize = InventoryItem.stackSizeLimit;
                        newItemCount = InventoryItem.CurrentStackSize - combinedSize;
                        Debug.Log("filled a stack with " + newItemCount + " left over");
                    }
                    else    // combined size fits in the stack
                    {
                        InventoryItem.CurrentStackSize = combinedSize;  // adds the items to the list 
                        Debug.Log("all items added successfully");
                        return 0;   // returns 0 as all items were collected and added to inventory
                    }
                }
            }
        }

        while(newItemCount > 0) // makes sure to sort out all the new items
        {
            if(InventoryList.Count == inventorySize)    // checks if the inventory is full 
            {
                Debug.Log("Inventory is full");
                return newItemCount;    // doesn't add any of the items
            }

            for(int I = 0 ; I < inventorySize; I++) // goes through each item slot in the inventory
            {
                if(InventoryList[I] != null)    // skips if there is already an item in that slot
                {
                    Debug.Log("Stack " + I + " has non-applicable items");
                    continue;
                }

                Item itemobj = ScriptableObject.Instantiate(newItem);    // creates a new instances of the item

                InventoryList[I] = itemobj;                     // adds the item to the inventory
                itemobj.CurrentStackSize = newItemCount;        // sets the item current for the new stack

                if(newItemCount > itemobj.stackSizeLimit)       // checks if the additional items surpass the stack size limit
                {
                    Debug.Log("items filled a new stack");
                    // gets the value of how much it goes over the stack size limit
                    newItemCount = itemobj.CurrentStackSize - itemobj.stackSizeLimit;     
                    itemobj.CurrentStackSize = itemobj.stackSizeLimit;  // sets the stack to full
                }
            }

        }
        Debug.Log("All items added");
        return 0;
    }


}
