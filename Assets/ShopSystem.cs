using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ShopSystem : MonoBehaviour
{

    public ItemDetailsPanel detailsPanel;
    public Item item; // The scriptable object for the item
    public UnityEvent<Item> OnClick; // The UnityEvent for the item click event
    public Image itemImage; // The UI image for the item sprite

    // Attach this method to the button's click event in the inspector
    public void OnButtonClick()
    {
        // Notify listeners of the click event
        OnClick.Invoke(item);

    }

    // Use this method to update the button UI with the item data
    public void UpdateUI()
    {
        // Update the item image
        itemImage.sprite = item.image;
    }
}
