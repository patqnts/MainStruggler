using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class seedCodeCheckbox : MonoBehaviour
{
    public bool isOpen = false;
    public GameObject seedInput;

    public void Open()
    {
        if (!isOpen)
        {
            isOpen = true;
            seedInput.SetActive(true);
        }
        else
        {
            isOpen = false;
            seedInput.SetActive(false);
        }
    }
}
