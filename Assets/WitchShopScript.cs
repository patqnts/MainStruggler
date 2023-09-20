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
    public GameObject strugglerBottle;
    public Animator animator;

    public AudioSource[] witchSounds;
    private void Start()
    {
        inventoryManager = FindObjectOfType<InventoryManager>();
        animator = GetComponentInParent<Animator>();
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
        int darkOrbisCost = 0;
        int lightOrbisCost = 0;
        int fireOrbisCost = 0;
        int windOrbisCost = 0;
        int swordCost = 0;

        int wispCost = 0;
        if (itemToBuy.name == "Struggler Bottle" && InventoryManager.instance.GetInventoryItem("Struggler Bottle") != null)
        {
            witchSounds[1].Play();
            Debug.Log("Item already exists in inventory. Cannot buy another one.");
            return; // Exit the method if the item already exists in the inventory
        }

        switch (itemToBuy.name)
        {
            case "Struggler Bottle":
                coinCost = 350;
                stoneheartCost = 25;
               
                break;
            case "Light Longsword":
                coinCost = 2500;
                lightOrbisCost = 10;
                swordCost = 1;
                
                break;
            case "Dark Longsword":
                coinCost = 2500;
                darkOrbisCost = 10;
                swordCost = 1;

                break;
            case "Flame Sword":
                coinCost = 2500;
                fireOrbisCost = 10;
                swordCost = 1;

                break;
            case "Wind Sword":
                coinCost = 2500;
                windOrbisCost = 10;
                swordCost = 1;
                break;

            case "Wind Wisp":
                coinCost = 1000;
                wispCost = 1;
                windOrbisCost = 10;
                
                break;
            case "Light Wisp":
                coinCost = 1000;
                wispCost = 1;
                lightOrbisCost = 10;

                break;

            case "Fire Wisp":
                coinCost = 1000;
                wispCost = 1;
                fireOrbisCost = 10;

                break;
            case "Dark Wisp":
                coinCost = 1000;
                wispCost = 1;
                darkOrbisCost = 10;

                break;

            case "Heart Container":
                coinCost = 1;
                darkOrbisCost = 1;
                lightOrbisCost = 1;
                fireOrbisCost = 1;
                windOrbisCost = 1;

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
           inventoryManager.GetItemCount("Steel") < steelIngotCost ||

           inventoryManager.GetItemCount("Dark Orbis") < darkOrbisCost ||
           inventoryManager.GetItemCount("Light Orbis") < lightOrbisCost ||
           inventoryManager.GetItemCount("Fire Orbis") < fireOrbisCost ||
           inventoryManager.GetItemCount("Wind Orbis") < windOrbisCost||

           inventoryManager.GetItemCount("Wisp") < wispCost ||
            inventoryManager.GetItemCount("Starstone Sword") < swordCost) 
        {
            animator.SetTrigger("NotEnough");
            witchSounds[1].Play();
            Debug.Log("Not enough resources to buy " + itemToBuy.name);
            return;
        }

        
        

        
        bool added = inventoryManager.AddItem(itemToBuy,itemToBuy.maxDurability,1);
        if (added)
        {
            witchSounds[0].Play();
            inventoryManager.RemoveItem("Coin", coinCost);
            inventoryManager.RemoveItem("Wood", woodCost);
            inventoryManager.RemoveItem("Stone", stoneCost);
            inventoryManager.RemoveItem("Stoneheart", stoneheartCost);
            inventoryManager.RemoveItem("Starstone", starstoneCost);
            inventoryManager.RemoveItem("Steel", steelIngotCost);

            inventoryManager.RemoveItem("Dark Orbis", darkOrbisCost);
            inventoryManager.RemoveItem("Light Orbis", lightOrbisCost);
            inventoryManager.RemoveItem("Fire Orbis", fireOrbisCost);
            inventoryManager.RemoveItem("Wind Orbis", windOrbisCost);
            inventoryManager.RemoveItem("Starstone Sword", swordCost);
            inventoryManager.RemoveItem("Wisp", wispCost);
            Debug.Log("Bought " + itemToBuy.name);
        }
        else
        {
            witchSounds[1].Play();
            Debug.Log("Inventory is full");
        }
    }

}
