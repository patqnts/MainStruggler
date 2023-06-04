using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManagerUI : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject mapButtonUI;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(InventoryManager.instance.GetItemByName("Island Map") != null)
        {
            mapButtonUI.SetActive(true);
        }
        else
        {
            mapButtonUI.SetActive(false);
        }
    }
}
