using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    public Dictionary<Item, int> itemDurabilityMap = new Dictionary<Item, int>();
    public Animator animator;
    // Add item with initial durability to the dictionary
    public void AddItemWithDurability(Item item, int durability)
    {
        if (item != null && !itemDurabilityMap.ContainsKey(item))
        {
            itemDurabilityMap[item] = durability;
        }
    }
    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    // Reduce durability of an item
    public void ReduceDurability(InventoryItem inventoryItem, int amount)
    {
        if (inventoryItem != null)
        {
           
            Debug.Log(inventoryItem + " current durability: " + inventoryItem.durability);
            inventoryItem.durability -= amount;
            if (inventoryItem.durability <= 0)
            {
                // Item is broken, handle accordingly
               // inventoryItem.durability = 0;
                HandleItemBreak(inventoryItem.item);
            }
        }
    }

    // Get the remaining durability of an item
    public int GetItemDurability(Item item)
    {
        if (item != null && itemDurabilityMap.ContainsKey(item))
        {
            return itemDurabilityMap[item];
        }
        return 0;
    }

    // Handle item break event
    private void HandleItemBreak(Item item)
    {
        animator.SetTrigger("Destroy");
        InventoryManager.instance.GetSelectedItem(true);
        // Item broke, perform any necessary actions
    }
}
