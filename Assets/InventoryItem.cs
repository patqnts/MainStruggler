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
    [HideInInspector] public int count = 1;
    [HideInInspector] public Transform parentAfterDrag;
    [HideInInspector] public Transform originalParent;
    public InventoryManager inventoryManager;



    void Start()
    {
        inventoryManager = FindObjectOfType<InventoryManager>();
    }
    public void InitialiseItem(Item newItem)
    {
        if (newItem != null)
        {
            item = newItem;
            image.sprite = item.image;

            if (//InventoryManager.instance.GetSelectedItem(false) == null ||
                !InventoryManager.instance.GetSelectedItem(false).holdable &&
                InventoryManager.instance.GetSelectedItem(false) != null && 
                item.holdable == true &&
                InventoryManager.instance.GetSelectedItem(false).type == ItemType.Weapon || 
                
                InventoryManager.instance.GetSelectedItem(false) != null &&
                item.holdable == true && InventoryManager.instance.GetSelectedItem(false).type == ItemType.Tool)
            {
                // Instantiate the prefab and set its parent to the weapon holder
                Sprite itemSprite = newItem.prefab.GetComponent<SpriteRenderer>().sprite;
                InventoryManager.instance.weaponHolder.GetComponent<SpriteRenderer>().sprite = itemSprite;

                // Destroy the spawned item that was previously in the hand
            }
            else
            {
                if (InventoryManager.instance.GetSelectedItem(false) != null && InventoryManager.instance.GetSelectedItem(false).holdable)
                {
                    Sprite selectedSprite = InventoryManager.instance.GetSelectedItem(false).prefab.GetComponent<SpriteRenderer>().sprite;
                    InventoryManager.instance.weaponHolder.GetComponent<SpriteRenderer>().sprite = selectedSprite;
                }
                else
                {
                    InventoryManager.instance.weaponHolder.GetComponent<SpriteRenderer>().sprite = null;
                }
            }

            RefreshCount();
        }
    }
    public void RefreshCount()
    {
        bool textActive = count > 1;
        countTxt.gameObject.SetActive(textActive);
        countTxt.text = count.ToString();
    }
    public void OnBeginDrag(PointerEventData eventData)
{
    image.raycastTarget = false;
    originalParent = transform.parent;
    parentAfterDrag = transform.parent;

    transform.SetParent(transform.root);
        
        
        //prevent holded weapon from destroying while dragging other items
        if (inventoryManager != null && !InventoryManager.instance.GetSelectedItem(false))
        {
        Destroy(inventoryManager.spawnedItem);
        
        }
        if (InventoryManager.instance.selectedSlot == transform.GetSiblingIndex())
        {
            InventoryManager.instance.weaponHolder.GetComponent<SpriteRenderer>().sprite = null;
            InventoryManager.instance.weaponHolder.GetComponent<Animator>().runtimeAnimatorController = null;
        }

    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        image.raycastTarget = true;
        Debug.Log("Yeet");

        // Check if the item is a fairy and if the destination slot is the fairy slot
        if (item.type == ItemType.Fairy && transform.parent == InventoryManager.instance.equipmentSlots[7].transform)
        {
            // If a fairy is dropped into the fairy slot, leave it there
            Debug.Log("Fairy in fairy slot");
            transform.SetParent(parentAfterDrag);
        }
        else if (transform.parent == InventoryManager.instance.equipmentSlots[7].transform)
        {
            // If a non-fairy is dropped into the fairy slot, return it to its original slot
            Debug.Log("Non-fairy in fairy slot");
            transform.SetParent(originalParent);
            transform.position = originalParent.position;
        }
        else
        {
            // If the item is not dropped into the fairy slot, leave it in its new slot
            Debug.Log("Item in regular slot");
            transform.SetParent(parentAfterDrag);
        }

    }





}
