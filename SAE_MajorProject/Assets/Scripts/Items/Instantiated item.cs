using UnityEngine;

[System.Serializable]
public class InstantiatedItem
{
    // this stores the data for an instance of a type of object within the game.
    public Item itemType;   // this stores the item's information
    public int currentStackSize;    // this stores the current stack size of the item
    public int HotInventoryIndex;   // this stores the index for whic hotInventory slot it links to
    public bool selectedItemBool = false; 
    public int quotaCount;

    public InstantiatedItem(Item item, int size, int HotIndex)
    {
        itemType = item;
        currentStackSize = size;
        HotInventoryIndex = HotIndex;
        quotaCount = HotIndex;
    }

    public InstantiatedItem(Item item, int size)
    {
        itemType = item;
        currentStackSize = size;
        HotInventoryIndex = 0;
    }

    public InstantiatedItem(Item item)
    {
        itemType = item;
        currentStackSize = 1;
        HotInventoryIndex = 0;
    }

    public InstantiatedItem(int HotIndex)
    {
        itemType = null;
        currentStackSize = 0;
        HotInventoryIndex = HotIndex;
    }

    public InstantiatedItem()
    {
        itemType = null;
        currentStackSize = 0;
        HotInventoryIndex = 0;
    }

    
}