using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Text;
public class ItemDatabase : MonoBehaviour
{
    [SerializeField]
    private Item[] items; // Array to store all the items

    // Singleton pattern to ensure only one instance of the database exists
    private static ItemDatabase instance;
    public static ItemDatabase Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<ItemDatabase>();
                if (instance == null)
                {
                    GameObject obj = new GameObject("ItemDatabase");
                    instance = obj.AddComponent<ItemDatabase>();
                }
            }
            return instance;
        }
    }

    public Item GetItemByName(string itemName)
    {
        foreach (Item item in items)
        {
            if (item.name == itemName)
            {
                return item;
            }
        }

        Debug.LogWarning("Item not found in the database: " + itemName);
        return null;
    }

    private void Awake()
    {
        StringBuilder itemText = new StringBuilder();

        // Append each item name to the StringBuilder
        foreach (Item item in items)
        {
            itemText.Append("Item: ").Append(item.name).Append(Environment.NewLine);
        }

        // Print the final text
        Debug.Log(itemText.ToString());
    }
}
