using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Visibility : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject self;
    private void Start()
    {
        self.SetActive(false);
    }
    private void OnBecameVisible()
    {
        if(self != null)
        {
            self.SetActive(true);
          
        }
        else
        {
            return;
        }
        
    }

    private void OnBecameInvisible()
    {

        if (self != null)
        {
            self.SetActive(false);
          
        }
        else
        {
            return;
        }
    }
}
