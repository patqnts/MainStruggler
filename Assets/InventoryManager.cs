using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;
    public InventorySlot[] inventorySlots;
    public InventorySlot[] equipmentSlots;
    public GameObject inventoryItemPrefab;
    public GameObject weaponHolder;
   
    public GameObject fairyHolder;
    public int fairySlot = 7;
    public int selectedSlot = -1;
    public GameObject spawnedItem;
    public Text selected;
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        ChangeSelectedSlot(0);
        SetFairySlot(7);
    }
    private void Update()
    {
        if (Input.inputString != null)
        {
            bool isNumber = int.TryParse(Input.inputString, out int number);
            if (isNumber && number > 0 && number < 8)
            {
                ChangeSelectedSlot(number - 1);
            }
            

        }
        Item selectedItem = GetSelectedItem(false);
        if (selectedItem != null)
        {
            selected.text = selectedItem.name;
        }
        else
        {
            selected.text = "";
        }

       
    }
    public void ReduceDurability()
    {
        
            Item selectedItem = GetSelectedItem(false);
            if (selectedItem != null && selectedItem.hasDurability)
            {
                CombatManager combatManager = FindObjectOfType<CombatManager>();
                InventoryItem selectedInventoryItem = inventorySlots[selectedSlot].GetComponentInChildren<InventoryItem>();
                combatManager.ReduceDurability(selectedInventoryItem, 1);
           
            }
        

    }
    public void SetFairySlot(int newValue)
    {
        if (spawnedItem != null)
        {
            Destroy(spawnedItem);
            spawnedItem = null;
        }
        if (fairySlot != null)
        {
            equipmentSlots[fairySlot].Deselect();
        }
        equipmentSlots[newValue].Select();
        fairySlot = newValue;
    }

    public void ChangeSelectedSlot(int newValue)
    {
        if (spawnedItem != null)
        {
            Destroy(spawnedItem);
            spawnedItem = null;
        }
        if (selectedSlot >= 0)
        {
            inventorySlots[selectedSlot].Deselect();
        }
        inventorySlots[newValue].Select();
        selectedSlot = newValue;

        Item selectedItem = GetSelectedItem(false);

        if (selectedItem != null && selectedItem.holdable)
        {
            SpriteRenderer weaponSpriteRenderer = weaponHolder.GetComponent<SpriteRenderer>();
            Animator weaponAnimator = weaponHolder.GetComponent<Animator>();

            Sprite selectedSprite = InventoryManager.instance.GetSelectedItem(false).prefab.GetComponent<SpriteRenderer>().sprite;
            InventoryManager.instance.weaponHolder.GetComponent<SpriteRenderer>().sprite = selectedSprite;
            if (selectedItem.prefab.GetComponent<Animator>() != null)
            {
                weaponAnimator.runtimeAnimatorController = selectedItem.prefab.GetComponent<Animator>().runtimeAnimatorController;
                weaponSpriteRenderer.sprite = null;
                weaponAnimator.enabled = true; // turn on the animator
            }
            else
            {
                weaponAnimator.enabled = false; // turn off the animator
                weaponSpriteRenderer.sprite = null;        
                
            }
        }
        else // No item is selected or the selected item is not holdable
        {
            
                weaponHolder.GetComponent<SpriteRenderer>().sprite = null;
                weaponHolder.GetComponent<Animator>().enabled = false; ;
           
               
            
            
        }
    }

    public void SpawnNewItem(Item item, InventorySlot slot, int durability)
    {
        GameObject newItemGo = Instantiate(inventoryItemPrefab, slot.transform);
        InventoryItem inventoryItem = newItemGo.GetComponent<InventoryItem>();
        inventoryItem.InitialiseItem(item); // Call the method with the item parameter
        inventoryItem.durability = durability; // Set the durability value separately
    }




    public bool AddItem(Item item, int durability)
    {
        CombatManager combatManager = FindObjectOfType<CombatManager>();

        bool addedToInventory = false;

        // Same item stacking in inventory
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null &&
                itemInSlot.item == item &&
                itemInSlot.count < item.maxStackCount &&
                itemInSlot.item.stackable && itemInSlot.item.type != ItemType.Weapon)
            {
                itemInSlot.count++;
                itemInSlot.RefreshCount();
                addedToInventory = true;
                break;
            }
        }

        // Finding empty slot
        if (!addedToInventory)
        {
            for (int i = 0; i < inventorySlots.Length; i++)
            {
                InventorySlot slot = inventorySlots[i];
                InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
                if (itemInSlot == null)
                {
                    SpawnNewItem(item, slot, durability); // Pass the durability value
                    addedToInventory = true;
                    break;
                }
            }
        }

        // If the item was successfully added to the inventory, add it to the CombatManager with its durability
        if (addedToInventory && item.hasDurability)
        {
            combatManager.AddItemWithDurability(item, durability);
            Debug.Log(item + " Durability: " + durability);
        }

        return addedToInventory;
    }







    public Item GetFairySlot(bool use)
    {
        InventorySlot slot = equipmentSlots[fairySlot];
        InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();

        if (itemInSlot != null)
        {
            Item item = itemInSlot.item;
            if (use == true)
            {
                itemInSlot.count--;
                if (itemInSlot.count <= 0)
                {
                    Destroy(itemInSlot.gameObject);
                }
                else
                {
                    itemInSlot.RefreshCount();
                }
            }
            return item;
        }
        return null;
    }

    public Item GetSelectedItem(bool use)
    {
        InventorySlot slot = inventorySlots[selectedSlot];
        InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
        if (itemInSlot != null)
        {
            Item item = itemInSlot.item;
            if(use == true)
            {
                itemInSlot.count--;
                if(itemInSlot.count <= 0)
                {
                    Destroy(itemInSlot.gameObject);
                }
                else
                {
                    itemInSlot.RefreshCount();
                }
                weaponHolder.GetComponent<SpriteRenderer>().sprite = null;
            }
            return item;
        }
        return null;
    }


    public InventoryItem GetInventoryItem(string itemName)
{
    for (int i = 0; i < inventorySlots.Length; i++)
    {
        InventorySlot slot = inventorySlots[i];
        InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
        if (itemInSlot != null && itemInSlot.item.name == itemName)
        {
            return itemInSlot;
        }
    }
    return null;
    }

    public void RemoveItem(string itemName, int count)
    {
        InventoryItem itemInInventory = GetInventoryItem(itemName);
        if (itemInInventory != null)
        {
            itemInInventory.count -= count;
            if (itemInInventory.count <= 0)
            {
                Destroy(itemInInventory.gameObject);
            }
            else
            {
                itemInInventory.RefreshCount();
            }
        }

    }

   

    public int GetItemCount(string itemName)
    {
        int count = 0;
        foreach (InventorySlot slot in inventorySlots)
        {
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null && itemInSlot.item.name == itemName)
            {
                count += itemInSlot.count;
            }
        }
        return count;
    }

    public void DropAllItems(Transform dropLocation)
    {
        HashSet<string> itemsDropped = new HashSet<string>();

        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem[] itemsInSlot = slot.GetComponentsInChildren<InventoryItem>();

            foreach (InventoryItem itemInSlot in itemsInSlot)
            {
                if (itemInSlot != null && itemInSlot.item.name != "Coin" && !itemsDropped.Contains(itemInSlot.item.name))
                {
                    if (!itemInSlot.item.stackable)
                    {
                        // Drop non-stackable items individually
                        GameObject itemObject = Instantiate(itemInSlot.item.prefab, dropLocation.position, Quaternion.identity);
                        Loot loot = itemObject.GetComponent<Loot>();
                        loot.Initialize(itemInSlot.item, 1, itemInSlot.durability); // Drop a single item with its durability
                        RemoveItem(itemInSlot.item.name, 1); // Remove a single item from the inventory
                        itemsDropped.Add(itemInSlot.item.name); // Add the item to the dropped items set
                    }
                    else
                    {
                        // Drop stackable items entirely
                        int itemCount = GetItemCount(itemInSlot.item.name);
                        GameObject itemObject = Instantiate(itemInSlot.item.prefab, dropLocation.position, Quaternion.identity);
                        Loot loot = itemObject.GetComponent<Loot>();
                        loot.Initialize(itemInSlot.item, itemCount, itemInSlot.durability); // Drop the entire item stack with its durability
                        RemoveItem(itemInSlot.item.name, itemCount); // Remove the entire item stack from the inventory
                        itemsDropped.Add(itemInSlot.item.name); // Add the item to the dropped items set
                    }
                }
            }
        }
    }







    public bool isInventoryEmpty()
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null)
            {
                return false;
            }
        }
        return true;
    }

}