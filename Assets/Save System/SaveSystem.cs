using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class SaveSystem : MonoBehaviour
{
    public Movement player;
    public InventoryManager inventoryManager;
    public Cellular cellular;
    public ItemDatabase itemDatabase; // Reference to the ItemDatabase

    private const string PlayerDataPath = "/PlayerDatas.json";
    public int currentSaveSlotIndex = 0;

    public LoadSystem loadSystem;
    private void Start()
    {
        loadSystem = FindObjectOfType<LoadSystem>();
    }
    //public void SavePlayer(string profileId)
    public void SavePlayer(string profileId)
    {
        profileId = loadSystem.selectedProfileId;
        PlayerData data = new PlayerData();
        data._health = player._health;
        data.moveSpeed = player.moveSpeed;
        data.playerPos = player.transform.position;
        data.mapSeed = cellular.seedCodex;

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
        // Save equipment item in equipmentSlots[7]
        var equipmentItem = inventoryManager.equipmentSlots[7].GetComponentInChildren<InventoryItem>();
        if (equipmentItem != null)
        {
            InventoryItemData equipmentData = new InventoryItemData();
            equipmentData.itemName = equipmentItem.item.name;
            equipmentData.durability = equipmentItem.durability;
            equipmentData.count = equipmentItem.count;
            data.inventoryItems.Add(equipmentData);
            Debug.Log("Equipment Item: " + equipmentData.itemName);
        }


        string json = JsonUtility.ToJson(data, true);

        string directoryPath = Path.Combine(Application.persistentDataPath, profileId);
        Directory.CreateDirectory(directoryPath);

        string filePath = Path.Combine(directoryPath, Path.GetFileName(PlayerDataPath));
        File.WriteAllText(filePath, json);






        Debug.Log(Application.persistentDataPath);
        Debug.Log("Player saved!");
        Debug.Log("Health: " + data._health);
        Debug.Log("Move Speed: " + data.moveSpeed);
        Debug.Log("Position: " + data.playerPos);
        Debug.Log("Inventory Items saved: " + data.inventoryItems.Count);
        Debug.Log("Map seedcode: " + data.mapSeed);
    }

    public void BackToMenu()
    {
        // Destroy the LoadSystem instance
        LoadSystem loadSystem = FindObjectOfType<LoadSystem>();
        if (loadSystem != null)
        {
            Destroy(loadSystem.gameObject);
        }

        // Load the first scene of your game
        SceneManager.LoadScene(0);
    }

}
