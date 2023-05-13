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
            //IF  ITEM IS NOT HOLDABLE AND SWAP TO WEAPON
            if (droppedItem.item != null && droppedItem.item.prefab != null && !droppedItem.item.holdable)
            {
                
                if (InventoryManager.instance.selectedSlot != transform.GetSiblingIndex())
                {
                    Item selectedItem = InventoryManager.instance.GetSelectedItem(false);
                    if (selectedItem != null && selectedItem.holdable)
                    {
                        Sprite itemSprite = selectedItem.prefab.GetComponent<SpriteRenderer>().sprite;
                        InventoryManager.instance.weaponHolder.GetComponent<SpriteRenderer>().sprite = itemSprite;
                        Animator itemAnimator = selectedItem.prefab.GetComponent<Animator>();
                        if (itemAnimator != null)
                        {
                            InventoryManager.instance.weaponHolder.GetComponent<Animator>().runtimeAnimatorController = itemAnimator.runtimeAnimatorController;
                        }
                        else
                        {
                            InventoryManager.instance.weaponHolder.GetComponent<Animator>().runtimeAnimatorController = null;
                            Debug.Log("Holdable item has no animator");
                        }
                    }
                    else
                    {
                        InventoryManager.instance.weaponHolder.GetComponent<Animator>().runtimeAnimatorController = null;
                        InventoryManager.instance.weaponHolder.GetComponent<SpriteRenderer>().sprite = null;
                    }
                }

            }

                // Instantiate prefab if holding a holdable item in selected slot
                if (droppedItem.item != null && droppedItem.item.prefab != null && droppedItem.item.holdable) 
            {
                // IF HOLDABLE ITEM IS DROPPED TO SELECTED SLOT
                if (InventoryManager.instance.selectedSlot == transform.GetSiblingIndex())   //holdable item- to selected slot
                {
                    Item selectedItem = InventoryManager.instance.GetSelectedItem(false);
                    Animator weaponAnimator = InventoryManager.instance.weaponHolder.GetComponent<Animator>();
                    if (selectedItem != null && selectedItem.holdable)  //IF WEAPON DROP TO SELECTED WEAPON
                    {
                        weaponAnimator.enabled = true;
                        Sprite itemSprite = droppedItem.item.prefab.GetComponent<SpriteRenderer>().sprite;
                        InventoryManager.instance.weaponHolder.GetComponent<SpriteRenderer>().sprite = itemSprite;

                        // Destroy the spawned item that was previously in the hand
                        if (InventoryManager.instance.spawnedItem != null)
                        {
                            Destroy(InventoryManager.instance.spawnedItem);
                        }

                        // Set the animator of the spawned item if it has one
                        Animator itemAnimator = droppedItem.item.prefab.GetComponent<Animator>();
                        if (itemAnimator != null)
                        {
                            InventoryManager.instance.weaponHolder.GetComponent<Animator>().runtimeAnimatorController = itemAnimator.runtimeAnimatorController;
                        }
                        else
                        {
                            InventoryManager.instance.weaponHolder.GetComponent<Animator>().runtimeAnimatorController = null;
                            Debug.Log("Holdable item has no animator");
                        }
                    }
                    else
                    {
                        if(selectedItem != null && !selectedItem.holdable)
                        {

                            InventoryManager.instance.weaponHolder.GetComponent<Animator>().runtimeAnimatorController = null;

                            InventoryManager.instance.weaponHolder.GetComponent<SpriteRenderer>().sprite = null;

                           
                        }
                    }
                    

                    
                }
                // IF HOLDABLE ITEM IS REMOVED FROM THE SELECTED SLOT TO "NOT SELECTED SLOT"
                else if (InventoryManager.instance.selectedSlot != transform.GetSiblingIndex()) 
                {

                    Item selectedItem = InventoryManager.instance.GetSelectedItem(false);

                    if (selectedItem != null && selectedItem.holdable) //switch weapon
                    {
                        Sprite itemSprite = selectedItem.prefab.GetComponent<SpriteRenderer>().sprite;
                        InventoryManager.instance.weaponHolder.GetComponent<SpriteRenderer>().sprite = itemSprite;
                        Animator itemAnimator = selectedItem.prefab.GetComponent<Animator>();
                        if (itemAnimator != null)
                        {
                            InventoryManager.instance.weaponHolder.GetComponent<Animator>().runtimeAnimatorController = itemAnimator.runtimeAnimatorController;
                        }
                        else
                        {
                            InventoryManager.instance.weaponHolder.GetComponent<Animator>().runtimeAnimatorController = null;
                            Debug.Log("Holdable item has no animator");
                        }
                    }
                    
                    else
                    {
                        InventoryManager.instance.weaponHolder.GetComponent<Animator>().runtimeAnimatorController = null;
                        InventoryManager.instance.weaponHolder.GetComponent<SpriteRenderer>().sprite = null;
                    }
                    

                    // Destroy the spawned item that was previously in the hand
                    if (InventoryManager.instance.spawnedItem != null)
                    {
                        Destroy(InventoryManager.instance.spawnedItem);
                    }
                }
            }
            else if (!droppedItem.item.holdable && InventoryManager.instance.selectedSlot == transform.GetSiblingIndex())
            {
                InventoryManager.instance.weaponHolder.GetComponent<Animator>().runtimeAnimatorController = null;
                InventoryManager.instance.weaponHolder.GetComponent<SpriteRenderer>().sprite = null;

                // Destroy the spawned item that was previously in the hand
                if (InventoryManager.instance.spawnedItem != null)
                {
                    Destroy(InventoryManager.instance.spawnedItem);
                }              
            }
            else
            {
                // Disable animator if item is not holdable and has no animator
                InventoryManager.instance.weaponHolder.GetComponent<Animator>().runtimeAnimatorController = null;
            }
        }
    }




    public void RemoveItem()
{
    if (transform.childCount > 0)
    {
        InventoryItem item = transform.GetChild(0).GetComponent<InventoryItem>();

        // Remove the item from the weapon holder
        if (item.inventoryManager.inventoryItemPrefab != null)
        {
                InventoryManager.instance.weaponHolder.GetComponent<SpriteRenderer>().sprite = null;
                InventoryManager.instance.weaponHolder.GetComponent<Animator>().runtimeAnimatorController = null;
            }

        // Destroy the item object
        Destroy(item.gameObject);
        Destroy(InventoryManager.instance.spawnedItem);

        // Reset the slot
        item.transform.SetParent(null);
        item.parentAfterDrag = null;

        // Remove the sprite and animator
        if (InventoryManager.instance.selectedSlot == slot)
        {
            InventoryManager.instance.weaponHolder.GetComponent<SpriteRenderer>().sprite = null;
            InventoryManager.instance.weaponHolder.GetComponent<Animator>().runtimeAnimatorController = null;
        }
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
