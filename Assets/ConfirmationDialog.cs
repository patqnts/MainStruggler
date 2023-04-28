using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfirmationDialog : MonoBehaviour
{


    public InventorySlot itemToRemove;
    public void ShowConfirmationBox()
    {
        // Set the UI Image object as active to show the confirmation box
        gameObject.SetActive(true);
    }


    public void ConfirmRemoval()
    {
        // Remove the item from the inventory slot
        itemToRemove.RemoveItem();

        // Set the UI Image object as inactive to hide the confirmation box
        gameObject.SetActive(false);
        itemToRemove.hasBeenClicked = false;
    }

    public void CancelRemoval()
    {
        // Set the UI Image object as inactive to hide the confirmation box
        gameObject.SetActive(false);
        itemToRemove.hasBeenClicked = false;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
