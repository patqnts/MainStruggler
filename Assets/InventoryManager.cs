using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;
    public int maxStackedItems = 99;
    public InventorySlot[] inventorySlots;
    public GameObject inventoryItemPrefab;
    public Transform handTransform;
    public GameObject weaponHolder;
    

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

        if (selectedItem != null && selectedItem.holdable == true)
        {
            SpriteRenderer weaponSpriteRenderer = weaponHolder.GetComponent<SpriteRenderer>();
            Animator weaponAnimator = weaponHolder.GetComponent<Animator>();

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

                if (selectedItem == null || !selectedItem.holdable)
                {
                    InventoryManager.instance.weaponHolder.GetComponent<SpriteRenderer>().sprite = null;
                }
                else
                {
                    Sprite selectedSprite = InventoryManager.instance.GetSelectedItem(false).prefab.GetComponent<SpriteRenderer>().sprite;
                    InventoryManager.instance.weaponHolder.GetComponent<SpriteRenderer>().sprite = selectedSprite;
                }
            }
        }
        else // No item is selected or the selected item is not holdable
        {
            if(selectedItem != null)
            {
                SpriteRenderer weaponSpriteRenderer = weaponHolder.GetComponent<SpriteRenderer>();
                Animator weaponAnimator = weaponHolder.GetComponent<Animator>();
                weaponAnimator.enabled = false; // turn off the animator
                weaponSpriteRenderer.sprite = null; // remove the sprite
            }
            
        }
    }


    public bool AddItem(Item item)
    {
        //same item stacking in inventory
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null &&
                itemInSlot.item == item &&
                itemInSlot.count < item.maxStackCount &&
                itemInSlot.item.stackable == true)
            {
                itemInSlot.count++;
                itemInSlot.RefreshCount();
                return true;
            }
        }

        //finding empty slot
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot == null)
            {
                SpawnNewItem(item, slot);
                return true;
            }
        }
        return false;
    }

   public void SpawnNewItem(Item item, InventorySlot slot)
    {
        GameObject newItemGo = Instantiate(inventoryItemPrefab, slot.transform);
        InventoryItem inventoryItem = newItemGo.GetComponent<InventoryItem>();
        inventoryItem.InitialiseItem(item);
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
        // Keep track of items that have already been dropped
        HashSet<string> itemsDropped = new HashSet<string>();

        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null && !itemsDropped.Contains(itemInSlot.item.name))
            {
                // Instantiate a new instance of the item's prefab at the drop location
                GameObject itemObject = Instantiate(itemInSlot.item.prefab, dropLocation.position, Quaternion.identity);

                // If the item is stackable, remove only one instance of the item from the player's inventory
                if (itemInSlot.item.stackable)
                {
                    RemoveItem(itemInSlot.item.name, 1);
                }
                // If the item is not stackable, remove all instances of the item from the player's inventory
                else
                {
                    RemoveItem(itemInSlot.item.name, itemInSlot.count);
                }

                // Add the item to the set of items that have already been dropped
                itemsDropped.Add(itemInSlot.item.name);
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