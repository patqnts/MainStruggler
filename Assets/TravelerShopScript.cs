using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TravelerShopScript : MonoBehaviour
{
    // Start is called before the first frame update
    public InventoryManager inventoryManager;
    public Item[] itemList;
    int coin;
    public Animator animator;
    public int Intest = -1;
    public AudioSource[] uiSounds;

    private void Start()
    {
        inventoryManager = FindObjectOfType<InventoryManager>();
        animator = GetComponentInParent<Animator>();
    }
    private void OnEnable()
    {
        Intest = 5;
    }
    public void BuyItem()
    {
        Item itemToBuy = itemList[Intest];


        switch (itemToBuy.name)
        {
            case "Coin":
                coin = 0;
               
                break;
            case "Dreamberry":
                coin = 5;
              
                break;
            case "Wood":
                coin = 5;
               
                break;
            case "Blackwood":
                coin = 50;

                break;
            case "Lumos Wood":
                coin = 200;

                break;
            case "Dark Orbis":
                coin = 100;

                break;
            case "Light Orbis":
                coin = 100;

                break;
            case "Fire Orbis":
                coin = 100;

                break;
            case "Wind Orbis":
                coin = 100;

                break;
            case "Stone":
                coin = 5;

                break;
            case "Stoneheart":
                coin = 30;

                break;
            case "Starstone":
                coin = 150;

                break;
            case "Wisp":
                coin = 2000;

                break;

            case "Island Map":
                coin = 350;
             
                break;
            case "Heart Container2":
                coin = 450;

                break;
            default:
                Debug.Log("Invalid item name");
                return;
        }

        if (inventoryManager.GetItemCount("Coin") < coin)
        {
            Debug.Log("Not enough coins/materials to buy " + itemToBuy.name);
            animator.SetTrigger("Not");
            uiSounds[1].Play();
            return;
        }


        bool added = inventoryManager.AddItem(itemToBuy, itemToBuy.maxDurability,1);
        if (added)
        {
            uiSounds[0].Play();
            if (coin > 0)
            {
                inventoryManager.RemoveItem("Coin", coin);
               
            }
            Debug.Log("Bought " + itemToBuy.name);
        }
        else
        {
            uiSounds[1].Play();
            Debug.Log("Inventory is full");
        }
    }

}
