using UnityEngine;

[CreateAssetMenu (fileName = "Item", menuName = "Inventory/Items")]
public class Item : ScriptableObject
{
    // this stores the data for a type of item
    public string ItemName;
    public UseType type;
    public Sprite InventorySprite;
    public int stackSizeLimit = 50;

    // scene object effector needed for tools

}

public enum UseType {
Material = 0,
Tool = 1,
Weapon = 2
};
