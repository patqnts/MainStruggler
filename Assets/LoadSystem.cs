using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using System;

public class LoadSystem : MonoBehaviour
{
    private const string PlayerDataPath = "/PlayerDatas.json";
    
    public InputField seedCode;
    public int lastSeedCode;


    public Dropdown selectedSlot;
    public string selectedProfileId = "";
    private static LoadSystem instance;
    public Movement player;
    private void Awake()
    {
        // Make sure only one instance of LoadSystem exists
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        seedCode = FindObjectOfType<InputField>();
        // Set the seed code value from the last session
        if (seedCode != null)
        {
            //seedCode.text = lastSeedCode.ToString();
            seedCode.text = "123";
        }
        player = FindObjectOfType<Movement>();
    }
    private void Update()
    {
        seedCode = FindObjectOfType<InputField>();
    }
    public void LoadPlayerAndGameScene()
    {
        // Store the seed code value before loading the next scene
        if (seedCode != null && !string.IsNullOrEmpty(seedCode.text))
        {
            if (int.TryParse(seedCode.text, out lastSeedCode))
            {
                SceneManager.LoadScene("StrugglerMain");
            }
            else
            {
                seedCode.text = "0";
                SceneManager.LoadScene("StrugglerMain");
                Debug.LogError("Invalid seed code input!");
            }
        }
        else
        {

            SceneManager.LoadScene("StrugglerMain");
           
        }
    }

    public void LoadPlayer(string profileId)
    {
        profileId = selectedSlot.options[selectedSlot.value].text;
        selectedProfileId = profileId;
        string directoryPath = Path.Combine(Application.persistentDataPath, profileId);
        string filePath = Path.Combine(directoryPath, Path.GetFileName(PlayerDataPath));
        Debug.Log(filePath);

        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            PlayerData data = JsonUtility.FromJson<PlayerData>(json);

            Cellular cellular = FindObjectOfType<Cellular>();
            player = FindObjectOfType<Movement>();
            InventoryManager inventoryManager = FindObjectOfType<InventoryManager>();
            ItemDatabase itemDatabase = FindObjectOfType<ItemDatabase>();
            player.transform.position = data.playerPos;
            if (cellular != null && seedCode != null)
            {
                cellular.seedCodex = data.mapSeed;
                seedCode.text = data.mapSeed.ToString();
                lastSeedCode = data.mapSeed;
            }
           

            if (player != null)
            {
                
                player._health = data._health;
                player.moveSpeed = data.moveSpeed;
                player.transform.position = data.playerPos;
                Debug.Log(data.playerPos);
                
            }

            if (inventoryManager != null)
            {
                inventoryManager.ClearInventory();

                foreach (var itemData in data.inventoryItems)
                {
                    if (itemDatabase != null)
                    {
                        Item item = itemDatabase.GetItemByName(itemData.itemName);
                        if (item != null)
                        {
                            bool itemAdded = inventoryManager.AddItem(item, itemData.durability, itemData.count);
                            if (!itemAdded)
                            {
                                Debug.LogWarning("No available slots to add the item: " + itemData.itemName);
                            }
                        }
                    }
                }

                inventoryManager.ChangeSelectedSlot(0);
            }

            Debug.Log("Player loaded!");
            Debug.Log("Map seed: " + data.mapSeed);
            Debug.Log("Health: " + data._health);
            Debug.Log("Move Speed: " + data.moveSpeed);
            Debug.Log("Position: " + data.playerPos);
            Debug.Log("Inventory Items loaded: " + data.inventoryItems.Count);
            player.transform.position = data.playerPos;
        }
        else
        {
            Debug.Log("No player data found.");
        }
    }





}
