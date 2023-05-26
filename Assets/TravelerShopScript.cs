using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TravelerShopScript : MonoBehaviour
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
            case "Coin":
                coin = 0;
                stoneheart = 0;
                break;
            case "Fruit":
                coin = 0;
                stoneheart = 0;
                break;
            case "Wood":
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


        bool added = inventoryManager.AddItem(itemToBuy, itemToBuy.maxDurability,1);
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
