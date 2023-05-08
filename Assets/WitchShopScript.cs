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
        Item itemToBuy = itemList[Intest];


        switch (itemToBuy.name)
        {
            case "BottleFilled":
                coin = 0;
                stoneheart = 0;
                break;
            case "Light Longsword":
                coin = 0;
                stoneheart = 0;
                break;
            case "Dark Longsword":
                coin = 0;
                stoneheart = 0;
                break;
            case "Flame Sword":
                coin = 0;
                stoneheart = 0;
                break;
            default:
                Debug.Log("Invalid item name");
                return;
        }

        if (inventoryManager.GetItemCount("Coin") < coin || inventoryManager.GetItemCount("Stoneheart") < stoneheart)
        {
            Debug.Log("Not enough coins/materials to buy " + itemToBuy.name);
            return;
        }


        bool added = inventoryManager.AddItem(itemToBuy);
        if (added)
        {
            if (coin > 0)
            {
                inventoryManager.RemoveItem("Coin", coin);
                inventoryManager.RemoveItem("Stoneheart", stoneheart);
            }
            Debug.Log("Bought " + itemToBuy.name);
        }
        else
        {
            Debug.Log("Inventory is full");
        }
    }

}
