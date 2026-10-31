using UnityEngine;

[CreateAssetMenu (fileName = "Item", menuName = "Inventory/Items")]
public class Item : ScriptableObject
{
    // this stores the data for a type of item
    public string ItemName;
    public UseType type;
    public Sprite inventorySprite;
    public int stackSizeLimit = 50;

    [Header("Tool and Weapons")]
    public GameObject ToolbehaviourManagerObject;
    public float damageStat = 1;
    public float CritChance = 0.2f;
    public float critMultiplier = 2;
    public float altmultiplier = 1;
    public float KnockbackMult = 1;

    // scene object effector needed for tools

}

public enum UseType {
Material = 0,
Tool = 1,
Weapon = 2
};
