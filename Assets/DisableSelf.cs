using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableSelf : MonoBehaviour
{

    public void DestroySelf()
    {
        Destroy(gameObject);
    }

    public void TurnOffSelf()
    {
        gameObject.SetActive(false);
    }
}
