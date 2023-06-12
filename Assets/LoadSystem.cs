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
    public GolemScript golem;
    public SlimeQueen slime;
    public BomberScript bomber;
    public DogoTotemScripts dogo;
    public CrowScripts crow;
    public BottleScript bottle;
    public Dropdown selectedSlot;
    public DayNightCycles time;
    private static LoadSystem instance;
    public Movement player;
    public string passedText = "";
    public bool dashPassValue;
    public ImageLoader imageLoader;
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
        if (cellular != null)
        {
            // LoadPlayer(passedText);
        }

       
    }
    public void RogueScript()
    {
        GameObject rogueGameObject = GameObject.Find("Rogue");
        if (rogueGameObject != null)
        {
            ImageLoader imageLoader = rogueGameObject.GetComponent<ImageLoader>();
            if (imageLoader != null)
            {
                imageLoader.DeleteSaveData("rogue");
            }
        }
    }

  
    public void LoadPlayerWithDefaultSeed(string profileId)
    {
        
        passedText = profileId;
        lastSeedCode = 0; // Set the seed code to 0
        StartCoroutine(LoadRougeAfterDelay());

        // Load the player with the default seed code (0)

    }
    private IEnumerator LoadRougeAfterDelay()
    {
        yield return new WaitForSeconds(3f); // Wait for 3 seconds
        SceneManager.LoadScene("Rogue");

    }
    public void LoadPlayerAndGameScene(string profileId)
    {
        passedText = profileId;
        StartCoroutine(LoadSceneAfterDelay());
    }

    private IEnumerator LoadSceneAfterDelay()
    {
        yield return new WaitForSeconds(3f); // Wait for 3 seconds

        if (seedCode != null)
        {
            Debug.Log("SEED = NOT NULL");
            SceneManager.LoadScene("StrugglerMain");
        }
        else
        {
            Debug.Log("SEED = NULL");
            lastSeedCode = 0;
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

            bottle = FindObjectOfType<BottleScript>();          
            player = FindObjectOfType<Movement>();
            golem = FindObjectOfType<GolemScript>();
            slime = FindObjectOfType<SlimeQueen>();
            bomber = FindObjectOfType<BomberScript>();
            dogo = FindObjectOfType<DogoTotemScripts>();
            crow = FindObjectOfType<CrowScripts>();
            time = FindObjectOfType<DayNightCycles>();

            InventoryManager inventoryManager = FindObjectOfType<InventoryManager>();
            ItemDatabase itemDatabase = FindObjectOfType<ItemDatabase>();
            Cellular cellular = FindObjectOfType<Cellular>();
            

            player.transform.position = data.playerPos;
            if (data != null && cellular != null)// && seedCode != null)
            {
                
                cellular.wispBottleisBroken = data.isAcquiredWisp;
                cellular.seedCode = data.mapSeed;
                lastSeedCode = cellular.seedCode;
                Debug.Log(data.mapSeed + " is existing");
            }
            if (time != null && data != null)
            {
                
                time.isNight = data.isNight;
                time.timeOfDay = data.timeOfDay;
                time.InitializeDayNightCycles(); // Assigning day/night time color
               
            }
            if(crow != null&& data != null)
            {
                crow.dashUnlocked = data.isDashUnlocked;
                
            }
            if (player != null)
            {
                cellular.isDeadGolem = data.isDeadGolem;
                cellular.isDeadSlime = data.isDeadSlime;
                cellular.isDeadBomber = data.isDeadBomber;
                cellular.isDeadDogo = data.isDeadDogo;
                player.maxHealth = data.maxHealth;
                player._health = data._health;  
                player.transform.position = data.playerPos;

                if(data.isDeadGolem = false)
                {
                    golem.transform.position = data.golemPos;
                }

                if (data.isDeadSlime = false)
                {
                    slime.transform.position = data.slimePos;
                }
                if (data.isDeadBomber = false)
                {
                    bomber.transform.position = data.bomberPos;
                }
                if (data.isDeadDogo = false)
                {
                    dogo.transform.position = data.dogoPos;
                }
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
            Debug.Log("dash ability: " + data.isDashUnlocked);
            Debug.Log("Position: " + data.playerPos);
            Debug.Log("Inventory Items loaded: " + data.inventoryItems.Count);
            Debug.Log("is Night: " + data.isNight);
            Debug.Log("Time of day: " + data.timeOfDay);
            player.transform.position = data.playerPos;
            dashPassValue = data.isDashUnlocked;
            
        }
        else
        {
            Cellular cellular = FindObjectOfType<Cellular>();
            Debug.Log(cellular.seedCode + " is null");
           
            
            Debug.Log("No player data found.");
        }
    }





}
