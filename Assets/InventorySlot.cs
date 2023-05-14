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
        // Get the InventoryItem component from the dragged object
        InventoryItem droppedItem = eventData.pointerDrag.GetComponent<InventoryItem>();
       
        if (droppedItem != null)
        {
            // Get the transform of the item's original parent
            Transform itemParent = droppedItem.parentAfterDrag;
            // Get the transform of the drop target
            Transform dropParent = transform;


            // If the item is being dropped into the same slot, do nothing
            if (itemParent == dropParent)
            {
                return; // No need to swap if dropping into the same slot
            }

            // Check if there is already an item in the drop target
            InventoryItem existingItem = null;

            if (dropParent.childCount > 0)
            {
                existingItem = dropParent.GetChild(0).GetComponent<InventoryItem>();
            }

            // Check if dropped item is not a weapon or fairy
            if (dropParent.parent == InventoryManager.instance.equipmentSlots[7].transform)
            {
                
                if (droppedItem.item.type != ItemType.Fairy && existingItem == null)
                {
                    Debug.Log("Item is not fairy : slot slot");
                    
                }
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

            //FAIRY DROPPING SLOT
            if (droppedItem.item != null && droppedItem.item.prefab != null && droppedItem.item.type == ItemType.Fairy)
            {
                Debug.Log("sibling index: "+ transform.GetSiblingIndex());

                if (InventoryManager.instance.fairySlot == transform.GetSiblingIndex())
                {
                    Debug.Log("Item is fairy slot: " + InventoryManager.instance.fairySlot);


                    Item fairySlot = InventoryManager.instance.GetFairySlot(false);
                    


                    if (fairySlot != null && fairySlot.type == ItemType.Fairy)
                    {
                        //FAIRY
                       
                        Sprite itemSprite = fairySlot.prefab.GetComponent<SpriteRenderer>().sprite;
                        InventoryManager.instance.fairyHolder.GetComponent<SpriteRenderer>().sprite = itemSprite;
                        // CURRENT SELECTED SLOT
                       
                        Animator fairyAnimator = fairySlot.prefab.GetComponent<Animator>();
                        if (fairyAnimator != null )
                        {
                            Debug.Log("Fairy and Weapon Animation Good");
                            InventoryManager.instance.fairyHolder.GetComponent<Animator>().runtimeAnimatorController = fairyAnimator.runtimeAnimatorController;
                        }
                        else
                        {
                            InventoryManager.instance.fairyHolder.GetComponent<Animator>().runtimeAnimatorController = null;
                            Debug.Log("Fairy or Wweapon item has no animator");
                        }
                    }
                    else
                    {
                        InventoryManager.instance.fairyHolder.GetComponent<Animator>().runtimeAnimatorController = null;
                        InventoryManager.instance.fairyHolder.GetComponent<SpriteRenderer>().sprite = null;
                        Debug.Log("ALL NULL");
                       
                    }
                
            }
                else if (InventoryManager.instance.fairySlot != transform.GetSiblingIndex())
                {
                    Debug.Log("Item  fairy is REMOVED from slot or SWAPPED");
                    Item fairySlot = InventoryManager.instance.GetFairySlot(false);
                    


                    if (fairySlot != null && fairySlot.type == ItemType.Fairy)
                    {
                        //FAIRY
                        Sprite itemSprite = fairySlot.prefab.GetComponent<SpriteRenderer>().sprite;
                        InventoryManager.instance.fairyHolder.GetComponent<SpriteRenderer>().sprite = itemSprite;
                        // CURRENT SELECTED SLOT
                        
                        

                        Animator fairyAnimator = fairySlot.prefab.GetComponent<Animator>();
                        
                        if (fairyAnimator != null)
                        {
                             InventoryManager.instance.fairyHolder.GetComponent<Animator>().runtimeAnimatorController = fairyAnimator.runtimeAnimatorController;
                        }
                        else
                        {
                            InventoryManager.instance.fairyHolder.GetComponent<Animator>().runtimeAnimatorController = null;
                            Debug.Log("Holdable item has no animator");
                        }
                    }
                    else
                    {
                        InventoryManager.instance.fairyHolder.GetComponent<Animator>().runtimeAnimatorController = null;
                        InventoryManager.instance.fairyHolder.GetComponent<SpriteRenderer>().sprite = null;

                       
                    }
                }
            }
            else
            {
                Debug.Log("item is not fairy");
                Item fairySlot = InventoryManager.instance.GetFairySlot(false); //SHOW CURRENT FAIRY
                if (fairySlot != null && fairySlot.type == ItemType.Fairy)
                {
                    Sprite itemSprite = fairySlot.prefab.GetComponent<SpriteRenderer>().sprite;
                    InventoryManager.instance.fairyHolder.GetComponent<SpriteRenderer>().sprite = itemSprite;
                    Animator itemAnimator = fairySlot.prefab.GetComponent<Animator>();
                    if (itemAnimator != null)
                    {
                        InventoryManager.instance.fairyHolder.GetComponent<Animator>().runtimeAnimatorController = itemAnimator.runtimeAnimatorController;
                    }
                    else
                    {
                        InventoryManager.instance.fairyHolder.GetComponent<Animator>().runtimeAnimatorController = null;
                        Debug.Log("Holdable item has no animator");
                    }
                }
                else
                {
                    InventoryManager.instance.fairyHolder.GetComponent<Animator>().runtimeAnimatorController = null;
                    InventoryManager.instance.fairyHolder.GetComponent<SpriteRenderer>().sprite = null;
                }
            }



                //IF  ITEM IS NOT HOLDABLE AND SWAP TO WEAPON
           if (droppedItem.item != null && droppedItem.item.prefab != null && !droppedItem.item.holdable)
           {
               
                Debug.Log(transform.GetSiblingIndex());
                if (InventoryManager.instance.selectedSlot != transform.GetSiblingIndex() &&
                    InventoryManager.instance.fairySlot != transform.GetSiblingIndex())
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
                        Debug.Log("Holdable item has no animator 2");
                    
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
                            Debug.Log("Item is not holdable");
                           
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

                            InventoryManager.instance.weaponHolder.GetComponent<SpriteRenderer>().sprite = null;
                            InventoryManager.instance.weaponHolder.GetComponent<Animator>().runtimeAnimatorController = null;
                            Debug.Log("Holdable item has no animator");
                        }
                    }
                    
                    else
                    {
                        Debug.Log("Item is not holdable24");
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
               // && InventoryManager.instance.GetFairySlot(false) == null) /////PLACEHOLDER
            {
                Debug.Log("Item is not holdable87: Sibling index : " + transform.GetSiblingIndex());
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
        Debug.Log(slot);
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
