using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackShopScript : MonoBehaviour
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
            case "Coin":
                coinCost = 0;
                break;
            case "Wooden Sword":
                coinCost = 5;
                woodCost = 10;
                stoneCost = 5;
                break;
            case "Stone Sword":
                coinCost = 400;
                stoneCost = 25;
                woodCost = 10;
                break;
            case "Steel Sword":
                coinCost = 800;
                stoneCost = 50;
                woodCost = 40;
                stoneheartCost = 20;
                break;
            case "Starstone Sword":
                coinCost = 1600;
                starstoneCost = 3;
                stoneCost = 150;
                woodCost = 200;
                stoneheartCost = 50;
                break;
            case "Wood Gathering Tool":
                coinCost = 5;
                woodCost = 5;
                break;
            case "Stone Gathering Tool":
                coinCost = 400;
                stoneCost = 25;
                woodCost = 15;
                break;
            case "Steel Gathering Tool":
                coinCost = 800;
                stoneCost = 50;
                woodCost = 100;
                stoneheartCost = 10;
                break;
            case "Starstone Gathering Tool":
                coinCost = 1600;
                starstoneCost = 3;
                stoneCost = 150;
                woodCost = 150;
                steelIngotCost = 50;
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

        bool added = inventoryManager.AddItem(itemToBuy, itemToBuy.maxDurability,1);
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
