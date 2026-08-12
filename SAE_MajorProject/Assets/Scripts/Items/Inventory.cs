using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class InventoryManager : MonoBehaviour
{
    // allows quick lookup of items based on name to get their index in the list
    private Dictionary<Item, int[]> InventoryLookup = new Dictionary<Item, int[]>();

    //list of items that make up the inventory
    private int inventorySize = 10;
    private List<Item> InventoryList = new List<Item>();

    private List<Item> HotInventoryList = new List<Item>();
    private int hotListsize = 3;
    private int itemIndex = 0;

    // attempts to add an item to a stack, or create a new stack in inventory. returns how many DID NOT enter the inventory
    public int AddItemToInventory(Item newItem, int newItemCount) 
    {
        if(InventoryLookup.ContainsKey(newItem))    // checks if there is a reference to the item already in the inventory
        {
            foreach(int index in InventoryLookup[newItem])  // goes through each stack of the item
            {
                Item InventoryItem = InventoryList[index];
                if(InventoryItem.CurrentStackSize != InventoryItem.stackSizeLimit)  // checks if the stak is full
                {
                    int combinedSize = newItemCount + InventoryItem.CurrentStackSize;

                    if(combinedSize > InventoryItem.stackSizeLimit) // checks if the combined size is greater than the stack size
                    {
                        InventoryItem.CurrentStackSize = InventoryItem.stackSizeLimit;
                        newItemCount = InventoryItem.CurrentStackSize - combinedSize;
                    }
                    else    // combined size fits in the stack
                    {
                        InventoryItem.CurrentStackSize = combinedSize;  // adds the items to the list 
                        return 0;   // returns 0 as all items were collected and added to inventory
                    }
                }
            }
        }

        while(newItemCount > 0) // makes sure to sort out all the new items
        {
            if(InventoryList.Count == inventorySize)    // checks if the inventory is full 
            {
                return newItemCount;    // doesn't add any of the items
            }

            foreach(Item item in InventoryList);
            {
                
                item = Item.CreateInstance<newItem>;
                item.CurrentStackSize = newItemCount;

                if(newItemCount > item.stackSizeLimit)
                {
                    //item
                }
                
            }

            return newItemCount;
        }
        return 0;





    }


}
