using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMenuScript : MonoBehaviour
{
    // Start is called before the first frame update
    public bool isOpen = false;
    public GameObject gameMenuObject;

    public void OnClick()
    {
        if (!isOpen)
        {
            gameMenuObject.SetActive(true);
            isOpen = true;
        }
        else
        {
            gameMenuObject.SetActive(false);
            isOpen = false;
        }
    }
}
