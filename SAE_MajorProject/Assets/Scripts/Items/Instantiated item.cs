using UnityEngine;

[System.Serializable]
public class InstantiatedItem
{
    // this stores the data for an instance of a type of object within the game.
    public Item itemType;
    public int currentStackSize;

    public InstantiatedItem(Item item, int size)
    {
        itemType = item;
        currentStackSize = size;
    }

    public InstantiatedItem(Item item)
    {
        itemType = item;
        currentStackSize = 1;
    }

    public InstantiatedItem()
    {
        itemType = null;
        currentStackSize = 0;
    }

    
}