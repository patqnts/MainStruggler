using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FairyUIScript : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject fairyButton;
    public GameObject wispnotif;
    void Start()
    {
        
    }
    private void OnEnable()
    {
       
       
    }
    // Update is called once per frame
    void Update()
    {
        if (InventoryManager.instance.GetFairySlot(false) != null)
        {
            fairyButton.SetActive(true);
            if (wispnotif != null)
            {
                wispnotif.SetActive(true);
            }
        }
        else
        {
            fairyButton.SetActive(false);
        }
    }
}
