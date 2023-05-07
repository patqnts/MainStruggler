using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleItemsButton : MonoBehaviour
{
    public GameObject[] objectsToToggle;
 

    public bool isOn;

    private void Start()
    {
        isOn = false;
       
    }

    public void ToggleObjects()
    {
        isOn = !isOn;
        foreach (GameObject obj in objectsToToggle)
        {
            obj.SetActive(isOn);
        }
        
    }

   
}
