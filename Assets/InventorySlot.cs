using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler, IPointerDownHandler, IPointerUpHandler
{
    public Image image;
    public Color selectedColor, notSelectedColor;
    public int slot;

    public Image itemImage;
    public Text itemNameText;
    public Text itemDescriptionText;
    public Text itemDurabilityText;
    public GameObject itemInfoWindow;
    private InventoryItem currentItem;
    public AudioSource dropSound;
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
        dropSound.Play();
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





            //IF  ITEM IS NOT HOLDABLE and drop to empty
            if (droppedItem.item != null && droppedItem.item.prefab != null && !droppedItem.item.holdable)
            {
                if (InventoryManager.instance.selectedSlot != transform.GetSiblingIndex()) // IF NOT HOLDABLE ITEM is NOT drop to selected
                {
                    Item selectedItem = InventoryManager.instance.GetSelectedItem(false); // GET CURRENT HOLDED SELECTED SLOT
                    Item selectedFairy = InventoryManager.instance.GetFairySlot(false);                                                                      // 
                    if (selectedItem != null && selectedItem.holdable)
                    {
                        Sprite itemSprite = selectedItem.prefab.GetComponent<SpriteRenderer>().sprite;
                        Animator selectedAnimator = selectedItem.prefab.GetComponent<Animator>();
                        InventoryManager.instance.weaponHolder.GetComponent<SpriteRenderer>().sprite = itemSprite;
                        if (selectedAnimator != null)
                        {
                            InventoryManager.instance.weaponHolder.GetComponent<Animator>().runtimeAnimatorController = selectedAnimator.runtimeAnimatorController;
                            
                        }

                    }
                    if (selectedFairy != null && selectedFairy.type == ItemType.Fairy)
                    {
                        Sprite itemSprite = selectedFairy.prefab.GetComponent<SpriteRenderer>().sprite;
                        Animator selectedAnimator = selectedFairy.prefab.GetComponent<Animator>();
                        InventoryManager.instance.fairyHolder.GetComponent<SpriteRenderer>().sprite = itemSprite;
                        if (selectedAnimator != null)
                        {
                            InventoryManager.instance.fairyHolder.GetComponent<Animator>().runtimeAnimatorController = selectedAnimator.runtimeAnimatorController;
                       
                        }
                       

                    }
                    else // IF FAIRY SLOT IS EMPTY
                    {
                       
                        InventoryManager.instance.fairyHolder.GetComponent<Animator>().runtimeAnimatorController = null;
                        InventoryManager.instance.fairyHolder.GetComponent<SpriteRenderer>().sprite = null;
                    }

                }

                else
                {
                   

                    InventoryManager.instance.weaponHolder.GetComponent<Animator>().runtimeAnimatorController = null;
                    InventoryManager.instance.weaponHolder.GetComponent<SpriteRenderer>().sprite = null;
                }

                if (InventoryManager.instance.fairySlot == transform.GetSiblingIndex()) //IF FAIRY ITEM IS DROP TO FAIRY SLOT
                {
                  
                    Item selectedFairy = InventoryManager.instance.GetFairySlot(false);
                    if (selectedFairy != null)
                    {
                        Sprite itemSprite = selectedFairy.prefab.GetComponent<SpriteRenderer>().sprite;
                        Animator selectedAnimator = selectedFairy.prefab.GetComponent<Animator>();
                        InventoryManager.instance.fairyHolder.GetComponent<SpriteRenderer>().sprite = itemSprite;
                        if (selectedAnimator != null)
                        {
                            InventoryManager.instance.fairyHolder.GetComponent<Animator>().runtimeAnimatorController = selectedAnimator.runtimeAnimatorController;

                        }
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
                // IF HOLDABLE ITEM IS move SLOT TO "NOT SELECTED SLOT"
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

        //IF NON-HOLDABLE ITEM IS MOVE TO SELECTED SLOT
            else if (!droppedItem.item.holdable && InventoryManager.instance.selectedSlot == transform.GetSiblingIndex())
               // && InventoryManager.instance.GetFairySlot(false) == null) /////PLACEHOLDER
            {
                

                Item selectedItem = InventoryManager.instance.GetSelectedItem(false);
                if (!selectedItem.holdable)
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
            else
            {
                Item selectedItem = InventoryManager.instance.GetSelectedItem(false);
                // Disable animator if item is not holdable and has no animator
                if (selectedItem != null && !selectedItem.holdable)
                {
                    InventoryManager.instance.weaponHolder.GetComponent<Animator>().runtimeAnimatorController = null;
                   
                }
               
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
    private bool isPointerDown = false;
    private float holdDuration = .5f;
    private Coroutine holdCoroutine;

    public void OnPointerDown(PointerEventData eventData)
    {
        holdCoroutine = StartCoroutine(StartHoldTimer());
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPointerDown = false;
        if (holdCoroutine != null)
        {
            StopCoroutine(holdCoroutine);
        }
        itemInfoWindow.SetActive(false);
    }

    private IEnumerator StartHoldTimer()
    {
        isPointerDown = true;
        yield return new WaitForSeconds(holdDuration);

        if (isPointerDown)
        {
            InventoryItem itemInSlot = GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null)
            {
                // Access the item in the inventory slot
                Item selected = itemInSlot.item;

                // Update the UI elements with item information
                itemImage.sprite = selected.image;
                itemNameText.text = selected.name;
                itemDescriptionText.text = selected.description;
                itemDurabilityText.text = "Durability: " + itemInSlot.durability.ToString() + "/"+itemInSlot.item.maxDurability;

                // Show the item info window
                itemInfoWindow.SetActive(true);
            }
        }
    }


}
