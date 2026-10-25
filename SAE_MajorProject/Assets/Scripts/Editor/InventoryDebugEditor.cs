using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Collections;

public class InventoryDebugEditor : EditorWindow
{

    private static InventoryManager inventoryScript;
    private static Item CurrentItemType;
    private static InstantiatedItem instantiatedItem;
    private static int itemcount; 
    private int spacingSize = 3;

    [MenuItem("Major Project/Inventory Debug")]
    public static void ShowWindow()
    {
            InventoryDebugEditor window = GetWindow<InventoryDebugEditor>("Inventory Debug");
    }

    void OnGUI()
    {
        Setup();
    }

    void Setup()
    {
        if(inventoryScript == null)
        {
            inventoryScript = (InventoryManager)FindFirstObjectByType(typeof(InventoryManager));
        }

        inventoryScript = (InventoryManager)EditorGUILayout.ObjectField("Inventory", inventoryScript, typeof(InventoryManager), true);

            CurrentItemType = (Item)EditorGUILayout.ObjectField("Item Type", CurrentItemType, typeof(Item), false);

            ListItems();
        
            AddItems();

            inventoryButtons();

        }

    void ListItems()
    {
        if(inventoryScript.InventoryList.Count < 1)
        {
            EditorGUILayout.LabelField("No inventory slots found");
        }

        foreach(InstantiatedItem item in inventoryScript.InventoryList)
    {
        
            GUILayout.Space(spacingSize);
            EditorGUILayout.BeginHorizontal();
            if(item.itemType == null)
            {
            EditorGUILayout.LabelField("Null item");
            }
            else
            {
            EditorGUILayout.LabelField("Item:  " + item.itemType.ItemName); // displays enemy name
            EditorGUILayout.LabelField("Count:  " + item.currentStackSize);  // displays kill count
            if (GUILayout.Button("Select item"))
            {
                instantiatedItem = item;
                CurrentItemType = item.itemType;
            }
            }

            EditorGUILayout.EndHorizontal();
        }
    }

    void AddItems()
    {
        GUILayout.Space(spacingSize);
        itemcount = EditorGUILayout.IntField("Item amount to add / remove", itemcount);
        if (GUILayout.Button("Add Items") && CurrentItemType != null)
        {
            InstantiatedItem newItem = new InstantiatedItem(CurrentItemType, itemcount, 0);
            int returnVal = inventoryScript.AddItemToInventory(newItem);
            if(returnVal > 0)
            {
            Debug.Log( returnVal + " items have not been added");
        }
            else
            {
                Debug.Log(" all items were added");
            }
        }

        if (GUILayout.Button("Remove Items") && instantiatedItem.itemType != null)
        {
            if(inventoryScript.RemoveItemFromInventory(instantiatedItem, itemcount) == true)
            {
                Debug.Log("all items successfully removed from inventory");
            }
        else
        {
                Debug.Log("Insufficient items to remove from inventory");
            }
        }
    }

    void inventoryButtons()
    {
        if (GUILayout.Button("Update Dictionary"))
        {
            inventoryScript.UpdateLookup();
        }

        if (GUILayout.Button("Update inventory count"))
        {
            inventoryScript.UpdateInventorySize();
        }

    }
}
