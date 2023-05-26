using System.IO;
using UnityEngine;
using System.Collections.Generic;

public class SaveSystem : MonoBehaviour
{
    public Movement player;
    public InventoryManager inventoryManager;
    public ItemDatabase itemDatabase; // Reference to the ItemDatabase

    private const string PlayerDataPath = "/PlayerDatas.json";

    public void SavePlayer()
    {
        PlayerData data = new PlayerData();
        data._health = player._health;
        data.moveSpeed = player.moveSpeed;
        data.playerPos = player.transform.position;

        // Save inventory items
        foreach (var slot in inventoryManager.inventorySlots)
        {
            var item = slot.GetComponentInChildren<InventoryItem>();
            if (item != null)
            {
                InventoryItemData itemData = new InventoryItemData();
                itemData.itemName = item.item.name;
                itemData.durability = item.durability;
                itemData.count = item.count;
                data.inventoryItems.Add(itemData);
                Debug.Log("Item: " + itemData.itemName);
            }
        }

        string json = JsonUtility.ToJson(data);
        string savePath = Application.persistentDataPath + PlayerDataPath;
        File.WriteAllText(savePath, json);

        Debug.Log("Player saved!");
        Debug.Log("Health: " + data._health);
        Debug.Log("Move Speed: " + data.moveSpeed);
        Debug.Log("Position: " + data.playerPos);
        Debug.Log("Inventory Items saved: " + data.inventoryItems.Count);
    }

    public void LoadPlayer()
    {
        string path = Application.persistentDataPath + "/PlayerDatas.json";

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            PlayerData data = JsonUtility.FromJson<PlayerData>(json);

            player._health = data._health;
            player.moveSpeed = data.moveSpeed;
            player.transform.position = data.playerPos;

            inventoryManager.ClearInventory();

            Debug.Log("Inventory Items Count: " + data.inventoryItems.Count);

            foreach (var itemData in data.inventoryItems)
            {
                Item item = itemDatabase.GetItemByName(itemData.itemName); // Use the ItemDatabase reference to get the item
                Debug.Log("Passed: " + item);
                if (item != null)
                {
                    bool itemAdded = inventoryManager.AddItem(item, itemData.durability, itemData.count);
                    Debug.Log("Item: " + item.name);
                    if (!itemAdded)
                    {
                        Debug.LogWarning("No available slots to add the item: " + itemData.itemName);
                    }
                }
            }

            Debug.Log("Player loaded!");
            Debug.Log("Health: " + data._health);
            Debug.Log("Move Speed: " + data.moveSpeed);
            Debug.Log("Position: " + data.playerPos);
            Debug.Log("Inventory Items loaded: " + data.inventoryItems.Count);
        }
        else
        {
            Debug.Log("No player data found.");
        }
    }

}
