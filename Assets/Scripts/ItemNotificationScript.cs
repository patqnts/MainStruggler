using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemNotificationScript : MonoBehaviour
{
    public GameObject NotificationObject;
    public Text textObject;
    public Image imageObject;
    
    public void DisableSelf()
    {
        gameObject.SetActive(false);
    }

    public void Notify(Item item)
    {
        imageObject.sprite = item.image;
        textObject.text = item.name+ " picked up!";
        NotificationObject.SetActive(true);
    }

}
