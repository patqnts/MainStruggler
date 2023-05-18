using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoldButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool isHolding = false;

    public void OnPointerDown(PointerEventData eventData)
    {
        isHolding = true;
        StartCoroutine(HoldCoroutine());
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isHolding = false;
    }

    private IEnumerator HoldCoroutine()
    {
        while (isHolding)
        {
           
           

            yield return null;
        }
    }
}
