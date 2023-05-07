using UnityEngine;
using UnityEngine.UI;

public class ItemListHolder : MonoBehaviour
{
    public GridLayoutGroup gridLayoutGroup;
    public float paddingX = 0f;
    public float paddingY = -25f;
    public float minHeight;

    private RectTransform rectTransform;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void UpdateHeight()
    {
        int activeItemCount = 0;

        foreach (Transform child in transform)
        {
            if (child.gameObject.activeSelf)
            {
                activeItemCount++;
            }
        }

        float height = activeItemCount * (gridLayoutGroup.cellSize.y + gridLayoutGroup.spacing.y) - gridLayoutGroup.spacing.y + paddingY * 2f;
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, height);

        // Set child alignment to upper center if there are more items than columns, otherwise set it to middle center
        if (activeItemCount > gridLayoutGroup.constraintCount)
        {
            gridLayoutGroup.childAlignment = TextAnchor.UpperCenter;
        }
        else
        {
            gridLayoutGroup.childAlignment = TextAnchor.MiddleCenter;
        }
    }

}
