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
    public int Intest;

    private void Start()
    {
        inventoryManager = FindObjectOfType<InventoryManager>();

    }
   

    public void BuyItem()
    {
        
        Item itemToBuy = itemList[Intest];


        switch (itemToBuy.name)
        {
            case "Wooden Sword"://0
                coin = 0;
                stoneheart = 0;
                break;
            case "Stone Sword"://1
                coin = 0;
                stoneheart = 0;
                break;
            case "Steel Sword"://2
                coin = 0;
                stoneheart = 0;
                break;
            case "Starstone Sword"://3
                coin = 0;
                stoneheart = 0;
                break;
            case "Coin"://4
                coin = 0;
                stoneheart = 0;
                break;
            case "Wood Gathering Tool"://5
                coin = 0;
                stoneheart = 0;
                break;
            case "Stone Gathering Tool"://6
                coin = 0;
                stoneheart = 0;
                break;
            case "Steel Gathering Tool"://7
                coin = 0;
                stoneheart = 0;
                break;
            case "Starstone Gathering Tool"://8
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
