using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WitchShopScript : MonoBehaviour
{
    // Start is called before the first frame update


    public InventoryManager inventoryManager;
    public Item[] itemList;
    int coin;
    int stoneheart;
    public int Intest = -1;

    private void Start()
    {
        inventoryManager = FindObjectOfType<InventoryManager>();
    }
    public void BuyItem()
    {
        if (Intest < 0 || Intest >= itemList.Length)
        {
            Debug.Log("Invalid item index");
            return;
        }

        Item itemToBuy = itemList[Intest];
        int coinCost = 0;
        int woodCost = 0;
        int stoneCost = 0;
        int stoneheartCost = 0;
        int starstoneCost = 0;
        int steelIngotCost = 0;


        switch (itemToBuy.name)
        {
            case "Struggler Bottle":
                coinCost = 0;
               
                break;
            case "Light Longsword":
                coinCost = 0;
                
                break;
            case "Dark Longsword":
                coinCost = 0;
               
                break;
            case "Flame Sword":
                coinCost = 0;

                break;
            case "Wind Sword":
                coinCost = 0;
                break;

            case "Wind Wisp":
                coinCost = 0;
                
                break;
            case "Light Wisp":
                coinCost = 0;
                
                break;

            case "Fire Wisp":
                coinCost = 0;
              
                break;
            case "Dark Wisp":
                coinCost = 0;
                
                break;
            default:
                Debug.Log("Invalid item name");
                return;
        }

        if (inventoryManager.GetItemCount("Coin") < coinCost ||
           inventoryManager.GetItemCount("Wood") < woodCost ||
           inventoryManager.GetItemCount("Stone") < stoneCost ||
           inventoryManager.GetItemCount("Stoneheart") < stoneheartCost ||
           inventoryManager.GetItemCount("Starstone") < starstoneCost ||
           inventoryManager.GetItemCount("Steel") < steelIngotCost)
        {
            Debug.Log("Not enough resources to buy " + itemToBuy.name);
            return;
        }

        bool added = inventoryManager.AddItem(itemToBuy,itemToBuy.maxDurability,1);
        if (added)
        {
            inventoryManager.RemoveItem("Coin", coinCost);
            inventoryManager.RemoveItem("Wood", woodCost);
            inventoryManager.RemoveItem("Stone", stoneCost);
            inventoryManager.RemoveItem("Stoneheart", stoneheartCost);
            inventoryManager.RemoveItem("Starstone", starstoneCost);
            inventoryManager.RemoveItem("Steel", steelIngotCost);
            Debug.Log("Bought " + itemToBuy.name);
        }
        else
        {
            Debug.Log("Inventory is full");
        }
    }

}
