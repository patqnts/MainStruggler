using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public Image image;
    public Color selectedColor, notSelectedColor;
    public int slot;
    


    private void Awake()
    {
        Deselect();
    }
    public void Select()
    {
        image.color = selectedColor;
        
    }
    public void Deselect()
    {
        image.color = notSelectedColor;
       
    }
    public void OnDrop(PointerEventData eventData)
    {
        InventoryItem droppedItem = eventData.pointerDrag.GetComponent<InventoryItem>();
        if (droppedItem != null)
        {

            

            Transform itemParent = droppedItem.parentAfterDrag;
            Transform dropParent = transform;
            if (itemParent == dropParent)
            {
                return; // No need to swap if dropping into the same slot
            }

            
            
            InventoryItem existingItem = null;
            if (dropParent.childCount > 0)
            {
                existingItem = dropParent.GetChild(0).GetComponent<InventoryItem>();
            }

            if (existingItem != null)
            {
                // Swap items
                existingItem.transform.SetParent(itemParent);
                droppedItem.transform.SetParent(dropParent);
                existingItem.parentAfterDrag = itemParent;
                droppedItem.parentAfterDrag = dropParent;
            }
            else
            {
                // Move item to empty slot
                droppedItem.transform.SetParent(dropParent);
                droppedItem.parentAfterDrag = dropParent;
            }

            // Instantiate prefab if holding a holdable item in selected slot
            if (droppedItem.item != null && droppedItem.item.prefab != null && droppedItem.item.holdable)
            {
                if (InventoryManager.instance.selectedSlot == transform.GetSiblingIndex())
                {
                    // Instantiate the prefab and set its parent to the weapon holder
                    GameObject newObject = Instantiate(droppedItem.item.prefab, InventoryManager.instance.weaponHolder.transform);
                    newObject.transform.localPosition = Vector3.zero;
                    newObject.transform.localRotation = Quaternion.identity;
                    newObject.transform.localScale = Vector3.one;

                    // Destroy the spawned item that was previously in the hand
                    if (InventoryManager.instance.spawnedItem != null)
                    {
                        Destroy(InventoryManager.instance.spawnedItem);
                    }

                    // Set the new spawned item as the currently selected item
                    InventoryManager.instance.spawnedItem = newObject;
                }
            }
            else if (!droppedItem.item.holdable && InventoryManager.instance.selectedSlot == transform.GetSiblingIndex())
            {
                Destroy(InventoryManager.instance.spawnedItem);
            }
        }
    }
    public void RemoveItem()
    {
        if (transform.childCount > 0)
        {
            InventoryItem item = transform.GetChild(0).GetComponent<InventoryItem>();

            // Remove the item from the weapon holder
            if (item.inventoryManager.inventoryItemPrefab != null && item.inventoryManager.inventoryItemPrefab.transform.parent == item.inventoryManager.weaponHolder.transform)
            {
                Destroy(item.inventoryManager.inventoryItemPrefab);

            }

            // Destroy the item object
            Destroy(item.gameObject);
            Destroy(InventoryManager.instance.spawnedItem);

            // Reset the slot
            item.transform.SetParent(null);
            item.parentAfterDrag = null;
        }
    }



    public ConfirmationDialog confirmationDialog;
  
    private int clickCount = 0;
    private float lastClickTime = 0f;

    public void SelectedItem()
    {
        if (transform.childCount == 0) return;
        InventoryManager.instance.ChangeSelectedSlot(slot);

        float currentTime = Time.time;
        if (currentTime - lastClickTime < 0.25f && clickCount >= 2)
        {
            // Show the confirmation box
            confirmationDialog.ShowConfirmationBox();
            confirmationDialog.itemToRemove = this;

            // Reset click count and time
            clickCount = 0;
            lastClickTime = 0f;
        }
        else
        {
            // Increment click count and update last click time
            clickCount++;
            lastClickTime = currentTime;
        }
    }





}
