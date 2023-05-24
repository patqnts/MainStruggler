using UnityEngine;
using UnityEngine.UI;

public class DroppedItem : MonoBehaviour
{
    public Text countText;
    private int stackCount = 0;

    public void AddToStack(Item item, int count)
    {
        stackCount += count;
        //countText.text = stackCount.ToString();
        // You can also update the visual representation of the dropped item here if needed
    }
}
