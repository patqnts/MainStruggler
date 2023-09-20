using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapButton : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject FairyUI;
    public bool isOpen = false;
    public GameObject mapCam;

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
            mapCam.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
            isOpen = false;
            FairyUI.SetActive(false);
            mapCam.SetActive(false);
            Time.timeScale = 1;
        }
    }
}
