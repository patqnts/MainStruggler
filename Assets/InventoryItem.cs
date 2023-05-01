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

            if (InventoryManager.instance.GetSelectedItem(false) != null && 
                item.holdable == true &&
                InventoryManager.instance.GetSelectedItem(false).type == ItemType.Weapon || 
                
                InventoryManager.instance.GetSelectedItem(false) != null &&
                item.holdable == true && InventoryManager.instance.GetSelectedItem(false).type == ItemType.Tool)
            {
                // Instantiate the prefab and set its parent to the weapon holder
                GameObject newObject = Instantiate(InventoryManager.instance.GetSelectedItem(false).prefab, InventoryManager.instance.weaponHolder.transform);
                newObject.transform.localPosition = Vector3.zero;
                newObject.transform.localRotation = Quaternion.identity;
                newObject.transform.localScale = Vector3.one;

                // Destroy the spawned item that was previously in the hand
                if (InventoryManager.instance.spawnedItem != null && item.holdable == true)

                {
                    Destroy(InventoryManager.instance.spawnedItem);
                }
                InventoryManager.instance.spawnedItem = newObject;

                // Set the new spawned item as the currently selected item
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
