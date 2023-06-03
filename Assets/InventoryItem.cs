using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler,IEndDragHandler
{
        
    [Header("UI")]
    public Image image;
    public Text countTxt;

    [HideInInspector] public Item item;
    public int count = 1;
    [HideInInspector] public Transform parentAfterDrag;
    [HideInInspector] public Transform originalParent;
    public InventoryManager inventoryManager;
    public int durability;
    public Sprite emptyBottle;



    void Start()
    {
        inventoryManager = FindObjectOfType<InventoryManager>();
    }
    public void InitialiseItem(Item newItem, int itemCount)
    {
        if (newItem != null)
        {
            item = newItem;
            durability = newItem.durability;
            image.sprite = item.image;
            count = itemCount;

            Item selectedItem = InventoryManager.instance.GetSelectedItem(false);
            Item selectedFairy = InventoryManager.instance.GetFairySlot(false);
            if (selectedItem != null)
            {
                if (!selectedItem.holdable && selectedItem.type == ItemType.Weapon || selectedItem.type == ItemType.Tool)
                {
                    Sprite itemSprite = newItem.prefab.GetComponent<SpriteRenderer>().sprite;
                    if (itemSprite != null)
                    {
                        InventoryManager.instance.weaponHolder.GetComponent<SpriteRenderer>().sprite = itemSprite;
                    }
                    else
                    {
                        Debug.Log("Weapon spawn is not in the selected slot.");
                    }
                }
                else if (selectedItem.holdable)
                {
                    Sprite selectedSprite = selectedItem.prefab.GetComponent<SpriteRenderer>().sprite;
                    InventoryManager.instance.weaponHolder.GetComponent<SpriteRenderer>().sprite = selectedSprite;
                }
                else
                {
                    InventoryManager.instance.weaponHolder.GetComponent<SpriteRenderer>().sprite = null;
                }
            }
            if (selectedFairy != null)
            {
                if (selectedFairy.type == ItemType.Fairy)
                {
                    Animator fairyAnimator = selectedFairy.prefab.GetComponent<Animator>();
                    Sprite itemSprite = selectedFairy.prefab.GetComponent<SpriteRenderer>().sprite;
                    if (itemSprite != null)
                    {
                        InventoryManager.instance.fairyHolder.GetComponent<SpriteRenderer>().sprite = itemSprite;
                    }
                    if (fairyAnimator != null)
                    {
                        InventoryManager.instance.fairyHolder.GetComponent<Animator>().runtimeAnimatorController = fairyAnimator.runtimeAnimatorController;
                        Debug.Log("Fairy - to Empty slot : fairy animator continue");
                    }
                    else
                    {
                        InventoryManager.instance.fairyHolder.GetComponent<Animator>().runtimeAnimatorController = null;
                        InventoryManager.instance.fairyHolder.GetComponent<SpriteRenderer>().sprite = null;
                    }
                }
                else
                {
                    InventoryManager.instance.fairyHolder.GetComponent<Animator>().runtimeAnimatorController = null;
                    InventoryManager.instance.fairyHolder.GetComponent<SpriteRenderer>().sprite = null;
                }
            }
            else
            {
                InventoryManager.instance.weaponHolder.GetComponent<SpriteRenderer>().sprite = null;
                InventoryManager.instance.fairyHolder.GetComponent<SpriteRenderer>().sprite = null;
            }

            RefreshCount();
        }
    }

    public void RefreshCount()
    {
        bool textActive = count > 1;
        countTxt.gameObject.SetActive(textActive);
        countTxt.text = count.ToString();
        if(item.name == "Struggler Bottle" && count <= 0)
        {
            count = 0;
            image.sprite = emptyBottle;
        }
        else
        {
            image.sprite = item.image;
        }
    }
    public void OnBeginDrag(PointerEventData eventData)
{
    image.raycastTarget = false;
    originalParent = transform.parent;
    parentAfterDrag = transform.parent;

    transform.SetParent(transform.root);
        
        

    }

    public void OnDrag(PointerEventData eventData)
    {
      
        transform.position = Input.mousePosition;
        if (transform.parent.GetSiblingIndex() == 8)
        {
            // The current item is from equipmentSlots[7]
            Debug.Log("Dragging item from equipmentSlots[7]");
            // Add your desired logic here
        }

     

        if (item.type != ItemType.Fairy)
        {
            
            // Make the specific slot disappear or become inaccessible
            InventoryManager.instance.equipmentSlots[7].gameObject.SetActive(false);
        }
        

        if (item.type == ItemType.Fairy)
        {
            
            // Iterate through all the equipment slots
            for (int i = 0; i < InventoryManager.instance.inventorySlots.Length; i++)
            {
                InventoryItem existingItem = InventoryManager.instance.inventorySlots[i].GetComponentInChildren<InventoryItem>();

                // Check if the slot has a non-fairy item
                if (existingItem != null && existingItem.item.type != ItemType.Fairy)
                {
                    // Make the slot disappear or become inaccessible
                    InventoryManager.instance.inventorySlots[i].gameObject.SetActive(false);
                }
                else
                {
                    InventoryManager.instance.inventorySlots[i].gameObject.SetActive(true);
                }
            }
        }

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        image.raycastTarget = true;
        InventoryManager.instance.equipmentSlots[7].gameObject.SetActive(true);
        for (int i = 0; i < InventoryManager.instance.inventorySlots.Length; i++)
        {
            InventoryManager.instance.inventorySlots[i].gameObject.SetActive(true);
        }
        // If the item is not dropped into the fairy slot, leave it in its new slot
        Debug.Log("Item in regular slot/Swappers");
        transform.SetParent(parentAfterDrag);
    }


    }
