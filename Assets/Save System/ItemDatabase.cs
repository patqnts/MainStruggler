using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
       
        // Debug log to check the items
        foreach (Item item in items)
        {
            Debug.Log("Item: " + item.name);
        }
    }
}
