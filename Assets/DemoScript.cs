using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class DemoScript : MonoBehaviour
{
    // Start is called before the first frame update
    public InventoryManager inventoryManager;
    public Item[] itemsToPickup;


    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void PickupItem(int id)
    {
       

        if (id == 0)       
        {
            for (int x = 0; x < 999; x++)
            {
                inventoryManager.AddItem(itemsToPickup[id],itemsToPickup[id].maxDurability,1);
            }
        }
        else
        {
            bool result = inventoryManager.AddItem(itemsToPickup[id], itemsToPickup[id].maxDurability,1);
            if (result == true)
            {
                Debug.Log("added");
            }
            else
            {
                Debug.Log("FULL");
            }
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
