using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ConfirmDrop : MonoBehaviour
{

    public GameObject box;
   
   
    
    public void Close()
    {
       
        box.SetActive(false);
    }
    public void Open()
    {

        box.SetActive(true);
    }
}
