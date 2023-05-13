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
        transform.SetParent(parentAfterDrag);
    }

    
    
}
