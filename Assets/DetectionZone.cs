using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionZone : MonoBehaviour
{
    public List<Collider2D> detectedObj = new List<Collider2D>();
    public Collider2D coll;
    public string Target = "Player";
    

   

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == Target)
        {
            detectedObj.Add(collision); 
        }
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        detectedObj.Remove(collision);
    }
}
