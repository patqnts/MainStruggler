using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HolderScripts : MonoBehaviour
{
    public Transform childTransform;

    // Update the parent's position based on the child's position
    void Update()
    {
        if(childTransform != null)
        {
            // Update the position of the parent to match the child's position
            transform.position = childTransform.position;
        }
        else
        {
            return;
        }
        
    }
}
