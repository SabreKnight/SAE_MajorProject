// using UnityEngine;
// using UnityEditor;
// using System.Collections.Generic;
// using System.Collections;

// public class UserInterfaceDebugEditor : EditorWindow
// {
//     private static InstantiatedItem instantiatedItem;
//     private static UI_itemPanel itemPanel;
//     private int spacingSize = 3;

//     [MenuItem("Major Project/UI Debug")]
//     public static void ShowWindow()
//     {
//         UserInterfaceDebugEditor window = GetWindow<UserInterfaceDebugEditor>("User Interface Debug");
//         window.Show();
//     }

//     void OnGUI()
//     {
//         SetUp();
//     }

//     void Setup()
//     {
//         instantiatedItem = (InstantiatedItem)EditorGUILayout.ObjectField("Item Type", instantiatedItem, typeof(InstantiatedItem), false);
//         itemPanel = (UI_itemPanel)EditorGUILayout.ObjectField("Item Panel", itemPanel, typeof(UI_itemPanel), false);
//         InstantiatedItemField();
//         UIPanelUpdates();
//     }

//     void InstantiatedItemField()
//     {
//         GUILayout.Space(spacingSize);

//         if (GUILayout.Button("Create Custom instance class"))
//         {
//             InstantiatedItem = new InstantiatedItem();
//         }

//         if(instantiatedItem == null)
//         {
//             return;
//         }

//         GUILayout.Space(spacingSize);
//         EditorGUILayout.LabelField("Instantiated Item stats");
//         EditorGUILayout.BeginHorizontal();
//         InstantiatedItem.itemType = (Item)EditorGUILayout.ObjectField("Item Type", InstantiatedItem.itemType, typeof(Item), false);
//         InstantiatedItem.currentStackSize = EditorGUILayout.IntField("Current Stack Size", InstantiatedItem.currentStackSize);
//         EditorGUILayout.EndHorizontal();
//     }

//     void UIPanelUpdates()
//     {
//         GUILayout.Space(spacingSize);
//         if (GUILayout.Button("Update Panel with item content"))
//         {
//             itemPanel.UpdatePanel(instantiatedItem);
//         }

//         if (GUILayout.Button("Empty panel content"))
//         {
//             itemPanel.EmptyPanel();
//         }
//     }
// }

