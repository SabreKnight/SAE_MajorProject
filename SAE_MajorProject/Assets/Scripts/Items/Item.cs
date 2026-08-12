using UnityEngine;

[CreateAssetMenu (fileName = "Item", menuName = "Inventory")]
public class Item : ScriptableObject
{
    public string ItemName;
    public Sprite InventorySprite;
    public int stackSizeLimit = 50;
    public int CurrentStackSize;


}
