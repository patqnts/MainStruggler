using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SlotButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, ISelectHandler, IDeselectHandler
{
    public Image itemImage;
    public Text itemNameText;
    public Text itemDescriptionText;
    public Text itemDurabilityText;
    public GameObject itemInfoWindow;

    private InventoryItem currentItem;

    public void OnPointerDown(PointerEventData eventData)
    {
        // Handle pointer down event (e.g., for hold functionality)
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // Handle pointer up event (e.g., for hold functionality)
    }

    public void OnSelect(BaseEventData eventData)
    {
        // Show item information when the button is selected

        // Check if there's a valid item assigned to this slot
        if (currentItem != null)
        {
            // Update the UI elements with item information
            itemImage.sprite = currentItem.item.image;
            itemNameText.text = currentItem.item.name;
            itemDescriptionText.text = currentItem.item.description;
            itemDurabilityText.text = "Durability: " + currentItem.durability.ToString();

            // Show the item info window
            itemInfoWindow.SetActive(true);
        }
    }

    public void OnDeselect(BaseEventData eventData)
    {
        // Hide the item information when the button is deselected
        itemInfoWindow.SetActive(false);
    }

    // Method to set the current item for this slot
    public void SetItem(InventoryItem item)
    {
        currentItem = item;
    }
}
