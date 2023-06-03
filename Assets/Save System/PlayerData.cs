using UnityEngine;
using System.Collections.Generic;
using System.IO;

[System.Serializable]
public class PlayerData
{
    // PLAYER DATA
    public bool isNight;
    public float timeOfDay;
    public float maxHealth;
   public float _health;
   public bool isDashUnlocked;
  
   public Vector2 playerPos;

   public List<InventoryItemData> inventoryItems;
   public int mapSeed;
    public PlayerData()
    {
        inventoryItems = new List<InventoryItemData>();
    }

    public bool isAcquiredWisp;
    //BOSS-DATA

    public Vector2 golemPos;
    public Vector2 slimePos;
    public Vector2 bomberPos;
    public Vector2 dogoPos;

    public bool isDeadGolem;
    public bool isDeadSlime;
    public bool isDeadBomber;
    public bool isDeadDogo;

    
}
[System.Serializable]
public class InventoryItemData
{
    public string itemName;
    public int durability;
    public int count;
}