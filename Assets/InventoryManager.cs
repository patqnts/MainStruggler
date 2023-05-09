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
            spawnedItem = Instantiate(selectedItem.prefab, handTransform);
            spawnedItem.transform.localPosition = Vector3.zero;
            spawnedItem.transform.localRotation = Quaternion.identity;
            spawnedItem.transform.localScale = Vector3.one;
            spawnedItem.transform.parent = weaponHolder.transform;
            //Destroy(selectedItem);
            CircleCollider2D itemCollider = spawnedItem.GetComponent<CircleCollider2D>();

            // check if the component exists before destroying it
            
                Destroy(itemCollider);
            
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
                itemInSlot.count < maxStackedItems &&
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
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null)
            {
                // Instantiate the item's prefab at the player's position
                GameObject itemObject = Instantiate(itemInSlot.item.prefab, dropLocation.position, Quaternion.identity);

              
                // Remove the item from the player's inventory
                RemoveItem(itemInSlot.item.name, itemInSlot.count);
            }
        }
    }








}
