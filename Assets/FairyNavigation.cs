using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FairyNavigation : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject FairyUI;
    public bool isOpen = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Fiary()
    {
        if (!isOpen)
        {
            isOpen = true;
            FairyUI.SetActive(true);
        }
        else
        {
            isOpen = false;
            FairyUI.SetActive(false);
        }
    }
}
