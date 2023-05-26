using UnityEngine;
using System.Collections.Generic;
using System.IO;

[System.Serializable]
public class PlayerData
{
   
   public float _health;
   public float moveSpeed;
   public Vector2 playerPos;
   public List<InventoryItemData> inventoryItems;
   public int mapSeed;

    public PlayerData()
    {
        inventoryItems = new List<InventoryItemData>();
    }
}
[System.Serializable]
public class InventoryItemData
{
    public string itemName;
    public int durability;
    public int count;
}