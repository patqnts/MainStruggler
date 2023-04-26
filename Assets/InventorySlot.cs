using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public Image image;
    public Color selectedColor, notSelectedColor;



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
        if (transform.childCount == 0)
        {
            InventoryItem inventoryItem = eventData.pointerDrag.GetComponent<InventoryItem>();
            if (inventoryItem != null)
            {
                inventoryItem.parentAfterDrag = transform;
                Item item = inventoryItem.item;


                if (item != null && item.prefab != null && 
                    InventoryManager.instance.selectedSlot == transform.GetSiblingIndex() &&
                    item.holdable == true
                    )
                {
                    // Instantiate the prefab and set its parent to the weapon holder
                    GameObject newObject = Instantiate(item.prefab, InventoryManager.instance.weaponHolder.transform);
                    newObject.transform.localPosition = Vector3.zero;
                    newObject.transform.localRotation = Quaternion.identity;
                    newObject.transform.localScale = Vector3.one;

                    // Destroy the spawned item that was previously in the hand
                    if (InventoryManager.instance.spawnedItem != null )
                        
                    {
                        Destroy(InventoryManager.instance.spawnedItem);
                    }

                    // Set the new spawned item as the currently selected item
                    InventoryManager.instance.spawnedItem = newObject;
                }
            }
        }
    }



}
