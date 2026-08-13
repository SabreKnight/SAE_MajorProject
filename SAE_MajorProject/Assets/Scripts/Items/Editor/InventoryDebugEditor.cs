using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Collections;

public class InventoryDebugEditor : EditorWindow
{

    private static InventoryManager inventoryScript;
    private static Item CurrentItemType;
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

        if(CurrentItemType != null)
        {
            AddItems();
        }

    }

    void ListItems()
    {
        
        foreach(Item item in inventoryScript.InventoryList)
        {
            
            GUILayout.Space(spacingSize);
            EditorGUILayout.BeginHorizontal();
            if(item == null)
            {
                EditorGUILayout.LabelField("Null item");
            }
            else
            {
                EditorGUILayout.LabelField("Item:  " + item.ItemName); // displays enemy name
                EditorGUILayout.LabelField("Count:  " + item.CurrentStackSize);  // displays kill count
            }
            EditorGUILayout.EndHorizontal();
        }
    }

    void AddItems()
    {
        GUILayout.Space(spacingSize);
        itemcount = EditorGUILayout.IntField("Item amount to add", itemcount);
        if (GUILayout.Button("Add Items"))
        {
            inventoryScript.AddItemToInventory(CurrentItemType, itemcount);
        }
    }
}
