using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoScript : MonoBehaviour
{
    // Start is called before the first frame update
    public InventoryManager inventoryManager;
    public Item[] itemsToPickup;

    public void PickupItem(int id)
    {
        bool result = inventoryManager.AddItem(itemsToPickup[id]); 
        if (result == true)
        {
            Debug.Log("added");
        }
        else
        {
            Debug.Log("FULL");
        }
    }
    public void GetSelectedItem()
    {
        Item receivedItem = inventoryManager.GetSelectedItem(false);
        if(receivedItem != null)
        {
            Debug.Log("Received Item:" + receivedItem);
        }
        else
        {
            Debug.Log("No Item");
        }
    }
}
