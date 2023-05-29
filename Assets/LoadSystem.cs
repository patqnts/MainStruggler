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
    
    private static LoadSystem instance;
    public Movement player;
    public string passedText = "";
    
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
        Cellular cellular = FindObjectOfType<Cellular>();
        if(cellular != null)
        {
           // LoadPlayer(passedText);
        }
        
    }

    public void LoadPlayerAndGameScene(string profileId)
    {
        passedText = profileId;
        
        // Store the seed code value before loading the next scene
        if (seedCode != null)// && !string.IsNullOrEmpty(seedCode.text))
        {
            Debug.Log("SEED = NOT NULL");
            SceneManager.LoadScene("StrugglerMain");

        }       
        else
        {
            Debug.Log("SEED =  NULL");
            lastSeedCode = 0 ;
            SceneManager.LoadScene("StrugglerMain");
           
        }
    }

    public void LoadPlayer(string profileId)
    {
        
        profileId = passedText;
      
        //profileId = selectedSlot.options[selectedSlot.value].text;

        string directoryPath = Path.Combine(Application.persistentDataPath, profileId);
        string filePath = Path.Combine(directoryPath, Path.GetFileName(PlayerDataPath));
        Debug.Log(filePath);

       // string screenshotPath = Path.Combine(directoryPath, "/screenshot.png");
        //ScreenCapture.CaptureScreenshot(screenshotPath);
      //  Debug.Log("Screenshot saved: " + screenshotPath);
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            PlayerData data = JsonUtility.FromJson<PlayerData>(json);

            
            player = FindObjectOfType<Movement>();

            InventoryManager inventoryManager = FindObjectOfType<InventoryManager>();
            ItemDatabase itemDatabase = FindObjectOfType<ItemDatabase>();
            Cellular cellular = FindObjectOfType<Cellular>();
            player.transform.position = data.playerPos;
            if (data != null && cellular != null)// && seedCode != null)
            {
              
                cellular.seedCode = data.mapSeed;
                lastSeedCode = cellular.seedCode;
                Debug.Log(data.mapSeed + " is existing");
                
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
            Cellular cellular = FindObjectOfType<Cellular>();
            Debug.Log(cellular.seedCode + " is null");
           
            
            Debug.Log("No player data found.");
        }
    }





}
